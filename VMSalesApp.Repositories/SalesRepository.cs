using System.Globalization;
using VMSalesApp.Repositories.Entities;

namespace VMSalesApp.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly string _filePath;

        public SalesRepository(string? filePath = null)
        {
            _filePath = filePath ?? Path.Combine(Directory.GetCurrentDirectory(), "Data", "Data.csv");
        }

        /// <summary>
        /// Get sales from the repository
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<List<Sale>> GetSalesAsync()
        {
            List<Sale> sales = new List<Sale>();           

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"The file {_filePath} was not found.");
            }

            try
            {
                using (var reader = new StreamReader(_filePath))
                {
                    string headerLine = await reader.ReadLineAsync();

                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line?.Split(',');

                        if (values?.Length != 8)
                            continue;

                        DateTime date;
                        bool success = DateTime.TryParseExact(values[7], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                                               
                        sales.Add(new Sale
                        {
                            Segment = values[0].Trim(),
                            Country = values[1].Trim(),
                            Product = values[2].Trim(),
                            Discount = values[3].Trim(),
                            UnitsSold = string.IsNullOrWhiteSpace(values[4]) ? 0 : ParseDecimal(values[4]),
                            ManufacturingPrice = string.IsNullOrWhiteSpace(values[5]) ? 0 : ParseCurrency(values[5]),
                            SalePrice = string.IsNullOrWhiteSpace(values[6]) ? 0: ParseCurrency(values[6]),
                            Date = success ? date : DateTime.MinValue
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }

            return sales;
        }
        static decimal ParseCurrency(string currency)
        {
            string cleanValue = new string(currency.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());

            if (decimal.TryParse(cleanValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            throw new FormatException($"Invalid currency format: {currency}");
        }

        private decimal ParseDecimal(string value)
        {            
            var cleanValue = value.Replace(" ", "").Trim();
            return decimal.Parse(cleanValue, CultureInfo.InvariantCulture);
        }
    }    
}
