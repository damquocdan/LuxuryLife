namespace VietNamTraverling.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public DateTime Createdate { get; set; }

        // Mối quan hệ: Một Provider có nhiều Tour và Homestay
        public ICollection<Tour> Tours { get; set; }
        public ICollection<Homestay> Homestays { get; set; }
    }

}
