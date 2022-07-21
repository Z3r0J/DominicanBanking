using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.Transfer
{
    public class TransferViewModel
    {
        public int Id { get; set; }
        public string IdentifyNumberFrom { get; set; }
        public double Amount { get; set; }
        public string IdentifyNumberTo { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
    }
}
