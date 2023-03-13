using BillingProcess.Billing.Models;
using BillingProcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Billing.Mappers
{
    internal static class CompanyMapper
    {
        internal static CompanyApiModel EntityToApi(Company entity)
        {
            if (entity is null)
                return null;
            return new CompanyApiModel
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
