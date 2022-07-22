using Microsoft.Extensions.Configuration;
using Service.Interface;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class NotificationMailService : INotificationMailService
    {
        private readonly IConfiguration _configuration;

        private MailInfomationModel _mailInfomationModel;

        public NotificationMailService(IConfiguration configuration)
        {
            this._configuration = configuration;
            InitSender();
        }

        private void InitSender()
        {
            _mailInfomationModel = new MailInfomationModel();
            _mailInfomationModel.SmtpAddress = _configuration["MailInfo:SmtpAddress"];
            _mailInfomationModel.PortNumber = int.Parse(_configuration["MailInfo:PortNumber"]);
            _mailInfomationModel.MailFrom = _configuration["MailInfo:MailFrom"];
            _mailInfomationModel.Password = _configuration["MailInfo:Password"];
        }

        public async Task SendEmailAsync(string subject, string body, string toAddress)
        {
            await Task.Factory.StartNew(() =>
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_mailInfomationModel.MailFrom);
                    mail.To.Add(toAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient(_mailInfomationModel.SmtpAddress, _mailInfomationModel.PortNumber))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(_mailInfomationModel.MailFrom, _mailInfomationModel.Password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            });
        }
    }
}
