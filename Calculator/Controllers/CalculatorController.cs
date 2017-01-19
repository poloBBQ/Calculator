using CalculatorCommon;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Calculator.Controllers
{
    /// <summary>
    /// To submit operations
    /// </summary>
    public class CalculatorController : ApiController
    {
        // log4net logger
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [ActionName("Add")]
        public IHttpActionResult Add([FromBody]CalculatorCommon.Operations.Summation operation, Int32 trackingId = -1)
        {
            try
            {
                Log.Info("Add operation requested");
                OperationResult summationResult = operation.Calculate();
                Database.SaveOperationIfNeeded(operation, trackingId);
                return Ok(summationResult);
            }
            catch (FormatException ex)
            {
                Log.Error(ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("Sub")]
        public IHttpActionResult Sub([FromBody]CalculatorCommon.Operations.Subtraction operation, Int32 trackingId = -1)
        {
            try
            {
                Log.Info("Subtract operation requested");
                OperationResult subtractionResult = operation.Calculate();
                Database.SaveOperationIfNeeded(operation, trackingId);
                return Ok(subtractionResult);
            }
            catch (FormatException ex)
            {
                Log.Error(ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("Mult")]
        public IHttpActionResult Mult([FromBody]CalculatorCommon.Operations.Multiplication operation, Int32 trackingId = -1)
        {
            try
            {
                Log.Info("Multiplicate operation requested");
                OperationResult multiplicationResult = operation.Calculate();
                Database.SaveOperationIfNeeded(operation, trackingId);
                return Ok(multiplicationResult);
            }
            catch (FormatException ex)
            {
                Log.Error(ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("Div")]
        public IHttpActionResult Div([FromBody]CalculatorCommon.Operations.Division operation, Int32 trackingId = -1)
        {
            try
            {
                Log.Info("Division operation requested");
                OperationResult divisionResult = operation.Calculate();
                Database.SaveOperationIfNeeded(operation, trackingId);
                return Ok(divisionResult);
            }
            catch (FormatException ex)
            {
                Log.Error(ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("Sqrt")]
        public IHttpActionResult Sqrt([FromBody]CalculatorCommon.Operations.Sqrt operation, Int32 trackingId = -1)
        {
            try
            {
                Log.Info("Square root operation requested");
                OperationResult sqrtResult = operation.Calculate();
                Database.SaveOperationIfNeeded(operation, trackingId);
                return Ok(sqrtResult);
            }
            catch (FormatException ex)
            {
                Log.Error(ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return InternalServerError(ex);
            }
        }
    }
}
