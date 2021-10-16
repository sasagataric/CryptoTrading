using CryptoTrading.API.Controllers;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CryptoTrading.Tests.Controllers
{

    public class CoinsControllerTests
    {
        private Mock<ICoinService> _mockCoinService;
        private CoinsController _coinsController;
        public CoinsControllerTests()
        {
            _mockCoinService = new Mock<ICoinService>();
            _coinsController = new CoinsController(_mockCoinService.Object);
        }

        [Fact]
        public async Task GetById_WhenGetByIdIsSuccessful_ReturnCoinData()
        {
            // Arrange
            var coinModel = new CoinDomainModel
            {
                Id = "bitcoin",
                Image = "img",
                Name = "Bitcoin",
                Symbol = "BTC"
            };
            var retunModel = new GenericDomainModel<CoinDomainModel>
            {
                IsSuccessful = true,
                Data = coinModel
            };
            _mockCoinService.Setup(s => s.GetByIdAsync(coinModel.Id)).ReturnsAsync(retunModel);

            // Act
            var result = await _coinsController.GetById(coinModel.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_WhenGetByIdIsNotSuccessful_ReturnsBadRequestResult()
        {
            // Arrange
            var retunModel = new GenericDomainModel<CoinDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.COIN_ID_NULL
            };
            var coinId= It.IsAny<string>();
            _mockCoinService.Setup(s => s.GetByIdAsync(coinId)).ReturnsAsync(retunModel);

            // Act
            var result = await _coinsController.GetById(coinId);
            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResult = Assert.IsType<ErrorResponseModel>(badRequestObjectResult.Value);
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorResult.ErrorMessage, Messages.COIN_ID_NULL);
        }
    }
}
