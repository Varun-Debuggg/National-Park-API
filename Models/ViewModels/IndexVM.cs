using NationalParkWebApp_3.Models;

namespace NationalParkWebApp_3.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParksList { get; set; }
        public IEnumerable<Trail> TrailList { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
