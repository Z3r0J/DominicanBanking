using DominicanBanking.Core.Application.ViewModel.UserProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserProductViewModel> userProducts { get; set; } 
    }
}
