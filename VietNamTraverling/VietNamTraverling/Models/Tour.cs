namespace VietNamTraverling.Models
{
    public class Tour
    {
        public int TourId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int ServiceId { get; set; }
        public int HomestayId { get; set; }
        public double PricePerson { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public DateTime Createdate { get; set; }
        public int ProviderId { get; set; }

        public Provider Provider { get; set; }
        public Service Service { get; set; }
        public Homestay Homestay { get; set; }
    }

}
