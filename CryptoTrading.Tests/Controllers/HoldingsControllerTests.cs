using CryptoTrading.API.Controllers;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CryptoTrading.Tests.Controllers
{
    public class HoldingsControllerTests
    {
        private Mock<IHoldingService> _mockHoldingService;
        private HoldingsController _holdingsController;
        public HoldingsControllerTests()
        {
            _mockHoldingService = new Mock<IHoldingService>();
            _holdingsController = new HoldingsController(_mockHoldingService.Object);
        }

       
        [Fact]
        public async Task GetHolding_WhenIsSuccessful_ReturnHoldingData()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var coinId = "bitcoin";

            var holdinModel = new HoldingDomainModel
            {
                WalletId = walletId,
                Amount=1,
                AverageBuyingPrice=1,
                AverageSellingPrice=1,
                DateOfFirstPurchase=new DateTime(),
                Coin=new CoinDomainModel
                {
                    Id=coinId
                }
            };
            var retunModel = new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                Data = holdinModel
            };
            _mockHoldingService.Setup(s => s.GetHolding(walletId, coinId)).ReturnsAsync(retunModel);

            // Act
            var result = await _holdingsController.GetHolding(walletId, coinId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsType<HoldingDomainModel>(okObjectResult.Value);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(walletId, returnData.WalletId);
            Assert.Equal(coinId, returnData.Coin.Id);
        }

        [Fact]
        public async Task GetHolding_WhenIsNotSuccessful_ReturnsBadRequestResult()
        {
            // Arrange
            var walletId = Guid.NewGuid();
            var coinId = "bitcoin";
            var retunModel = new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.PURCHASED_CANT_BE_FOUND
            };
            _mockHoldingService.Setup(s => s.GetHolding(walletId, coinId)).ReturnsAsync(retunModel);

            // Act
            var result = await _holdingsController.GetHolding(walletId, coinId);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorModel = Assert.IsType<ErrorResponseModel>(badRequestObjectResult.Value);
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(Messages.PURCHASED_CANT_BE_FOUND, errorModel.ErrorMessage);
        }

        [Fact]
        public async Task GetHoldingsByUserId_WhenIsSuccessful_ReturnUserHoldings()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var walletId = Guid.NewGuid();
            var coinId = "bitcoin";
            var holdinModel = new HoldingDomainModel
            {
                WalletId = walletId,
                Amount = 1,
                AverageBuyingPrice = 1,
                AverageSellingPrice = 1,
                DateOfFirstPurchase = new DateTime(),
                Coin = new CoinDomainModel
                {
                    Id = coinId
                }
            };
            var retunModel = new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                DataList = new List<HoldingDomainModel>{holdinModel, holdinModel}
            };
            _mockHoldingService.Setup(s => s.GetHoldingsByUserId(userId)).ReturnsAsync(retunModel);

            // Act
            var result = await _holdingsController.GetHoldingsByUserId(userId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnData = Assert.IsType<List<HoldingDomainModel>>(okObjectResult.Value);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(2,returnData.Count);
        }

        [Fact]
        public async Task GetHoldingsByUserId_WhenIsNotSuccessful_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var retunModel = new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.USERS_NOT_FOUND
            };
            _mockHoldingService.Setup(s => s.GetHoldingsByUserId(userId)).ReturnsAsync(retunModel);

            // Act
            var result = await _holdingsController.GetHoldingsByUserId(userId);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorModel = Assert.IsType<ErrorResponseModel>(badRequestObjectResult.Value);
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(Messages.USERS_NOT_FOUND, errorModel.ErrorMessage);
        }

        [Fact]
        public async Task BuyCoin_ModelStateIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var expectedErrorMessage = "The Amount field is required.";
            var buyModel = new HoldingModel
            { 
                CoinId = "bitcoin",
                WalletId = Guid.NewGuid()
            };
            _holdingsController.ModelState.AddModelError("Amount", expectedErrorMessage);

            // Act
            var result = await _holdingsController.BuyCoin(buyModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = ((SerializableError)badRequestObjectResult.Value).GetValueOrDefault("Amount");
            var message = (string[])errorResponse;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(message[0], expectedErrorMessage);
        }

        [Fact]
        public async Task BuyCoin_PurchaseIsSuccessful_ReturnCreatedAtActionResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var walletId = Guid.NewGuid();
            var coinId = "bitcoin";
            var holdinModel = new HoldingDomainModel
            {
                WalletId = walletId,
                Amount = 1,
                AverageBuyingPrice = 1,
                AverageSellingPrice = 1,
                DateOfFirstPurchase = new DateTime(),
                Coin = new CoinDomainModel
                {
                    Id = coinId
                }
            };
            var buyModel = new HoldingModel
            {
                Amount=1,
                CoinId = "bitcoin",
                WalletId = walletId
            };
            var retunModel = new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                Data = holdinModel
            };
            _mockHoldingService.Setup(s => s.BuyCoinAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(retunModel);

            // Act
            var result = await _holdingsController.BuyCoin(buyModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnData = Assert.IsType<HoldingDomainModel>(createdAtActionResult.Value);
            Assert.NotNull(result);
            Assert.Equal(walletId, returnData.WalletId);
            Assert.Equal(coinId, returnData.Coin.Id);
        }

        [Fact]
        public async Task BuyCoin_PurchaseIsNotSuccessful_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var walletId = Guid.NewGuid();
            var holdinModel = new HoldingDomainModel();
            var buyModel = new HoldingModel
            {
                Amount = 1,
                CoinId = "bitcoin",
                WalletId = walletId
            };
            var retunModel = new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.WALLET_ID_NULL
            };
            _mockHoldingService.Setup(s => s.BuyCoinAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(retunModel);

            // Act
            var result = await _holdingsController.BuyCoin(buyModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnData = Assert.IsType<ErrorResponseModel>(badRequestObjectResult.Value);
            Assert.NotNull(result);
            Assert.Equal(Messages.WALLET_ID_NULL, returnData.ErrorMessage);
        }

        [Fact]
        public async Task BuyCoin_DataBaseError_ReturnCreatedAtActionResult()
        {
            // Arrange
            var exceptionMessage = "Database error";
            _mockHoldingService.Setup(s => s.BuyCoinAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>())).ThrowsAsync(new DbUpdateException(exceptionMessage));

            // Act
            var result =await _holdingsController.BuyCoin(new HoldingModel());

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnData = Assert.IsType<ErrorResponseModel>(badRequestObjectResult.Value);
            Assert.NotNull(result);
            Assert.Equal(exceptionMessage, returnData.ErrorMessage);
        }

        [Fact]
        public async Task SellCoin_ModelStateIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var expectedErrorMessage = "The Amount field is required.";
            var buyModel = new HoldingModel
            {
                CoinId = "bitcoin",
                WalletId = Guid.NewGuid()
            };
            _holdingsController.ModelState.AddModelError("Amount", expectedErrorMessage);

            // Act
            var result = await _holdingsController.SellCoin(buyModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = ((SerializableError)badRequestObjectResult.Value).GetValueOrDefault("Amount");
            var message = (string[])errorResponse;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(message[0], expectedErrorMessage);
        }

        [Fact]
        public async Task SellCoin_PurchaseIsSuccessful_ReturnCreatedAtActionResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var walletId = Guid.NewGuid();
            var coinId = "bitcoin";
            var holdinModel = new HoldingDomainModel
            {
                WalletId = walletId,
                Amount = 1,
                AverageBuyingPrice = 1,
                AverageSellingPrice = 1,
                DateOfFirstPurchase = new DateTime(),
                Coin = new CoinDomainModel
                {
                    Id = coinId
                }
            };
            var sellModel = new HoldingModel
            {
                Amount = 1,
                CoinId = "bitcoin",
                WalletId = walletId
            };
            var retunModel = new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                Data = holdinModel
            };
            _mockHoldingService.Setup(s => s.SellCoinAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(retunModel);

            // Act
            var result = await _holdingsController.SellCoin(sellModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnData = Assert.IsType<HoldingDomainModel>(createdAtActionResult.Value);
            Assert.NotNull(result);
            Assert.Equal(walletId, returnData.WalletId);
            Assert.Equal(coinId, returnData.Coin.Id);
        }

        [Fact]
        public async Task SellCoin_SaleIsNotSuccessful_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var walletId = Guid.NewGuid();
            var holdinModel = new HoldingDomainModel();
            var sellModel = new HoldingModel
            {
                Amount = 1,
                CoinId = "bitcoin",
                WalletId = walletId
            };
            var retunModel = new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = false,
                ErrorMessage = Messages.WALLET_ID_NULL
            };
            _mockHoldingService.Setup(s => s.SellCoinAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(retunModel);

            // Act
            var result = await _holdingsController.SellCoin(sellModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnData = Assert.IsType<ErrorResponseModel>(badRequestObjectResult.Value);
            Assert.NotNull(result);
            Assert.Equal(Messages.WALLET_ID_NULL, returnData.ErrorMessage);
        }

        [Fact]
        public async Task SellCoin_DataBaseError_ReturnCreatedAtActionResult()
        {
            // Arrange
            var exceptionMessage = "Database error";
            _mockHoldingService.Setup(s => s.SellCoinAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>())).ThrowsAsync(new DbUpdateException(exceptionMessage));

            // Act
            var result = await _holdingsController.SellCoin(new HoldingModel());

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnData = Assert.IsType<ErrorResponseModel>(badRequestObjectResult.Value);
            Assert.NotNull(result);
            Assert.Equal(exceptionMessage, returnData.ErrorMessage);
        }
    }
}
