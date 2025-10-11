using Newtonsoft.Json;

namespace WbApiData
{
    public class PriceAndDiscount
    {
        [JsonProperty("nmID")] public int Articule { get; init; }
        [JsonProperty("vendorCode")] public required string SellerArticule { get; init; }
        [JsonProperty("sizes")] public List<PriceAndDiscountStat> PriceAndDiscountStats { get; init; } = [];
    }

    public class PriceAndDiscountStat
    {
        [JsonProperty("price")] public int Price { get; init; }
        [JsonProperty("discountedPrice")] public double DiscountedPrice { get; init; }
    }
}