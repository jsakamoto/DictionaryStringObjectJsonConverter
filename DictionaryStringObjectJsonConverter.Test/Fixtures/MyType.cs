using Toolbelt.Text.Json.Serialization;

namespace Toolbelt.Text.Json.Test.Fixtures;

public class MyType
{
    [DictionaryStringObjectJsonConverter]
    public Dictionary<string, object?> Items { get; set; } = new();
}
