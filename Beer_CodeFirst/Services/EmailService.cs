using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;



    using System.Net.Mail;

    namespace Beer_CodeFirst.Services
    {
        public class EmailService
        {
            public static void SendEmail(string toEmail, string subject, string body)
            {
                var fromEmail = "nguyenhoangson10092006@gmail.com";
                var password = "sjjb ttur bptk pyoa";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(fromEmail, password);

                MailMessage mail = new MailMessage(fromEmail, toEmail, subject, body);
                mail.IsBodyHtml = true;

                smtp.Send(mail);
            }
        }
    }
