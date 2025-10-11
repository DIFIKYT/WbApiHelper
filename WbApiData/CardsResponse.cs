using Newtonsoft.Json;

namespace WbApiData
{
    public class CardsResponse
    {
        [JsonProperty("cursor")] public Cursor Cursor { get; init; } = new();
        [JsonProperty("cards")] public List<Card> Cards { get; init; } = new();
    }

    public class Cursor
    {
        [JsonProperty("total")] public int Total { get; init; }
        [JsonProperty("updatedAt")] public string UpdatedAt { get; init; } = string.Empty;
        [JsonProperty("nmID")] public int NmID { get; init; }
    }
}
