using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Enums
{
    public enum DirectionType
    {
        [Display(Name = "OUTBOUND")]
        OUTBOUND,
        [Display(Name = "INBOUND")]
        INBOUND,
        [Display(Name = "USER")]
        USER,
    }
}
