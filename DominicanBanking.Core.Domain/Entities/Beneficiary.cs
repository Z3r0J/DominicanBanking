using DominicanBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Domain.Entities
{
    public class Beneficiary : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentifyNumber { get; set; }

        public string UserId { get; set; }
    }
}
