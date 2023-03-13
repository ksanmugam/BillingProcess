using BillingProcess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Entities
{
    public class Rate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Plan Plan { get; set; }
        public string RateType { get; set; }
        public int Priority { get; set; }
        public string Filter { get; set; }
        public double RateValue { get; set; }
    }
}
