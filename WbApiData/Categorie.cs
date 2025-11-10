using Newtonsoft.Json;
using System.Text.Json;

namespace WbApiData
{
    public class Categorie
    {
        [JsonProperty("subjectID")] public int SubjectId { get; init; }
        [JsonProperty("parentID")] public int ParentID { get; init; }
        [JsonProperty("subjectName")] public required string SubjectName { get; init; }
        [JsonProperty("parentName")] public required string ParentName { get; init; }
    }
}