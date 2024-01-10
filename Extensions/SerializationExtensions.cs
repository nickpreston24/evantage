using Newtonsoft.Json;

namespace CodeMechanic.Extensions;

public static class SerializationExtensions
{
    public static string AsJSON<T>(this T value) => value != null ? JsonConvert.SerializeObject(value) : "";
}