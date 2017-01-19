using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CalculatorCommon
{
    public abstract class OperationResult
    {
        public abstract String ToFormattedString();

        /// <summary>
        /// Try to parse user input into a OperationResult child class
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Appropriate filled OperationResult on success, null otherwise</returns>
        public OperationResult TryParse(String input)
        {
            try
            {
                return (OperationResult)JsonConvert.DeserializeObject(input, GetType());
            }
            catch
            {
                return null;
            }
        }
    }
}
