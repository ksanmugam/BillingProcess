using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingProcess.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Plan Plan { get; set; }

        private Company() { }

        internal Company(Guid id, string name, Guid planId)
        {
            Id = id;
            Name = name;
            Plan.Id = planId;
        }
    }
}
