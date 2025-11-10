using Newtonsoft.Json;
using System.Text.Json;

namespace WbApiData
{
    public class Logistic
    {
        [JsonProperty("boxDeliveryBase")] public required string LogisticFirstLiter { get; init; }
        [JsonProperty("boxDeliveryMarketplaceBase")] public required string LogisticAdditionalLiter { get; init; }
        [JsonProperty("boxDeliveryCoefExpr")] public required string LogisticCoefExpr { get; init; }

        [JsonProperty("boxStorageBase")] public required string StorageFirstLiter { get; init; }
        [JsonProperty("boxStorageLiter")] public required string StorageAdditionalLiter { get; init; }
        [JsonProperty("boxStorageCoefExpr")] public required string StorageCoefExpr { get; init; }

        [JsonProperty("warehouseName")] public required string WarehouseName { get; init; }
    }
}