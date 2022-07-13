using DominicanBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Domain.Entities
{
    public class Transfer : AuditableBaseEntity
    {

        public string IdentifyNumberFrom { get; set; }
        public double Amount { get; set; }
        public string IdentifyNumberTo { get; set; }
        public string UserId { get; set; }


    }
}
