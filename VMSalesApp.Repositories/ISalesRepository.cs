using VMSalesApp.Repositories.Entities;

namespace VMSalesApp.Repositories
{
    public interface ISalesRepository
    {
        /// <summary>
        /// Get sales from the repository
        /// </summary>
        /// <returns></returns>
        Task<List<Sale>> GetSalesAsync();
    }
}
