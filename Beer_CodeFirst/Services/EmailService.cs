using System;
using System.Net;
using System.Net.Mail;

namespace Beer_CodeFirst.Services
{
    public class EmailService
    {
        // Method mới để gửi email xác nhận
        public static void SendConfirmationEmail(string toEmail, string confirmationLink)
        {
            var fromEmail = "nguyenhoangson10092006@gmail.com";
            var password = "sjjb ttur bptk pyoa";

            string subject = "Xác nhận tài khoản Beer Shop";
            string body = $@"
                <h2>Chào mừng bạn đến với Beer Shop!</h2>
                <p>Vui lòng nhấp vào link bên dưới để xác nhận tài khoản của bạn:</p>
                <a href='{confirmationLink}' style='background-color: #28a745; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                    Xác nhận tài khoản
                </a>
                <p>Hoặc copy link này vào trình duyệt: <br/>{confirmationLink}</p>
                <p>Link có hiệu lực trong <strong>24 giờ</strong>.</p>
                <p>Nếu bạn không đăng ký tài khoản, vui lòng bỏ qua email này.</p>
                <hr/>
                <small>Beer Shop - Trải nghiệm bia tuyệt vời</small>
            ";

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.Timeout = 20000;

                using (MailMessage mail = new MailMessage(fromEmail, toEmail, subject, body))
                {
                    mail.IsBodyHtml = true;
                    smtp.Send(mail);
                }
            }
        }

        // Method cũ của bạn - giữ nguyên
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