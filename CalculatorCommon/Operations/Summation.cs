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
    public class Summation : IOperation
    {
        [JsonProperty(Required = Required.Always)]
        public List<int> Addends { get; set; } = new List<int>();

        [ScriptIgnore] // Ignore for json serialization
        public string OperationCode
        {
            get
            {
                return "add";
            }
        }

        public OperationResult Calculate()
        {
            if (Addends == null || Addends.Count < 2)
                throw new FormatException("Summation requires at least two addends");

            return new SummationResult { Sum = Addends.Sum() };
        }

        public String ToFormattedString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < Addends.Count; i++)
            {
                sb.Append(Addends[i]);
                if (i != Addends.Count - 1)
                    sb.Append(" + ");
            }
            sb.Append(" = ");
            sb.Append((Calculate() as SummationResult).Sum);
            return sb.ToString();
        }

        public bool TryParse(String input)
        {
            try
            {
                // Remove + and whitespaces to see if the result is all numerical
                if (Regex.IsMatch(input.Replace("+", "").Replace(" ", ""), @"^\d+$"))
                {
                    foreach (String token in input.Split('+'))
                    {
                        Addends.Add(Int32.Parse(token.Trim()));
                    }
                    return true;
                }

                return false;
            }
            catch { return false; }
        }
    }

    public class SummationResult : OperationResult
    {
        [JsonProperty(Required = Required.Always)]
        public int Sum { get; set; }

        public override String ToFormattedString()
        {
            return "Sum: " + Sum;
        }
    }
}
