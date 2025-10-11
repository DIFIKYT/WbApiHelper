using Newtonsoft.Json;

namespace WbApiData
{
    public class Statistic
    {
        [JsonProperty("nmID")] public int Articule { get; init; }
        [JsonProperty("vendorCode")] public required string SellerArticule { get; init; }
        [JsonProperty("selectedPeriod")] public StatisticPeroid StatisticPeroid { get; init; } = new();
    }

    public class StatisticPeroid
    {
        [JsonProperty("openCardCount")] public int OpenCardCount { get; init; }
        [JsonProperty("addToCartCount")] public int AddToCartCount { get; init; }
        [JsonProperty("ordersCount")] public int OrdersCount { get; init; }
        [JsonProperty("ordersSumRub")] public int OrdersSumRub { get; init; }
        [JsonProperty("buyoutsCount")] public int BuyoutsCount { get; init; }
        [JsonProperty("buyoutsSumRub")] public int BuyoutsSumRub { get; init; }
        [JsonProperty("conversions")] public Conversions Conversions { get; init; } = new();
    }

    public class Conversions
    {
        [JsonProperty("addToCartPercent")] public int AddToCartPercent { get; init; }
        [JsonProperty("cartToOrderPercent")] public int CartToOrderPercent { get; init; }
        [JsonProperty("buyoutsPercent")] public int BuyoutsPercent { get; init; }
    }
}