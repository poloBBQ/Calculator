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
    public class Sqrt : IOperation
    {
        [JsonProperty(Required = Required.Always)]
        public int Number { get; set; }

        [ScriptIgnore] // Ignore for json serialization
        public string OperationCode
        {
            get
            {
                return "sqrt";
            }
        }

        public OperationResult Calculate()
        {
            // Specs don't seem to specify what should happen when a number doesn't have
            // a perfect square root. Decided to round.
            return new SqrtResult { Square = Convert.ToInt32(Math.Sqrt(Number)) };
        }

        public String ToFormattedString()
        {
            return "Sqrt " + Number + " = " + (Calculate() as SqrtResult).Square;
        }

        public bool TryParse(String input)
        {
            try
            {
                // Match to sqrt X or sqrtX
                if (Regex.IsMatch(input, @"^sqrt *\d+$"))
                {
                    Number = Int32.Parse(input.Replace("sqrt", "").Trim());
                    return true;
                }

                return false;
            }
            catch { return false; }
        }
    }

    public class SqrtResult : OperationResult
    {
        [JsonProperty(Required = Required.Always)]
        public int Square { get; set; }

        public override String ToFormattedString()
        {
            return "Square: " + Square;
        }
    }
}
