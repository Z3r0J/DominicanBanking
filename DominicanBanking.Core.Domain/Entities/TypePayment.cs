using DominicanBanking.Core.Domain.Common;
using System.Collections.Generic;

namespace DominicanBanking.Core.Domain.Entities
{
    public class TypePayment : AuditableBaseEntity
    {
        public string Name { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}