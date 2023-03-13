using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Billing.Models
{
    /// <summary>
    /// Call detail record the user made/received
    /// </summary>
    public class CallDetailApiModel
    {
        /// <summary>
        /// Unique call detail record id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Date the call was made/received
        /// </summary>
        public string DateTime { get; set; }
        /// <summary>
        /// User made a call to destination number
        /// </summary>
        public string CalledTo { get; set; }
        /// <summary>
        /// User receiving a call from source number
        /// </summary>
        public string CallFrom { get; set; }
        /// <summary>
        /// Duration of the call
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Rate to be charged for the call
        /// </summary>
        public double Gross { get; set; }
        /// <summary>
        /// Cost of the call
        /// </summary>
        public double Amount { get; set; }

        #region Constructor
        public CallDetailApiModel(Guid id, string dateTime, string duration, double gross, double amount, string calledTo = null, string callFrom = null)
        {
            Id = id;
            DateTime = dateTime;
            CalledTo = calledTo;
            CallFrom = callFrom;
            Duration = duration;
            Gross = gross;
            Amount = amount;
        }
        #endregion
    }
}
