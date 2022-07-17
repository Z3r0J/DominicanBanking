﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.DTOS.Account
{
    public class AccountResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Documents { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
    }
}