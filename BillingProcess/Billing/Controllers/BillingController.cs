using BillingProcess.Billing.DllApi;
using BillingProcess.Billing.Models;
using BillingProcess.Entities;
using BillingProcess.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BillingProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        #region Constructor
        private readonly BillingApi _billingApi;

        public BillingController(BillingApi billingApi)
        {
            _billingApi = billingApi;
        }
        #endregion

        #region GET Generate invoice
        /// <summary>
        /// Generate invoice through CDR records
        /// </summary>
        /// <param name="cdrModels"> Manual list of CDR records </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<InvoiceApiModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GenerateInvoice([FromBody] List<CDRApiModel> cdrModels)
        {
            try
            {
                if (cdrModels.Count > 0)
                {
                    return Ok(await _billingApi.GenerateInvoiceAsync(cdrModels: cdrModels));
                }
                else
                {
                    return BadRequest("Failed to find any CDR records");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to generate invoice", e);
            }
        }
        #endregion

        #region POST Generate invoice via file
        /// <summary>
        /// Generate invoice via CDR file
        /// </summary>
        /// <param name="file"> File input of type csv </param>
        /// <returns></returns>
        [HttpPost("file")]
        [ProducesResponseType(typeof(List<InvoiceApiModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GenerateInvoiceViaFile([FromForm] FileApiModel file)
        {
            try
            {
                if (file.File != null && file.File.Length > 0)
                {
                    if (Path.GetExtension(file.File.FileName) != ".csv")
                        return BadRequest("Invalid file type");
                    else
                    return Ok(await _billingApi.GenerateInvoiceViaFileAsync(file.File));
                }
                else
                {
                    return BadRequest("No file found");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to read CDR file", e);
            }
        }
        #endregion
    }
}
