using System.Text.Json.Serialization;

public class ApiResponse
{
    [JsonPropertyName("USDBRL")] public MoedaData USDBRL { get; set; }
    [JsonPropertyName("EURBRL")] public MoedaData EURBRL { get; set; }
    [JsonPropertyName("GBPBRL")] public MoedaData GBPBRL { get; set; }
    [JsonPropertyName("JPYBRL")] public MoedaData JPYBRL { get; set; }
    [JsonPropertyName("CNYBRL")] public MoedaData CNYBRL { get; set; }
}

public class MoedaData
{
    [JsonPropertyName("bid")] public string Bid { get; set; }
}