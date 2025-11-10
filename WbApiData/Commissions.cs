using Newtonsoft.Json;

namespace WbApiData
{
    public class Commissions
    {
        [JsonProperty("kgvpBooking")] public double Booking { get; init; }
        [JsonProperty("kgvpMarketplace")] public double FBS { get; init; }
        [JsonProperty("kgvpPickup")] public double Pickup { get; init; }
        [JsonProperty("kgvpSupplier")] public double DBSAndDBW { get; init; }
        [JsonProperty("kgvpSupplierExpress")] public double EDBS { get; init; }
        [JsonProperty("paidStorageKgvp")] public double FBW { get; init; }
        [JsonProperty("parentID")] public int ParentID { get; init; }
        [JsonProperty("parentName")] public required string ParentName { get; init; }
        [JsonProperty("subjectID")] public int SubjectID { get; init; }
        [JsonProperty("subjectName")] public required string SubjectName { get; init; }
    }
}