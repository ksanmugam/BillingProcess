using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Enums
{
    public enum RateType
    {
        [Display(Name = "User")]
        User = 0,
        [Display(Name = "InboundCall")]
        InboundCall = 1,
        [Display(Name = "OutboundCall")]
        OutboundCall = 2
    }
}
