namespace VMSalesApp.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using VMSalesApp.Services;
    using VMSalesApp.Repositories;
    using Xunit;
    using Sale = Repositories.Entities.Sale;

    public class SalesServiceTests
    {
        private readonly Mock<ISalesRepository> _salesRepositoryMock;
        private readonly SalesService _salesService;

        public SalesServiceTests()
        {
            _salesRepositoryMock = new Mock<ISalesRepository>();
            _salesService = new SalesService(_salesRepositoryMock.Object);
        }

        [Fact]
        public async Task GetSales_ShouldReturnMappedSales_WhenRepositoryReturnsData()
        {
            // Arrange
            var salesEntities = new List<Sale>
        {
            new Sale
            {
                Segment = "Government",
                Country = "USA",
                Product = "ProductA",
                Discount = "5%",
                UnitsSold = 100,
                ManufacturingPrice = 10.50m,
                SalePrice = 20.75m,
                Date = new DateTime(2023, 1, 12)
            },
            new Sale
            {
                Segment = "Enterprise",
                Country = "Canada",
                Product = "ProductB",
                Discount = "10%",
                UnitsSold = 200,
                ManufacturingPrice = 15.00m,
                SalePrice = 30.00m,
                Date = new DateTime(2023, 2, 15)
            }
        };

            _salesRepositoryMock
                .Setup(repo => repo.GetSalesAsync())
                .ReturnsAsync(salesEntities);

            // Act
            var result = await _salesService.GetSales();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Government", result[0].Segment);
            Assert.Equal("USA", result[0].Country);
            Assert.Equal(100, result[0].UnitsSold);
            Assert.Equal(10.50m, result[0].ManufacturingPrice);
            Assert.Equal(20.75m, result[0].SalePrice);
            Assert.Equal(new DateTime(2023, 1, 12), result[0].Date);
        }

        [Fact]
        public async Task GetSales_ShouldReturnEmptyList_WhenRepositoryReturnsEmpty()
        {
            // Arrange
            _salesRepositoryMock
                .Setup(repo => repo.GetSalesAsync())
                .ReturnsAsync(new List<Sale>());

            // Act
            var result = await _salesService.GetSales();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSales_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            _salesRepositoryMock
                .Setup(repo => repo.GetSalesAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _salesService.GetSales());
            Assert.Equal("Database error", exception.Message);
        }
    }

}