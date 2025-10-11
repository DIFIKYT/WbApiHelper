using Newtonsoft.Json;

namespace WbApiData
{
    public class Stock
    {
        [JsonProperty("nmId")] public int Articule { get; init; }
        [JsonProperty("vendorCode")] public required string SellerArticule { get; init; }
        [JsonProperty("barcode")] public required string Barcode { get; init; }
        [JsonProperty("techSize")] public int TechSize { get; init; }
        [JsonProperty("volume")] public double Volume { get; init; }
        [JsonProperty("wareHouses")] public List<Warehouse> Warehouses { get; init; } = [];
    }

    public class Warehouse
    {
        [JsonProperty("warehouseName")] public required string Name { get; init; }
        [JsonProperty("quantity")] public int Quantity { get; init; }
    }
}