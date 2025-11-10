using Newtonsoft.Json;

namespace WbApiData
{
    public class CostHistory
    {
        [JsonProperty("updTime")] public required string UpdTime { get; init; }
        [JsonProperty("advertId")] public int AdvertId { get; init; }
        [JsonProperty("updSum")] public int UpdSum { get; init; }
        [JsonProperty("updNum")] public int UpdNum { get; init; }
        [JsonProperty("advertType")] public int AdvertType { get; init; }
        [JsonProperty("paymentType")] public required string PaymentType { get; init; }
        [JsonProperty("advertStatus")] public int AdvertStatus { get; init; }
        [JsonProperty("campName")] public required string CampName { get; init; }
    }
}