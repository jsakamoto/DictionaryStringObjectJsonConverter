using System.Text.Json;
using System.Text.Json.Serialization;

namespace Toolbelt.Text.Json.Serialization;

public class DictionaryStringObjectJsonConverter : JsonConverter<Dictionary<string, object?>>
{
    public override Dictionary<string, object?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, object?>>(ref reader, options);
        ExtractValues(dictionary);
        return dictionary;
    }
    public override void Write(Utf8JsonWriter writer, Dictionary<string, object?> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }

    private static void ExtractValues(Dictionary<string, object?> dictionary)
    {
        foreach (var key in dictionary.Keys)
        {
            var value = dictionary[key];
            if (!(value is JsonElement jsonElement)) continue;
            dictionary[key] = ExtractValue(jsonElement);
        }
    }

    private static object? ExtractValue(JsonElement jsonElement)
    {
        return jsonElement.ValueKind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number =>
                jsonElement.TryGetInt32(out var n1) ? (object)n1 :
                jsonElement.TryGetInt64(out var n2) ? (object)n2 :
                jsonElement.GetDouble(),
            JsonValueKind.String => jsonElement.GetString(),
            JsonValueKind.Array => ExtractArray(jsonElement),
            JsonValueKind.Object => ExtractDictionary(jsonElement),
            _ => jsonElement
        };
    }

    private static object ExtractArray(JsonElement jsonElement)
    {
        var srcArray = jsonElement.EnumerateArray().Select(e => ExtractValue(e)).ToArray();
        if (srcArray.Length == 0) return srcArray;

        var hasNullElement = srcArray.Any(e => e == null);
        var nonNullEmenets = srcArray.Where(e => e != null).ToArray();
        if (nonNullEmenets.Length == 0) return srcArray;

        var initialType = nonNullEmenets[0]!.GetType();
        var typeOfArray = nonNullEmenets.Aggregate((Type?)initialType, (pre, cur) =>
        {
            if (pre == null) return null;
            var typeOfCur = cur!.GetType();
            if (typeOfCur == pre) return pre;
            if (typeOfCur == typeof(int) && (pre == typeof(long) || pre == typeof(double))) return pre;
            if (typeOfCur == typeof(long) && pre == typeof(int)) return typeOfCur;
            if (typeOfCur == typeof(long) && pre == typeof(double)) return pre;
            return null;
        });
        if (typeOfArray == null) return srcArray;

        var underType = typeOfArray;
        var isNullableValue = hasNullElement && typeOfArray.IsValueType;
        if (isNullableValue) typeOfArray = typeof(Nullable<>).MakeGenericType(typeOfArray);

        var convertedArray = Array.CreateInstance(typeOfArray, srcArray.Length);
        if (isNullableValue)
        {
            for (var i = 0; i < srcArray.Length; i++)
            {
                var rawValue = srcArray[i];
                var value = rawValue == null ? null : Activator.CreateInstance(typeOfArray, Convert.ChangeType(rawValue, underType));
                convertedArray.SetValue(value, i);
            }
        }
        else
        {
            for (var i = 0; i < srcArray.Length; i++)
            {
                convertedArray.SetValue(Convert.ChangeType(srcArray[i], typeOfArray), i);
            }
        }
        return convertedArray;
    }

    private static Dictionary<string, object?> ExtractDictionary(JsonElement jsonElement)
    {
        return jsonElement.EnumerateObject().ToDictionary(e => e.Name, e => ExtractValue(e.Value));
    }
}
