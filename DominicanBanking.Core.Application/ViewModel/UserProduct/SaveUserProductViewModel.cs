using DominicanBanking.Core.Application.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.UserProduct
{
    public class SaveUserProductViewModel
    {
        public int Id { get; set; }
        public string IdentifyNumber { get; set; }
        public double Amount { get; set; }
        public double? Limit { get; set; }

        public string UserId { get; set; }
        public int ProductId { get; set; }

        public bool IsPrincipal { get; set; }
    }
}
