using NationalParkWebApp_3.Models;
using NationalParkWebApp_3.Repository.IRepository;

namespace NationalParkWebApp_3.Repository
{
    public class TrailRepository:Repository<Trail>,ITrailRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TrailRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
