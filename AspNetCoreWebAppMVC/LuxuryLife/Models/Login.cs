using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email không để trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không để trống")]
        public string Password { get; set; }
        public bool Remember { get; set; }
        public int CustomerId { get; internal set; }
    }
}
