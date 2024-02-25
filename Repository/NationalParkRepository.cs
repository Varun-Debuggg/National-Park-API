using NationalParkWebApp_3.Models;
using NationalParkWebApp_3.Repository.IRepository;

namespace NationalParkWebApp_3.Repository
{
    public class NationalParkRepository:Repository<NationalPark>,INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NationalParkRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
