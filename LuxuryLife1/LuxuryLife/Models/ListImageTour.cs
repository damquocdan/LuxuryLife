namespace LuxuryLife.Models
{
    public class ListImageTour
    {
        public int ListImageTourId { get; set; }
        public int TourId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDescription { get; set; }
        public DateTime Createdate { get; set; }

        public Tour Tour { get; set; }
    }
}
