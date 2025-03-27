using VMSalesApp.Services.Model;

namespace VMSalesApp.Services
{
    public interface ISalesService
    {
        Task<List<Sale>> GetSales();
    }
}
