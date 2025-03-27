using VMSalesApp.Services.Model;
using VMSalesApp.Repositories;
using VMSalesApp.Services;

namespace VMSalesApp.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _salesRepo;

        public SalesService(ISalesRepository salesRepository)
        {
                _salesRepo = salesRepository;
        }

        /// <summary>
        /// Get sales from the repository
        /// </summary>
        /// <returns></returns>
        public async Task<List<Sale>> GetSales()
        {
            var salesEntities = await _salesRepo.GetSalesAsync();
            
            var sales = salesEntities.Select(s => new Sale
            {
                Segment = s.Segment,
                Discount = s.Discount,
                UnitsSold = s.UnitsSold,
                ManufacturingPrice = s.ManufacturingPrice,
                SalePrice = s.SalePrice,
                Date = s.Date,
                Country = s.Country,
                Product = s.Product,             
            }).ToList();

            return sales;
        }
    }
}
