using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Beer_CodeFirst.Services
{
    public class TokenService
    {
        public static string GenerateEmailToken(string email)
        {
            using (var sha256 = SHA256.Create())
            {
                var rawToken = $"{email}_{DateTime.UtcNow.Ticks}_{Guid.NewGuid()}";
                var bytes = Encoding.UTF8.GetBytes(rawToken);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash).Replace('/', '_').Replace('+', '-');
            }
        }

        public static string GenerateConfirmationLink(string token, string email)
        {
            var encodedEmail = HttpUtility.UrlEncode(email);

            // Tự động lấy URL hiện tại
            var url = HttpContext.Current.Request.Url;
            var baseUrl = $"{url.Scheme}://{url.Authority}";

            return $"{baseUrl}/Account/ConfirmEmail?token={token}&email={encodedEmail}";
        }
    }
}