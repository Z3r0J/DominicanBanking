using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.Payment
{
    public class PaymentViewModel
    {
        public string IdentifyNumberFrom { get; set; }
        public double Amount { get; set; }
        public string IdentifyNumberTo { get; set; }
        public string UserId { get; set; }
        public int TypeId { get; set; }
        public string TypePayment { get; set; }
    }
}
