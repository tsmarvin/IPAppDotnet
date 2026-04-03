using System.Text.Json.Serialization;

namespace IPApp.Models;

[JsonSerializable( typeof( IpResponse ) )]
internal partial class AppJsonContext : JsonSerializerContext;
