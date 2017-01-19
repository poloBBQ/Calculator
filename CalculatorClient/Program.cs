using CalculatorCommon;
using log4net;
using NDesk.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculatorClient
{
    class Program
    {
        // log4net logger
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            try
            {
                Log.Info("Calculator client started");

                #region Input Arguments
                String server = null;
                Int32? port = null;
                Boolean? ShowHelp = null;
                OptionSet p = new OptionSet() {
                    { "s|server=", "the {SERVER} where CalculatorServer is running.",
                       v => server=v },
                    { "p|port=",
                       "the {PORT} to access CalculatorServer.",
                        (int v) => port = v },
                    { "h|help",  "show this help message.",
                       v => ShowHelp = v != null },
                };

                try
                {
                    p.Parse(args);
                }
                catch (OptionException)
                {
                    Console.WriteLine("Run --help for parameters syntax.");
                    return;
                }

                if ((ShowHelp != null && ShowHelp.Value)
                    || port == null
                    || server == null)
                {
                    p.WriteOptionDescriptions(Console.Out);
                    return;
                }
                #endregion

                Boolean Exit = false;
                do
                {
                    try
                    {
                        String userInput;
                        IOperation operation = GetOperation(out userInput);

                        if (operation != null) // Check if user input was actually an operation
                        {
                            Console.Write("Track this operation? (trackingId or empty): ");
                            String trackInput = Console.ReadLine();
                            int trackingId;
                            if (!Int32.TryParse(trackInput, out trackingId))
                                trackingId = -1;

                            SendOperationAndPrintResult(operation, server, port.ToString(), trackingId);
                        }
                        else // User input was not an operation
                        {
                            if (userInput.ToLower() == "exit")
                            {
                                Log.Info("Calculator client exiting");

                                Console.WriteLine("Happy to serve you whenever you need me :)");
                                Exit = true;
                            }

                            if (!Exit && userInput.ToLower().StartsWith("journal")) // User wants to see the journal
                            {
                                //Remove non-numbers from the input
                                String inputWithoutPrefix = userInput.Replace("journal", "").Replace(" ", "");
                                int trackingId;
                                if (Int32.TryParse(inputWithoutPrefix, out trackingId))
                                {
                                    GetAndPrintJournalEntry(trackingId, server, port.ToString());
                                }
                            }
                            else if(!Exit) // Command not understood
                            {
                                Console.WriteLine("Sorry, I didn't hear, could you speak a little louder, please?");
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Log.Error(ex);
                        Console.WriteLine("I'm sorry Dave, I'm afraid I can't do that");
                    }   
                }
                while (!Exit);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Retrieve journal entry from the server and print it.
        /// </summary>
        /// <param name="trackingId">Id of the jornal entry to request</param>
        /// <param name="host">Host to send the request to</param>
        /// <param name="port">Port to send the request to</param>
        static void GetAndPrintJournalEntry(int trackingId, String host, String port)
        {
            JournalRequest journalRequest = new JournalRequest() { Id = trackingId.ToString() };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + host + ":" + port + "/journal/query");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(journalRequest);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(JsonConvert.DeserializeObject<JournalResponse>(result).ToFormattedString());
            }
        }

        /// <summary>
        /// Send IOperation to the server and print the result
        /// </summary>
        /// <param name="operation">Operation to calculate</param>
        /// <param name="host">Host to send the request to</param>
        /// <param name="port">Port to send the request to</param>
        /// <param name="trackingId">TrackingId to track the operation at the server. -1 to disable tracking</param>
        static void SendOperationAndPrintResult(IOperation operation, String host, String port, int trackingId = -1)
        {
            String requestUri = "http://" + host + ":" + port + "/calculator/" + operation.OperationCode;
            if(trackingId != -1)
            {
                requestUri += "?trackingId=" + trackingId.ToString();
            }
            WebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                String json = JsonConvert.SerializeObject(operation);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                String result = streamReader.ReadToEnd();

                // Get all types that are non-abstract classes inheriting from OperationResult
                IEnumerable<Type> types = Assembly.GetAssembly(typeof(OperationResult)).GetTypes()
                    .Where(p => p.IsClass && !p.IsAbstract && p.IsSubclassOf(typeof(OperationResult)));

                // Find the OperationResult subclass that represents the received result by trying to parse it
                foreach (Type t in types)
                {
                    OperationResult operationResult = (OperationResult)Activator.CreateInstance(t);
                    operationResult = operationResult.TryParse(result);
                    if (operationResult != null)
                    {
                        Console.WriteLine(operationResult.ToFormattedString());
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to get an IOperation instance from the user input
        /// </summary>
        /// <param name="input">Input by the user</param>
        /// <returns></returns>
        static IOperation GetOperation(out string input)
        {
            Console.Write(
                Environment.NewLine +
                "Option examples: " + Environment.NewLine +
                "\"1+5+3\" for Summation" + Environment.NewLine +
                "\"1-5\" for Subtraction" + Environment.NewLine +
                "\"1*5*3\" for Multiplication" + Environment.NewLine +
                "\"1/5\" for Division" + Environment.NewLine +
                "\"sqrt 1\" for Square root" + Environment.NewLine +
                "\"journal 1\" to show past entries for trackingId 1" + Environment.NewLine +
                "\"exit\" to exit" + Environment.NewLine +
                "Option: "
                );
            
            input = Console.ReadLine();

            // Get all types from all assemblies that can be assigned from an IOperation and are not interfaces.
            // Meaning get all classes that implement IOperation.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IOperation).IsAssignableFrom(p) && !p.IsInterface);

            // Find the IOperation class that represents the input by trying to parse the input
            foreach (Type t in types)
            {
                IOperation operation = (IOperation)Activator.CreateInstance(t);
                if (operation.TryParse(input))
                    return operation;
            }

            return null;
        }
    }
}
