using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorCommon
{
    public class JournalRequest
    {
        public String Id;
    }

    public class JournalResponse
    {
        public List<JournalEntry> Operations { get; set; } = new List<JournalEntry>();

        /// <summary>
        /// Make a JournalResponse into a readable string
        /// </summary>
        /// <returns>Human-readable string representing a JournalEntry</returns>
        public String ToFormattedString()
        {
            if (Operations == null || Operations.Count < 1)
                return "Empty journal entry";

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < Operations.Count; i++)
            {
                sb.Append(Operations[i].ToFormattedString());
                if (i != Operations.Count - 1)
                    sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }

    public class JournalEntry
    {
        /// <summary>
        /// Short code for the operation
        /// </summary>
        public String Operation { get; set; }

        /// <summary>
        /// The human-readable string representing the operation
        /// </summary>
        public String Calculation { get; set; }

        /// <summary>
        /// When the operation was received by the server
        /// </summary>
        public String Date { get; set; }

        public String ToFormattedString()
        {
            return Operation + ", " + Calculation + ", " + Date;
        }
    }
}
