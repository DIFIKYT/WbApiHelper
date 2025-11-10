using Newtonsoft.Json;

namespace WbApiData
{
    public class ArticulesCompanyStat()
    {
        [JsonProperty("advertId")] public int AdvertId { get; init; }
        [JsonProperty("days")] public required List<ArticuleCompanyDay> ArticuleCompanyDays { get; init; }
    }

    public class ArticuleCompanyDay
    {
        [JsonProperty("date")] public required string Date { get; init; }
        [JsonProperty("apps")] public required List<App> Apps { get; init; }
    }

    public class App
    {
        [JsonProperty("nms")] public required List<Nm> Nms { get; init; }
    }

    public class Nm
    {
        [JsonProperty("atbs")] public int Atbs { get; init; }
        [JsonProperty("clicks")] public int Clicks { get; init; }
        [JsonProperty("cr")] public double Cr { get; init; }
        [JsonProperty("name")] public required string Name { get; init; }
        [JsonProperty("nmId")] public int NmId { get; init; }
        [JsonProperty("orders")] public int Orders { get; init; }
        [JsonProperty("shks")] public int Shks { get; init; }
        [JsonProperty("sum")] public double Sum { get; init; }
        [JsonProperty("sum_price")] public double SumPrice { get; init; }
        [JsonProperty("views")] public int Views { get; init; }
    }
}