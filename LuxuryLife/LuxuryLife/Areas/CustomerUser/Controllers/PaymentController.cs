using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    [Area("CustomerUser")]
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;

        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("create")]
        public IActionResult CreatePayment(decimal amount)
        {
            string vnp_TmnCode = _configuration["VNPay:TmnCode"];
            string vnp_HashSecret = _configuration["VNPay:HashSecret"];
            string vnp_Url = _configuration["VNPay:Url"];
            string vnp_ReturnUrl = _configuration["VNPay:ReturnUrl"];

            // Tạo thông tin thanh toán
            var vnPayParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", ((int)(amount * 100)).ToString() }, // Số tiền (x100)
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1" },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", "Payment for order #" + DateTime.Now.Ticks },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", vnp_ReturnUrl },
                { "vnp_TxnRef", DateTime.Now.Ticks.ToString() }
            };

            // Tạo URL thanh toán
            string queryString = string.Join("&", vnPayParams.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));
            string rawData = queryString + "&" + vnp_HashSecret;
            string vnp_SecureHash = GenerateHmacSHA512(rawData, vnp_HashSecret);

            string paymentUrl = $"{vnp_Url}?{queryString}&vnp_SecureHash={vnp_SecureHash}";
            return Redirect(paymentUrl);
        }

        [HttpGet("return")]
        public IActionResult Return()
        {
            var queryParams = HttpContext.Request.Query;
            var vnp_SecureHash = queryParams["vnp_SecureHash"];
            string hashSecret = _configuration["VNPay:HashSecret"];

            // Xác minh chữ ký
            var sortedParams = queryParams.Where(x => x.Key != "vnp_SecureHash")
                                          .OrderBy(x => x.Key)
                                          .ToDictionary(x => x.Key, x => x.Value.ToString());

            string rawData = string.Join("&", sortedParams.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));
            string calculatedHash = GenerateHmacSHA512(rawData, hashSecret);

            if (calculatedHash == vnp_SecureHash)
            {
                string transactionStatus = queryParams["vnp_TransactionStatus"];
                if (transactionStatus == "00")
                {
                    return Content("Payment successful!");
                }
                else
                {
                    return Content($"Payment failed! Status: {transactionStatus}");
                }
            }

            return Content("Invalid payment signature!");
        }

        private string GenerateHmacSHA512(string data, string key)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }
        }
    }
}
