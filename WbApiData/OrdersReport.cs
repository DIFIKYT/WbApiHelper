using Newtonsoft.Json;

namespace WbApiData
{
    public class OrdersReport
    {
        private int _quantity = 1;

        [JsonProperty("date")] public required string Date { get; init; }
        [JsonProperty("lastChangeDate")] public required string LastChangeDate { get; init; }
        [JsonProperty("supplierArticle")] public required string SellerArticule { get; init; }
        [JsonProperty("techSize")] public required string TechSize { get; init; }
        [JsonProperty("barcode")] public required string Barcode { get; init; }
        [JsonProperty("totalPrice")] public double TotalPrice { get; init; }
        [JsonProperty("discountPercent")] public int DiscountPercent { get; init; }
        [JsonProperty("warehouseName")] public required string WarehouseName { get; init; }
        [JsonProperty("regionName")] public required string RegionName { get; init; }
        [JsonProperty("incomeID")] public int IncomeID { get; init; }
        [JsonProperty("priceWithDisc")] public double PriceWithDisc { get; init; }
        [JsonProperty("nmId")] public int NmId { get; init; }
        [JsonProperty("subject")] public required string Subject { get; init; }
        [JsonProperty("category")] public required string Category { get; init; }
        [JsonProperty("brand")] public required string Brand { get; init; }
        [JsonProperty("isCancel")] public bool IsCancel { get; init; }
        [JsonProperty("cancelDate")] public required string CancelDate { get; init; }
        [JsonProperty("finishedPrice")] public double FinishedPrice { get; init; }
        [JsonProperty("sticker")] public required string Sticker { get; init; }
        [JsonProperty("srid")] public required string Srid { get; init; }
        [JsonProperty("spp")] public int SPP { get; init; }

        public int Quantity => _quantity;

        public void MergeWith(OrdersReport other)
        {
            if (other.NmId == NmId)
                _quantity += other._quantity;
        }
    }
}