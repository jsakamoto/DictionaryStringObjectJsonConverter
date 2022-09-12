# DictionaryStringObjectJsonConverter [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Text.Json.Serialization.DictionaryStringObjectJsonConverter.svg)](https://www.nuget.org/packages/Toolbelt.Text.Json.Serialization.DictionaryStringObjectJsonConverter/) [![unit tests](https://github.com/jsakamoto/DictionaryStringObjectJsonConverter/actions/workflows/unit-tests.yml/badge.svg?branch=main&event=push)](https://github.com/jsakamoto/DictionaryStringObjectJsonConverter/actions/workflows/unit-tests.yml)

## Summary

This is an attribute and a converter on the `System.Text.Json` infrastructure to deserialize a JSON string to a `Dictionary<string, object>` object with each value of appropriate basic type, not `System.Text.JsonElement`.

## Usage

1. Add the `Toolbelt.Text.Json.Serialization.DictionaryStringObjectJsonConverter` NuGet package to your .NET project.

```shell
dotnet add package Toolbelt.Text.Json.Serialization.DictionaryStringObjectJsonConverter
```

2. Add the `[DictionaryStringObjectJsonConverter]` attribute to a property of the `Dictionary<string, object>` type.

```csharp
using Toolbelt.Text.Json.Serialization;

public class MyType
{
    [DictionaryStringObjectJsonConverter]
    public Dictionary<string, object?> Items { get; set; } = new();
}
```

3. You can deserialize a JSON string to an object of the `Dictionary<string, object>` type.

```csharp
using System.Text.Json;

var json = @"{\"items\": {
    \"Lorem\": 1,
    \"ipsum\": \"Two\", 
    \"amets\": [3.4, 5.6],
    \"dolor\": true
  }
}";

var obj = JsonSerializer.Deserialize<MyType>(json);
obj.Items["Lorem"]; // -> 1, typeof(int)
obj.Items["ipsum"]; // -> "Two", typeof(string)
obj.Items["amets"]; // -> new[]{ 3.4, 5.6 }, typeof(double[])
obj.Items["dolor"]; // -> true, typeof(bool)
```

If you didn't add the `[DictionaryStringObjectJsonConverter]` attribute to a property of the `Dictionary<string, object>` type, you would get all of the values in that dictionary object as the `System.Text.JsonElement` struct value instead of an appropriate basic type value of the .NET.

## License

[Mozilla Public License, version 2.0](https://github.com/jsakamoto/DictionaryStringObjectJsonConverter/blob/main/LICENSE)