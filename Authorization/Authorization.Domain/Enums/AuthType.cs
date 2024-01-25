using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Authorization.Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum AuthType
{
    Email,
}