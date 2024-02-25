namespace NationalParkWebApp_3.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T objToCreate);
        Task<bool> UpdateAsync(string url, T objToUpdate);
        Task<bool> DeleteAsync(string url, int id);
        Task<T> GetAsync(string url, int bId, int nId);
        Task<T> GetLastestAsync(string url, string bEmail);

    }
}
