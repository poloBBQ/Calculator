using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CalculatorCommon.Operations
{
    public class Subtraction : IOperation
    {
        [JsonProperty(Required = Required.Always)]
        public int Minuend { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Subtrahend { get; set; }

        [ScriptIgnore] // Ignore for json serialization
        public string OperationCode
        {
            get
            {
                return "sub";
            }
        }

        public OperationResult Calculate()
        {
            return new SubtractionResult { Difference = Minuend - Subtrahend };
        }

        public String ToFormattedString()
        {
            return Minuend + " - " + Subtrahend + " = " + (Calculate() as SubtractionResult).Difference;
        }

        public bool TryParse(String input)
        {
            try
            {
                // Remove whitespaces to see if minus sign is between numbers.
                if (Regex.IsMatch(input.Replace(" ", ""), @"\d+-\d+$"))
                {
                    String[] tokens = input.Split('-');

                    Minuend = Int32.Parse(tokens[0]);
                    Subtrahend = Int32.Parse(tokens[1]);
                    
                    return true;
                }

                return false;
            }
            catch { return false; }
        }
    }

    public class SubtractionResult : OperationResult
    {
        [JsonProperty(Required = Required.Always)]
        public int Difference { get; set; }

        public override String ToFormattedString()
        {
            return "Difference: " + Difference;
        }
    }
}
