using Newtonsoft.Json;

namespace WbApiData
{
    public class FullInfo
    {
        [JsonProperty("nmId")] public int Articule { get; init; }
        [JsonProperty("vendorCode")] public required string SellerArticule { get; init; }
        [JsonProperty("name")] public required string Name { get; init; }
        [JsonProperty("mainPhoto")] public required string MainPhotoUrl { get; init; }
        [JsonProperty("rating")] public double Rating { get; init; }

        [JsonProperty("openCard")] public OpenCard OpenCard { get; init; } = new();
        [JsonProperty("addToCart")] public AddToCart AddToCart { get; init; } = new();
        [JsonProperty("orders")] public Orders Orders { get; init; } = new();

        [JsonProperty("openToCart")] public OpenToCartConversion OpenToCart { get; init; } = new();
        [JsonProperty("cartToOrder")] public CartToOrderConversion CartToOrder { get; init; } = new();
    }

    public class OpenCard
    {
        [JsonProperty("current")] public int Count { get; init; }
    }

    public class AddToCart
    {
        [JsonProperty("current")] public int Count { get; init; }
    }

    public class Orders
    {
        [JsonProperty("current")] public int Count { get; init; }
    }

    public class OpenToCartConversion
    {
        [JsonProperty("current")] public int Value { get; init; }
    }

    public class CartToOrderConversion
    {
        [JsonProperty("current")] public int Value { get; init; }
    }
}