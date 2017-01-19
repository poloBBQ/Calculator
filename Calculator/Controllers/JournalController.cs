using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Calculator.Controllers
{
    /// <summary>
    /// Controller for queries to the journal
    /// </summary>
    public class JournalController : ApiController
    {
        /// <summary>
        /// Get all entries for a given trackingId
        /// </summary>
        /// <param name="journalRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Query")]
        public IHttpActionResult Query([FromBody]CalculatorCommon.JournalRequest journalRequest)
        {
            return Ok(Database.GetJournalEntries(journalRequest.Id));
        }
    }
}
