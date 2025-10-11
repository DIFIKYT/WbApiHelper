using Newtonsoft.Json;

namespace WbApiData
{
    public class Card
    {
        [JsonProperty("nmID")] public int Articule { get; init; }

        [JsonProperty("vendorCode")] public required string SellerArticule { get; init; }

        [JsonProperty("photos")] public List<Photo> Photos { get; init; } = [];

        [JsonProperty("dimensions")] public Dimensions Dimensions { get; init; } = new();

        [JsonProperty("sizes")] public List<Size> Sizes { get; init; } = [];
    }

    public class Photo
    {
        [JsonProperty("big")] public string Big { get; init; } = string.Empty;
    }

    public class Dimensions
    {
        [JsonProperty("length")] public int Length { get; init; }

        [JsonProperty("width")] public int Width { get; init; }

        [JsonProperty("height")] public int Height { get; init; }

        [JsonProperty("weightBrutto")] public double WeightBrutto { get; init; }
    }

    public class Size
    {
        [JsonProperty("techSize")] public string TechSize { get; init; } = string.Empty;

        [JsonProperty("skus")] public List<string> Skus { get; init; } = [];
    }
}
