using Newtonsoft.Json;

namespace WbApiData
{
    public class Sale
    {
        private int _quantity;

        [JsonProperty("nmId")] public int Articule { get; init; }
        [JsonProperty("supplierArticle")] public required string SellerArticule { get; init; }
        [JsonProperty("barcode")] public required string Barcode { get; init; }
        [JsonProperty("techSize")] public required string TechSize { get; init; }
        [JsonProperty("spp")] public int SPP { get; init; }
        public int Quantity => _quantity;

        public void IncreaseQuantity() =>
            _quantity++;
    }
}