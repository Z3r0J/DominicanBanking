using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.CashAdvance
{
    public class SaveCashAdvanceViewModel
    {
        public int Id { get; set; }
        public string CreditCardNumberFrom { get; set; }
        public double Amount { get; set; }
        public string IdentifyNumberTo { get; set; }
        public string UserId { get; set; }
    }
}
