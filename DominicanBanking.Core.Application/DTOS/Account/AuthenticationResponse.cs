﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.DTOS.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Documents { get; set; }
        public string Phone { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}
