using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CalculatorCommon.Operations;
using CalculatorCommon;
using System.Web.Script.Serialization;

namespace Calculator
{
    /// <summary>
    /// Save and retrieve journal entries.
    /// TODO: Actually use a database engine, as queries are simple sqlite or sqlce would do.
    /// </summary>
    public static class Database
    {
        // log4net logger
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private const String databaseFile = "CalculatorTracking.csv";
        private const String CSV_SEPARATOR = ";";

        private static Object Mutex = new Object();

        /// <summary>
        /// Check if tracking is enabled and, in that case, save the operation persistently
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="trackingId"></param>
        public static void SaveOperationIfNeeded(IOperation operation, int trackingId)
        {
            if (trackingId == -1)
                return;

            lock (Mutex)
            {
                File.AppendAllText(databaseFile,
                    trackingId.ToString() + CSV_SEPARATOR +
                    operation.OperationCode + CSV_SEPARATOR +
                    operation.ToFormattedString() + CSV_SEPARATOR +
                    DateTime.Now.ToString() + 
                    Environment.NewLine);
            }
        }

        /// <summary>
        /// Get all journal entries for a given trackingId
        /// </summary>
        /// <param name="trackingId">trackingId to look for</param>
        /// <returns></returns>
        public static JournalResponse GetJournalEntries(string trackingId)
        {
            JournalResponse response = new JournalResponse();
            lock (Mutex)
            {
                foreach (String entry in File.ReadLines(databaseFile))
                {
                    if (entry.StartsWith(trackingId + CSV_SEPARATOR))
                    {
                        String[] entryTokens = entry.Split(new String[] { CSV_SEPARATOR }, StringSplitOptions.None );
                        response.Operations.Add(new JournalEntry
                        {
                            Operation = entryTokens[1],
                            Calculation = entryTokens[2],
                            Date = entryTokens[3]
                        });
                    }
                }
            }
            return response;
        }
    }
}