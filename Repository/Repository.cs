using NationalParkWebApp_3.Repository.IRepository;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace NationalParkWebApp_3.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // This is API Calling Interface
        private readonly IHttpClientFactory _httpClientFactory;
        public Repository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateAsync(string url, T objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage Respone = await client.SendAsync(request);
            if (Respone.StatusCode == System.Net.HttpStatusCode.Created)
                return true;
            return false;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + "/" + id.ToString());
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage Response = await client.SendAsync(request);
            if (Response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await Response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + "/" + id.ToString());
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage Response = await client.SendAsync(request);
            if (Response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await Response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int bId, int nId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + "/" + bId.ToString() + "/" + nId.ToString());
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode== System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return null;
        }

        public async Task<T> GetLastestAsync(string url, string bEmail)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + "/" + bEmail);
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return null;
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            if (objToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");
            }
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage Response = await client.SendAsync(request);
            if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            return false;
        }


    }
}
