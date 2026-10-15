using System;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;

namespace Beer_CodeFirst.Others
{
    public class MoMoSecurity
    {
        public static string signSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                return hex.Replace("-", "").ToLower();
            }
        }
    }

    public class PaymentRequest
    {
        public static string sendPaymentRequest(string endpoint, string postJsonString)
        {
            try
            {
                // 1. Cấu hình Request chuẩn chỉnh
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.Method = "POST";
                request.ContentType = "application/json";

                // QUAN TRỌNG: Tính toán độ dài dựa trên Byte UTF8 để tránh lỗi lệch ký tự
                byte[] data = Encoding.UTF8.GetBytes(postJsonString);
                request.ContentLength = data.Length;

                // Thêm các thuộc tính giúp kết nối ổn định hơn
                request.KeepAlive = true;
                request.Timeout = 30000; // Đợi tối đa 30 giây

                // 2. Ghi dữ liệu vào Stream
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }

                // 3. Nhận phản hồi
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException webEx)
            {
                // Nếu bị lỗi mạng, đọc chi tiết lỗi từ Server gửi về (rất quan trọng để debug)
                if (webEx.Response != null)
                {
                    using (var errorReader = new StreamReader(webEx.Response.GetResponseStream()))
                    {
                        return errorReader.ReadToEnd();
                    }
                }
                return webEx.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}