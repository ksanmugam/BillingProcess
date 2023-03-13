using BillingProcess.Billing.Services;
using BillingProcess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Billing.DllApi
{
    public class BillingApi
    {
        #region Constructor
        private readonly BillingService _billingService;
        public BillingApi(BillingService billingService)
        {
            _billingService = billingService;
        }
        #endregion

        #region Generate invoice
        /// <summary>
        /// Generate invoice through CDR records
        /// </summary>
        /// <param name="cdrModels"> Manual list of CDR records </param>
        /// <returns></returns>
        internal async Task<List<InvoiceApiModel>> GenerateInvoiceAsync(List<CDRApiModel> cdrModels)
        {
            try
            {
                return _billingService.GenerateInvoice(cdrModels: cdrModels);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to generate invoice", e);
            }
        }
        #endregion

        #region Generate invoice via file
        /// <summary>
        /// Generate invoice via CDR file
        /// </summary>
        /// <param name="file"> File input of type csv </param>
        /// <returns></returns>
        internal async Task<List<InvoiceApiModel>> GenerateInvoiceViaFileAsync(IFormFile file)
        {
            try
            {
                string fileContent = null;
                List<CDRApiModel> cdrModels = new List<CDRApiModel>();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    reader.ReadLine();
                    fileContent = reader.ReadToEnd();

                    foreach(string row in fileContent.Split("\r\n"))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            string[] line = row.Split(',');
                            cdrModels.Add(new CDRApiModel(long.Parse(line[0]), line[1], int.Parse(line[2]), long.Parse(line[3]), long.Parse(line[4]), line[5]));
                        }
                    }
                    return _billingService.GenerateInvoice(cdrModels);
                }

            }
            catch (Exception e)
            {
                throw new Exception("Failed to generate invoice via file", e);
            }

        }
        #endregion
    }
}
