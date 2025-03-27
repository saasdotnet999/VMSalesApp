using Microsoft.AspNetCore.Mvc;
using VMSalesApp.Services;

namespace VMSalesApp.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : Controller
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSalesData()
        {
            var salesData = await _salesService.GetSales();
            return Ok(salesData);
        }
    }
}
