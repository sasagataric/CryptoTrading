
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoTrading.API
{
    public class MarketData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("image")]
        public Uri Image { get; set; }

        [JsonPropertyName("current_price")]
        public decimal? CurrentPrice { get; set; }

        [JsonPropertyName("market_cap")]
        public decimal? MarketCap { get; set; }

        [JsonPropertyName("total_volume")]
        public decimal? TotalVolume { get; set; }

        [JsonPropertyName("high_24h")]
        public decimal? High24H { get; set; }

        [JsonPropertyName("low_24h")]
        public decimal? Low24H { get; set; }

        [JsonPropertyName("ath")]
        public decimal? Ath { get; set; }

        [JsonPropertyName("ath_change_percentage")]
        public decimal? AthChangePercentage { get; set; }

        [JsonPropertyName("ath_date")]
        public DateTimeOffset AthDate { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }
    }
}
