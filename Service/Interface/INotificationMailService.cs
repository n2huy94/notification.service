using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface INotificationMailService
    {
        Task SendEmailAsync(string subject,string body,string toAddress);
    }
}
