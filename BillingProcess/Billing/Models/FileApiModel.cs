using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Billing.Models
{
    public class FileApiModel
    {
        public IFormFile File
        {
            get;
            set;
        }
    }
}
