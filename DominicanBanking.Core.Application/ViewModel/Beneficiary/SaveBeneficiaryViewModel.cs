using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.Beneficiary
{
    public class SaveBeneficiaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "The Number Account is required")]
        public string IdentifyNumber { get; set; }

        public string UserId { get; set; }
    }
}
