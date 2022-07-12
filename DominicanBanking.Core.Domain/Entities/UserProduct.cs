using DominicanBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Domain.Entities
{
    public class UserProduct : AuditableBaseEntity
    {
        public string IdentifyNumber { get; set; }
        public double Amount { get; set; }
        public double? Limit { get; set; }

        public string UserId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public bool IsPrincipal { get; set; }
    }
}
