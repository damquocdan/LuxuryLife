using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LuxuryLife.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;

        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("payment/create")]
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
            string rawData = string.Join("&", vnPayParams.Select(x => $"{x.Key}={x.Value}")); // Không sử dụng UrlEncode
            string vnp_SecureHash = GenerateHmacSHA512(rawData, vnp_HashSecret);

            // Tạo chuỗi query string (URL cần mã hóa ở đây)
            string queryString = string.Join("&", vnPayParams.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));
            string paymentUrl = $"{vnp_Url}?{queryString}&vnp_SecureHash={vnp_SecureHash}";

            return Redirect(paymentUrl);

        }

        [HttpGet("payment/return")]
        public IActionResult Return()
        {
            var queryParams = HttpContext.Request.Query;
            var vnp_SecureHash = queryParams["vnp_SecureHash"];
            string hashSecret = _configuration["VNPay:HashSecret"];

            // Lọc bỏ tham số vnp_SecureHash, sắp xếp các tham số còn lại theo tên khóa
            var sortedParams = queryParams.Where(x => x.Key != "vnp_SecureHash")
                                          .OrderBy(x => x.Key)
                                          .ToDictionary(x => x.Key, x => x.Value.ToString());

            // Tạo chuỗi rawData từ các tham số đã sắp xếp
            string rawData = string.Join("&", sortedParams.Select(x => $"{x.Key}={x.Value}"));

            // Tính toán lại mã băm từ rawData và HashSecret
            string calculatedHash = GenerateHmacSHA512(rawData, hashSecret);

            // Kiểm tra nếu mã băm tính toán khớp với mã băm trả về
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
