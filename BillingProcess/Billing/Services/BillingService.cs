using BillingProcess.Billing.Mappers;
using BillingProcess.Billing.Models;
using BillingProcess.Entities;
using BillingProcess.Enums;
using BillingProcess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BillingProcess.Billing.Services
{
    public class BillingService
    {
        #region Constructor
        private readonly DatabaseContext _dbContext;

        public BillingService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Generate invoice
        /// <summary>
        /// Generate invoice through CDR records
        /// </summary>
        /// <param name="cdrModels"> Manual list of CDR records </param>
        /// <returns></returns>
        internal List<InvoiceApiModel> GenerateInvoice(List<CDRApiModel> cdrModels)
        {
            // generate a list of invoices for each company by user
            return GenerateInvoiceFilters(cdrModels: cdrModels);
        }

        /// <summary>
        /// Generate invoice through CDR records
        /// </summary>
        /// <param name="cdrModels"> Manual list of CDR records </param>
        /// <returns></returns>
        internal List<InvoiceApiModel> GenerateInvoiceFilters(List<CDRApiModel> cdrModels)
        {
            try
            {
                List<InvoiceApiModel> invoices = new List<InvoiceApiModel>();
                // iterate through each cdr record
                foreach (var cdr in cdrModels)
                {
                    switch (cdr.Direction)
                    {
                        // person calling gets charged
                        case "OUTBOUND":
                            // get user making the call
                            var userCalling = _dbContext.Users
                                                .Include(c => c.Company)
                                                .ThenInclude(p => p.Plan)
                                                .Where(a => a.PhoneNumber == cdr.SourceNumber.ToString())
                                                .AsNoTracking()
                                                .FirstOrDefault() ?? throw new ArgumentException("Failed to retrieve user");
                            if (userCalling != null)
                            {
                                // get user rates based on rate type matching with direction
                                var userRate = _dbContext.Rates
                                                .Include(p => p.Plan)
                                                .Where(r => r.Plan.Id == userCalling.Company.Plan.Id && r.RateType == RateType.OutboundCall.ToString()).OrderBy(a => a.Priority)
                                                .AsNoTracking().ToList() ?? throw new ArgumentException("Failed to retrieve user rate");

                                // check if invoice against a company exist
                                if (invoices.Where(a => a.Company.Id == userCalling.Company.Id).Any())
                                {
                                    // get existing company invoice
                                    var existingInvoice = invoices.Where(a => a.Company.Id == userCalling.Company.Id).FirstOrDefault();
                                    // check if user has a record in the company
                                    if (existingInvoice.UserCalls.Where(u => u.User.Id == userCalling.Id).Any())
                                    {
                                        if (userRate != null)
                                        {
                                            // get exising user call records
                                            var existingUserRecord = invoices.Where(a => a.Company.Id == userCalling.Company.Id).Select(a => a.UserCalls.Where(a => a.User.Id == userCalling.Id).FirstOrDefault()).FirstOrDefault();
                                            // get user rate based on destination number against filter
                                            var getFilter = userRate.FirstOrDefault(a => a.Filter == "*" || Regex.IsMatch(cdr.DestinationNumber.ToString(), a.Filter));

                                            // formula to calculate call charge
                                            var callCharge = cdr.Duration * (getFilter.RateValue / 60);
                                            var callDetail = new CallDetailApiModel(Guid.NewGuid(), cdr.ConnectDateTime, new TimeSpan(0, 0, cdr.Duration).ToString(), (getFilter.RateValue / 60), callCharge, cdr.DestinationNumber.ToString());

                                            // update existing user call record with new call charge
                                            existingUserRecord.UpdateUserCharge(1, callCharge, cdr.Duration, callDetail, DirectionType.OUTBOUND);
                                        }

                                    }
                                    // create a new user record in the company
                                    else
                                    {
                                        if (userRate != null)
                                        {
                                            // get user rate based on destination number against filter
                                            var getFilter = userRate.FirstOrDefault(a => a.Filter == "*" || Regex.IsMatch(cdr.DestinationNumber.ToString(), a.Filter));

                                            // formula to calculate call charge
                                            var callCharge = cdr.Duration * (getFilter.RateValue / 60);

                                            // add new user call record against the company invoice
                                            existingInvoice.UserCalls.Add(new UserChargeApiModel(Guid.NewGuid(), UserMapper.EntityToApi(userCalling), 1, callCharge, cdr.Duration));
                                        }

                                    }

                                }
                                // create new invoice against a company
                                else
                                {
                                    if (userRate != null)
                                    {
                                        // get user rate based on destination number against filter
                                        var getFilter = userRate.FirstOrDefault(a => a.Filter == "*" || Regex.IsMatch(cdr.DestinationNumber.ToString(), a.Filter));

                                        // formula to calculate call charge
                                        var callCharge = cdr.Duration * (getFilter.RateValue / 60);

                                        // create new invoice against a company
                                        var newInvoice = new InvoiceApiModel(Guid.NewGuid(), CompanyMapper.EntityToApi(userCalling.Company));
                                        // add new user call record to the new invoice
                                        newInvoice.UserCalls.Add(new UserChargeApiModel(Guid.NewGuid(), UserMapper.EntityToApi(userCalling), 1, callCharge, cdr.Duration));
                                        invoices.Add(newInvoice);
                                    }
                                }
                            }
                            break;
                        // person receiving the call gets charged
                        case "INBOUND":
                            // get user receiving the call
                            var userReceiving = _dbContext.Users
                                                .Include(c => c.Company)
                                                .ThenInclude(p => p.Plan)
                                                .Where(a => a.PhoneNumber == cdr.DestinationNumber.ToString())
                                                .AsNoTracking()
                                                .FirstOrDefault() ?? throw new ArgumentException("Failed to retrieve user");

                            if (userReceiving != null)
                            {
                                // get user rates based on rate type matching with direction
                                var userRate = _dbContext.Rates
                                                .Include(p => p.Plan)
                                                .Where(r => r.Plan.Id == userReceiving.Company.Plan.Id && r.RateType == RateType.InboundCall.ToString()).OrderBy(a => a.Priority)
                                                .AsNoTracking().ToList() ?? throw new ArgumentException("Failed to retrieve user rate");

                                // check if invoice against a company exist
                                if (invoices.Where(a => a.Company.Id == userReceiving.Company.Id).Any())
                                {
                                    // get existing company invoice
                                    var existingInvoice = invoices.Where(a => a.Company.Id == userReceiving.Company.Id).FirstOrDefault();
                                    // check if user has a record in the company
                                    if (existingInvoice.UserCalls.Where(u => u.User.Id == userReceiving.Id).Any())
                                    {
                                        if (userRate != null)
                                        {
                                            // get exising user call records
                                            var existingUserRecord = invoices.Where(a => a.Company.Id == userReceiving.Company.Id).Select(a => a.UserCalls.Where(a => a.User.Id == userReceiving.Id).FirstOrDefault()).FirstOrDefault();
                                            // get user rate based on destination number against filter
                                            var getFilter = userRate.FirstOrDefault(a => a.Filter == "*" || Regex.IsMatch(cdr.DestinationNumber.ToString(), a.Filter)) ?? throw new ArgumentException("Failed to retrieve rate to charge");

                                            // formula to calculate call charge
                                            var callCharge = cdr.Duration * (getFilter.RateValue / 60);
                                            var callDetail = new CallDetailApiModel(Guid.NewGuid(), cdr.ConnectDateTime, new TimeSpan(0, 0, cdr.Duration).ToString(), (getFilter.RateValue / 60), callCharge, callFrom: cdr.SourceNumber.ToString());

                                            // update existing user call record with new call charge
                                            existingUserRecord.UpdateUserCharge(1, callCharge, cdr.Duration, callDetail, DirectionType.INBOUND);
                                        }
                                    }
                                    else
                                    {
                                        if (userRate != null)
                                        {
                                            // get user rate based on destination number against filter
                                            var getFilter = userRate.FirstOrDefault(a => a.Filter == "*" || Regex.IsMatch(cdr.DestinationNumber.ToString(), a.Filter));

                                            // formula to calculate call charge
                                            var callCharge = cdr.Duration * (getFilter.RateValue / 60);

                                            // add new user call record against the company invoice
                                            existingInvoice.UserCalls.Add(new UserChargeApiModel(Guid.NewGuid(), UserMapper.EntityToApi(userReceiving), 1, callCharge, cdr.Duration));
                                        }

                                    }

                                }
                                // create new invoice against a company
                                else
                                {
                                    if (userRate != null)
                                    {
                                        // get user rate based on destination number against filter
                                        var getFilter = userRate.FirstOrDefault(a => a.Filter == "*" || Regex.IsMatch(cdr.DestinationNumber.ToString(), a.Filter));

                                        // formula to calculate call charge
                                        var callCharge = cdr.Duration * (getFilter.RateValue / 60);

                                        // create new invoice against a company
                                        var newInvoice = new InvoiceApiModel(Guid.NewGuid(), CompanyMapper.EntityToApi(userReceiving.Company));
                                        // add new user call record to the new invoice
                                        newInvoice.UserCalls.Add(new UserChargeApiModel(Guid.NewGuid(), UserMapper.EntityToApi(userReceiving), 1, callCharge, cdr.Duration));
                                        invoices.Add(newInvoice);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                return invoices;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to generate invoice", e);
            }
        }
        #endregion
    }
}
