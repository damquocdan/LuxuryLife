namespace VietNamTraverling.Models
{
    public class Favourite
    {
        public int FavouriteId { get; set; }
        public int CustomerId { get; set; }
        public int TourId { get; set; }
        public double Sumprice { get; set; }

        public Customer Customer { get; set; }
        public Tour Tour { get; set; }
    }


}
