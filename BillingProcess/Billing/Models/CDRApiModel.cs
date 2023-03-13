using BillingProcess.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Models
{
    public class CDRApiModel
    {
        public long CarrierReference { get; set; }
        public string ConnectDateTime { get; set; }
        public int Duration { get; set; }
        public long SourceNumber { get; set; }
        public long DestinationNumber { get; set; }
        public string Direction { get; set; }

        [JsonConstructor]
        public CDRApiModel(long carrierReference, string connectDateTime, int duration, long sourceNumber, long destinationNumber, string direction)
        {
            CarrierReference = carrierReference;
            ConnectDateTime = connectDateTime;
            Duration = duration;
            SourceNumber = sourceNumber;
            DestinationNumber = destinationNumber;
            Direction = direction;
        }
    }
}
