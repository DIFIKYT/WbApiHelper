// using Newtonsoft.Json;

// namespace WbApiData
// {
//     public class Statistic
//     {
//         [JsonProperty("nmID")] public int Articule { get; init; }
//         [JsonProperty("vendorCode")] public required string SellerArticule { get; init; }
//         [JsonProperty("statistics")] public required Statistics Statistics { get; init; }
//     }

//     public class Statistics
//     {
//         [JsonProperty("selectedPeriod")] public SelectedPeroid SelectedPeroid { get; init; } = new();
//     }

//     public class SelectedPeroid
//     {
//         [JsonProperty("openCardCount")] public int OpenCardCount { get; init; }
//         [JsonProperty("addToCartCount")] public int AddToCartCount { get; init; }
//         [JsonProperty("ordersCount")] public int OrdersCount { get; init; }
//         [JsonProperty("ordersSumRub")] public int OrdersSumRub { get; init; }
//         [JsonProperty("buyoutsCount")] public int BuyoutsCount { get; init; }
//         [JsonProperty("buyoutsSumRub")] public int BuyoutsSumRub { get; init; }
//         [JsonProperty("avgPriceRub")] public int AveragePrice { get; init; }
//         [JsonProperty("conversions")] public Conversions Conversions { get; init; } = new();
//     }

//     public class Conversions
//     {
//         [JsonProperty("addToCartPercent")] public int AddToCartPercent { get; init; }
//         [JsonProperty("cartToOrderPercent")] public int CartToOrderPercent { get; init; }
//         [JsonProperty("buyoutsPercent")] public int BuyoutsPercent { get; init; }
//     }
// }

using Newtonsoft.Json;

namespace WbApiData
{
    public class Statistic
    {
        [JsonProperty("product")] public required Product Product { get; init; }
        [JsonProperty("statistic")] public required Statistics Statistics { get; init; }
    }

    public class Product
    {
        [JsonProperty("nmId")] public int NmId { get; init; }
        [JsonProperty("vendorCode")] public required string VendorCode { get; init; }
        [JsonProperty("productRating")] public double ProductRating { get; init; }
        [JsonProperty("feedbackRating")] public double FeedbackRating { get; init; }

    }

    public class Statistics
    {
        [JsonProperty("openCount")] public int OpenCount { get; init; }
        [JsonProperty("cartCount")] public int CartCount { get; init; }
        [JsonProperty("orderCount")] public int OrderCount { get; init; }
        [JsonProperty("orderSum")] public int OrderSum { get; init; }
        [JsonProperty("buyoutCount")] public int BuyoutCount { get; init; }
        [JsonProperty("buyoutSum")] public int BuyoutSum { get; init; }
        [JsonProperty("avgPrice")] public int AvgPrice { get; init; }
        [JsonProperty("conversions")] public Conversions Conversions { get; init; } = new();
    }

    public class Conversions
    {
        [JsonProperty("addToCartPercent")] public int AddToCartPercent { get; init; }
        [JsonProperty("cartToOrderPercent")] public int CartToOrderPercent { get; init; }
        [JsonProperty("buyoutPercent")] public int BuyoutPercent { get; init; }
    }
}