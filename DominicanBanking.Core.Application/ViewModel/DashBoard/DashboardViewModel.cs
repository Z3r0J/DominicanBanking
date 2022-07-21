using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.DashBoard
{
    public class DashboardViewModel
    {
        public int Transaction { get; set; }
        public int TransactionToday { get; set; }
        public int PaymentToday { get; set; }
        public int PaymentTotal { get; set; }
        public int ClientActive { get; set; }
        public int ClientInactive { get; set; }
        public int ProductQuantityCredit { get; set; }
        public int ProductQuantitySavings { get; set; }
        public int ProductQuantityLoan { get; set; }
        public int ProductQuantityTotal { get; set; }

    }
}
