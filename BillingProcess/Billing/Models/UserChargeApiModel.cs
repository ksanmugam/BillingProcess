using BillingProcess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Billing.Models
{
    /// <summary>
    /// User charge model
    /// </summary>
    public class UserChargeApiModel
    {
        /// <summary>
        /// Unique user record id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Total calls made by the user
        /// </summary>
        public int TotalCalls { get; set; }
        /// <summary>
        /// Total cost to be charged to the user
        /// </summary>
        public double TotalCost { get; set; }
        /// <summary>
        /// Total duration of calls made/received by the user
        /// </summary>
        public double TotalDuration { get; set; }
        /// <summary>
        /// User profile
        /// </summary>
        public UserApiModel User { get; set; }
        /// <summary>
        /// Inbound calls are received by the user
        /// </summary>
        public List<CallDetailApiModel> InboundCalls { get; set; }
        /// <summary>
        /// Outbound calls made by the user
        /// </summary>
        public List<CallDetailApiModel> OutboundCalls { get; set; }
        /// <summary>
        /// User calls made between users in the same company
        /// </summary>
        public List<CallDetailApiModel> UserCalls { get; set; }

        #region Constructor
        internal UserChargeApiModel(Guid id, UserApiModel user, int totalCalls, double totalCost, double totalDuration)
        {
            Id = id;
            User = user;
            TotalCalls = totalCalls;
            TotalCost = totalCost;
            TotalDuration = totalDuration;
            InboundCalls = new List<CallDetailApiModel>();
            OutboundCalls = new List<CallDetailApiModel>();
            UserCalls = new List<CallDetailApiModel>();
        }

        // Add call detail to user call records
        public void AddCallDetail(CallDetailApiModel callDetail, DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.OUTBOUND:
                    OutboundCalls.Add(callDetail);
                    break;
                case DirectionType.INBOUND:
                    InboundCalls.Add(callDetail);
                    break;
                case DirectionType.USER:
                    UserCalls.Add(callDetail);
                    break;
                default:
                    break;
            }
        }

        // Update existing user record with new call record
        public void UpdateUserCharge(int totalCalls, double totalCost, double totalDuration, CallDetailApiModel callDetail, DirectionType direction)
        {
            TotalCalls += totalCalls;
            TotalCost += totalCost;
            TotalDuration += totalDuration;
            switch (direction)
            {
                case DirectionType.OUTBOUND:
                    OutboundCalls.Add(callDetail);
                    break;
                case DirectionType.INBOUND:
                    InboundCalls.Add(callDetail);
                    break;
                case DirectionType.USER:
                    UserCalls.Add(callDetail);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
