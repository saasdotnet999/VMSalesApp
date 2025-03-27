namespace VMSalesApp.Services.Model
{
    public class Sale
    {
        public string Segment { get; set; }

        public string Country { get; set; }

        public string Product { get; set; }

        public string Discount { get; set; }

        public decimal UnitsSold { get; set; }

        public decimal ManufacturingPrice { get; set; }

        public decimal SalePrice { get; set; }

        public DateTime Date { get; set; }
    }
}
