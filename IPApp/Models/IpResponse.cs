using System.Text.Json.Serialization;

namespace IPApp.Models;

public sealed class IpResponse {
    [JsonPropertyName( "ip" )]
    public string Ip { get; set; } = string.Empty;

    [JsonPropertyName("protocol")]
    public string Protocol { get; set; } = string.Empty;
}
