using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorCommon
{
    /// <summary>
    /// Interface that represents an arithmetic operation.
    /// A common interface simplifies usage and facilitates serialization.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Short code for the operation
        /// </summary>
        String OperationCode { get; }

        /// <summary>
        /// Calculate the result of the operation.
        /// It doesn't make much sense to have it here, as the client could calculate it by itself,
        /// but there is no reason not to do so, as the server doesn't add any neccessary information
        /// for this function.
        /// </summary>
        /// <returns>Specific IOperationResult of the operation</returns>
        OperationResult Calculate();

        /// <summary>
        /// Make the operation human-readable
        /// </summary>
        /// <returns>Operation in a readable format</returns>
        String ToFormattedString();

        /// <summary>
        /// Check if input can be parsed to the current class of the IOperation
        /// </summary>
        /// <param name="input">Operation input, maybe invalid</param>
        /// <returns>True if successful, false otherwise</returns>
        Boolean TryParse(String input);
    }
}
