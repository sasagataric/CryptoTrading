using CryptoTrading.API;
using CryptoTrading.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoTrading.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private CryptoCoinsController _cryptoCoinsControler;

        private Mock<CoinGecko.Interfaces.ICoinGeckoClient> _mockCoinGeckoClient;

        [TestInitialize]
        public void TestInit()
        {
            _mockCoinGeckoClient = new Mock<CoinGecko.Interfaces.ICoinGeckoClient>();

            _cryptoCoinsControler = new CryptoCoinsController( _mockCoinGeckoClient.Object);

        }

        [TestMethod]
        public async Task TestMethod1()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 1;

            var expectedCoinMarkets = new List<CoinGecko.Entities.Response.Coins.CoinMarkets>
            {
                new CoinGecko.Entities.Response.Coins.CoinMarkets
                {
                    Id = "bitcoin",
                    Symbol= "btc"
                }
            };

            _mockCoinGeckoClient.Setup(srvc => srvc.CoinsClient.GetCoinMarkets(It.IsAny<string>(), 
                                                                               It.IsAny<string[]>(), 
                                                                               It.IsAny<string>(), 
                                                                               It.IsAny<int>(), 
                                                                               It.IsAny<int>(), 
                                                                               It.IsAny<bool>(), 
                                                                               It.IsAny<string>(),
                                                                               It.IsAny<string>())).ReturnsAsync(expectedCoinMarkets);

            //Act
            var result = await _cryptoCoinsControler.GetCoinMarkets();
            var coinsResult = ((OkObjectResult)result).Value;
            var CoinMarketsModel = (List<CoinGecko.Entities.Response.Coins.CoinMarkets>)coinsResult;

            //Assert
            Assert.IsNotNull(coinsResult);
            Assert.AreEqual(expectedResultCount, CoinMarketsModel.Count);
            Assert.AreSame(expectedCoinMarkets[0].Id, CoinMarketsModel[0].Id);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }
    }
}
