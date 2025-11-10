using Newtonsoft.Json;

namespace WbApiData
{
    public class Company
    {
        [JsonProperty("advertId")] public int AdvertId { get; init; }
        [JsonProperty("name")] public required string Name { get; init; }
        [JsonProperty("type")] public int Type { get; init; }
        [JsonProperty("status")] public int Status { get; init; }
        [JsonProperty("dailyBudget")] public int DailyBudget { get; init; }
        [JsonProperty("createTime")] public required string CreateTime { get; init; }
        [JsonProperty("changeTime")] public required string ChangeTime { get; init; }
        [JsonProperty("startTime")] public required string StartTime { get; init; }
        [JsonProperty("endTime")] public required string EndTime { get; init; }
    }
}