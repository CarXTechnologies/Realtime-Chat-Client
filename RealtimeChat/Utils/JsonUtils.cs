using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RealtimeChat.Utils;

/// <summary>
/// Json utils
/// </summary>
internal static class JsonUtils
{
    private static readonly JsonSerializerSettings Options = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        },
        Formatting = Formatting.Indented
    };

    /// <summary>
    /// Serializing object to json
    /// </summary>
    /// <param name="item">object for serialization</param>
    /// <returns>json string</returns>
    public static string ToJson(object item)
    {
        return JsonConvert.SerializeObject(item, Options);
    }

    /// <summary>
    /// Deserializing from json string to object 
    /// </summary>
    /// <param name="json">json string</param>
    /// <typeparam name="T">type of target item</typeparam>
    /// <returns></returns>
    public static T? FromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, Options);
    }
}