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
    public class Division : IOperation
    {
        [JsonProperty(Required = Required.Always)]
        public int Dividend { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Divisor { get; set; }

        [ScriptIgnore] // Ignore for json serialization
        public string OperationCode
        {
            get
            {
                return "div";
            }
        }

        public OperationResult Calculate()
        {
            return new DivisionResult
            {
                Quotient = Dividend / Divisor,
                Rest = Dividend % Divisor
            };
        }

        public String ToFormattedString()
        {
            DivisionResult result = Calculate() as DivisionResult;
            return Dividend + " / " + Divisor + " = " + result.Quotient + " and " + result.Rest;
        }

        public bool TryParse(String input)
        {
            try
            {
                // Remove whitespaces to see if div sign is between numbers.
                if (Regex.IsMatch(input.Replace(" ", ""), @"\d+\/\d+$"))
                {
                    String[] tokens = input.Split('/');

                    Dividend = Int32.Parse(tokens[0]);
                    Divisor = Int32.Parse(tokens[1]);

                    return true;
                }

                return false;
            }
            catch { return false; }
        }
    }

    public class DivisionResult : OperationResult
    {
        [JsonProperty(Required = Required.Always)]
        public int Quotient { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Rest { get; set; }

        public override String ToFormattedString()
        {
            return "Divison: " + Quotient + " and " + Rest;
        }
    }
}
