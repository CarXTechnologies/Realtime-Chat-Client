using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Reatime_Chat.utils;

public static class JsonUtils
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    

    public static string ToJson(object item)
    {
        return JsonSerializer.Serialize(item, Options);
    }

    public static T FromJson<T>([StringSyntax("Json")] string json)
    {
        return JsonSerializer.Deserialize<T>(json, Options);
    }
}