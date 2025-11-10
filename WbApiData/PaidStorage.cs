using Newtonsoft.Json;

namespace WbApiData
{
    public class PaidStorage
    {
        private double _sumPrice;

        [JsonProperty("officeId")] public int OfficeId { get; init; }
        [JsonProperty("warehouse")] public required string Warehouse { get; init; }
        [JsonProperty("warehouseCoef")] public double WarehouseCoef { get; init; }
        [JsonProperty("giId")] public required string GiId { get; init; }
        [JsonProperty("size")] public required string Size { get; init; }
        [JsonProperty("barcode")] public required string Barcode { get; init; }
        [JsonProperty("subject")] public required string Subject { get; init; }
        [JsonProperty("brand")] public required string Brand { get; init; }
        [JsonProperty("vendorCode")] public required string SellerArticle { get; init; }
        [JsonProperty("nmId")] public int NmId { get; init; }
        [JsonProperty("volume")] public double Volume { get; init; }
        [JsonProperty("calcType")] public required string CalcType { get; init; }
        [JsonProperty("warehousePrice")] public double KeepingPrice { get; init; }
        [JsonProperty("barcodesCount")] public int BarcodesCount { get; init; }
        [JsonProperty("palletPlaceCode")] public int PalletPlaceCode { get; init; }
        [JsonProperty("palletCount")] public int PalletCount { get; init; }
        [JsonProperty("loyaltyDiscount")] public double LoyaltyDiscount { get; init; }
        [JsonProperty("tariffFixDate")] public required string TariffFixDate { get; init; }
        [JsonProperty("tariffLowerDate")] public required string TariffLowerDate { get; init; }

        public double SumPrice => _sumPrice;

        public void GetStartPrice(double startPrice)
        {
            _sumPrice = startPrice;
        }

        public void MergeWith(PaidStorage other)
        {
            if (other.NmId == NmId)
                _sumPrice += other.KeepingPrice;
        }
    }
}