using BillingProcess.Billing.Models;
using BillingProcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingProcess.Billing.Mappers
{
    internal static class UserMapper
    {
        internal static UserApiModel EntityToApi(User entity)
        {
            if (entity is null)
                return null;
            return new UserApiModel
            {
                Id = entity.Id,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber
            };
        }
    }
}
