using BillingProcess.Billing.Models;
using BillingProcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Models
{
    /// <summary>
    /// Invoicing model
    /// </summary>
    public class InvoiceApiModel
    {
        /// <summary>
        /// Unique invoice Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Company information
        /// </summary>
        public CompanyApiModel Company { get; set; }
        /// <summary>
        /// Call records made by user(s) to be charged
        /// </summary>
        public List<UserChargeApiModel> UserCalls {get; set;}

        #region Constructor
        internal InvoiceApiModel(Guid id, CompanyApiModel company)
        {
            Id = id;
            Company = company;
            UserCalls = new List<UserChargeApiModel>();
        }
        #endregion
    }
}
