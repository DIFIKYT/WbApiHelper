using Newtonsoft.Json;

namespace WbApiData
{
    public class StockReport
    {
        [JsonProperty("lastChangeDate")] public required string LastChangeDate { get; init; }
        [JsonProperty("supplierArticle")] public required string SupplierArticle { get; init; }
        [JsonProperty("techSize")] public required string TechSize { get; init; }
        [JsonProperty("barcode")] public required string Barcode { get; init; }
        [JsonProperty("quantity")] public int Quantity { get; init; }
        [JsonProperty("isSupply")] public bool IsSupply { get; init; }
        [JsonProperty("isRealization")] public bool IsRealization { get; init; }
        [JsonProperty("quantityFull")] public int QuantityFull { get; init; }
        [JsonProperty("warehouseName")] public required string WarehouseName { get; init; }
        [JsonProperty("inWayToClient")] public int InWayToClient { get; init; }
        [JsonProperty("inWayFromClient")] public int InWayFromClient { get; init; }
        [JsonProperty("nmId")] public int NmId { get; init; }
        [JsonProperty("subject")] public required string Subject { get; init; }
        [JsonProperty("category")] public required string Category { get; init; }
        [JsonProperty("brand")] public required string Brand { get; init; }
        [JsonProperty(nameof(Discount))] public double Discount { get; init; }
    }
}