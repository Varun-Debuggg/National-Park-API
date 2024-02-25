using NationalParkWebApp_3.Models;
using NationalParkWebApp_3.Repository.IRepository;

namespace NationalParkWebApp_3.Repository
{
    public class BookingRepository:Repository<Booking>,IBookingRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BookingRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
