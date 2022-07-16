using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.ViewModel.User
{
    public class SaveUserViewModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Documents { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
        public int UserType { get; set; }
        public double? Amount { get; set; }
        public string Error { get; set; }
        public bool HasError { get; set; }

    }
}
