namespace VietNamTraverling.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public int TourId { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Createdate { get; set; }

        public Customer Customer { get; set; }
        public Tour Tour { get; set; }
    }


}
