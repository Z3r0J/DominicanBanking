using DominicanBanking.Core.Application.DTOS.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Interfaces.Services
{
    public interface IEmailServices
    {
        Task SendAsync(EmailRequest emailRequest);
    }
}
