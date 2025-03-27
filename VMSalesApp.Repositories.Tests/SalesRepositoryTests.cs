namespace VMSalesApp.Repositories.Tests
{
    using Xunit;

    public class SalesRepositoryTests
    {
        [Fact]
        public async Task GetSalesAsync_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), "NonExistentFile.csv");
            var repository = new SalesRepository(tempFilePath);

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => repository.GetSalesAsync());
        }

        [Fact]
        public async Task GetSalesAsync_ShouldReturnValidSales_WhenFileContainsValidData()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), "ValidData.csv");

            string csvContent = "Segment,Country,Product,Discount,UnitsSold,ManufacturingPrice,SalePrice,Date\n" +
                                "Government,USA,ProductA,5%,100,10.50,20.75,12/01/2023\n" +
                                "Enterprise,Canada,ProductB,10%,200,15.00,30.00,15/02/2023";

            File.WriteAllText(tempFilePath, csvContent);

            var repository = new SalesRepository(tempFilePath);

            var sales = await repository.GetSalesAsync();

            File.Delete(tempFilePath);

            Assert.NotNull(sales);
            Assert.Equal(2, sales.Count);
            Assert.Equal("Government", sales[0].Segment);
            Assert.Equal("USA", sales[0].Country);
            Assert.Equal(100, sales[0].UnitsSold);
            Assert.Equal(10.50m, sales[0].ManufacturingPrice);
            Assert.Equal(20.75m, sales[0].SalePrice);
            Assert.Equal(new DateTime(2023, 1, 12), sales[0].Date);
        }

        [Fact]
        public async Task GetSalesAsync_ShouldSkipMalformedRows()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), "MalformedData.csv");

            string csvContent = "Segment,Country,Product,Discount,UnitsSold,ManufacturingPrice,SalePrice,Date\n" +
                                "Government,USA,ProductA,5%,100,10.50,20.75,12/01/2023\n" +
                                "Enterprise,Canada,ProductB,10%\n" +
                                "Enterprise,UK,ProductC,15%,150,12.00,25.00,20/03/2023";

            File.WriteAllText(tempFilePath, csvContent);

            var repository = new SalesRepository(tempFilePath);
            
            var sales = await repository.GetSalesAsync();
          
            File.Delete(tempFilePath);
            
            Assert.NotNull(sales);
            Assert.Equal(2, sales.Count);
        }

        [Fact]
        public async Task GetSalesAsync_ShouldAssign0_WhenValuesAreEmpty()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), "MalformedData.csv");

            string csvContent = "Segment,Country,Product,Discount,UnitsSold,ManufacturingPrice,SalePrice,Date\n" +
                                "Government,USA,ProductA,5%, , , ,12/01/2023\n" +                                
                                "Enterprise,UK,ProductC,15%,150,12.00,25.00,20/03/2023";

            File.WriteAllText(tempFilePath, csvContent);

            var repository = new SalesRepository(tempFilePath);

            var sales = await repository.GetSalesAsync();

            File.Delete(tempFilePath);

            Assert.NotNull(sales);
            Assert.Equal(2, sales.Count);
            Assert.Equal(0, sales[0].UnitsSold);
            Assert.Equal(0, sales[0].ManufacturingPrice);
            Assert.Equal(0, sales[0].SalePrice);
        }

        [Fact]
        public async Task GetSalesAsync_ShouldRemoveSpacesInDecimalValue()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), "MalformedData.csv");

            string csvContent = "Segment,Country,Product,Discount,UnitsSold,ManufacturingPrice,SalePrice,Date\n" +
                                "Government,USA,ProductA,5%,45 . 00 ,10.50,20.75,12/01/2023\n" +
                                "Enterprise,UK,ProductC,15%,150,12.00,25.00,20/03/2023";

            File.WriteAllText(tempFilePath, csvContent);

            var repository = new SalesRepository(tempFilePath);

            var sales = await repository.GetSalesAsync();

            File.Delete(tempFilePath);

            Assert.NotNull(sales);
            Assert.Equal(2, sales.Count);
            Assert.Equal(Convert.ToDecimal(45.00), sales[0].UnitsSold);
        }
    }
}