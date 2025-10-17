namespace Fundo.Shared.Helper;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Reflection;

public class EnumMemberJsonConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString();
        if (text is null)
            throw new JsonException();

        foreach (var value in Enum.GetValues(typeof(T)))
        {
            var member = typeof(T).GetMember(value.ToString()!)[0];
            var attr = member.GetCustomAttribute<EnumMemberAttribute>();
            if ((attr?.Value ?? value.ToString()) == text)
                return (T)value;
        }

        throw new JsonException($"Unable to map '{text}' to enum '{typeof(T).Name}'");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var member = typeof(T).GetMember(value.ToString()!)[0];
        var attr = member.GetCustomAttribute<EnumMemberAttribute>();
        writer.WriteStringValue(attr?.Value ?? value.ToString());
    }
}