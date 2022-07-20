using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.Transfer
{
    public class SaveTransferViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Account number is required")]
        public string IdentifyNumberFrom { get; set; }
        [Required(ErrorMessage = "The amount is required")]
        [Range(1.00, double.MaxValue, ErrorMessage = "The Amount have been greater than 0")]
        public double Amount { get; set; }
        [Required(ErrorMessage = "The Account Number to transfer is required")]
        public string IdentifyNumberTo { get; set; }
        public string UserId { get; set; }
    }
}
