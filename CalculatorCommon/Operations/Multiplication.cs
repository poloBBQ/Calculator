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
    public class Multiplication : IOperation
    {
        [JsonProperty(Required = Required.Always)]
        public List<int> Factors { get; set; } = new List<int>();

        [ScriptIgnore] // Ignore for json serialization
        public string OperationCode
        {
            get
            {
                return "mult";
            }
        }

        public OperationResult Calculate()
        {
            if (Factors == null || Factors.Count < 2)
                throw new FormatException("Multiplication requires at least two addends");

            return new MultiplicationResult { Product = Factors.Aggregate((a, b) => a * b) };
        }

        public String ToFormattedString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Factors.Count; i++)
            {
                sb.Append(Factors[i]);
                if (i != Factors.Count - 1)
                    sb.Append(" * ");
            }
            sb.Append(" = ");
            sb.Append((Calculate() as MultiplicationResult).Product);
            return sb.ToString();
        }

        public bool TryParse(String input)
        {
            try
            {
                // Remove * and whitespaces to see if the result is all numerical
                if (Regex.IsMatch(input.Replace("*", "").Replace(" ", ""), @"^\d+$"))
                {
                    foreach (String token in input.Split('*'))
                    {
                        Factors.Add(Int32.Parse(token.Trim()));
                    }
                    return true;
                }

                return false;
            }
            catch { return false; }
        }
    }

    public class MultiplicationResult : OperationResult
    {
        [JsonProperty(Required = Required.Always)]
        public int Product { get; set; }

        public override String ToFormattedString()
        {
            return "Product: " + Product;
        }
    }
}
