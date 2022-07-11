using DominicanBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Domain.Entities
{
    public class Product:AuditableBaseEntity
    {
        public string Name { get; set; }

        public ICollection<UserProduct> UserProducts{ get; set; }
    }
}
