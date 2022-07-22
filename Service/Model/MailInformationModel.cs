using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    internal class MailInfomationModel
    {
        public string MailFrom { get; set; }
        public string Password { get; set; }
        public string SmtpAddress { get; set; }
        public int PortNumber { get; set; }
    }
}