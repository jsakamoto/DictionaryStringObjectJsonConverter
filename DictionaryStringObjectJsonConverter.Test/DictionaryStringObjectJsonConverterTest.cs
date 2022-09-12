using System.Text.Json;
using Toolbelt.Text.Json.Test.Fixtures;

namespace DictionaryStringObjectJsonConverter.Test;

public class Tests
{
    [Test]
    public void Deserialize_Test()
    {
        var stringValue1 = Guid.NewGuid().ToString("N");
        var stringValue2 = Guid.NewGuid().ToString("N");
        var stringValue3 = Guid.NewGuid().ToString("N");
        var json = $@"{{
            ""Items"":{{
                ""IntMinValue"": {int.MinValue},
                ""IntMaxValue"": {int.MaxValue},
                ""LongMinValue"": {long.MinValue},
                ""LongMaxValue"": {long.MaxValue},
                ""DecimalMinValue"": {decimal.MinValue},
                ""DecimalMaxValue"": {decimal.MaxValue},
                ""DoubleValue"": 1.2,
                ""String"": ""{stringValue1}"",
                ""EmptyArray"": [],
                ""NullArray"": [null, null, null],
                ""IntArray"": [{int.MinValue}, 0, {int.MaxValue}],
                ""LongArray"": [{long.MinValue}, {int.MinValue}, {long.MaxValue}],
                ""DoubleArray"": [-0.1, 1, {long.MaxValue}],
                ""StringArray"": [""{stringValue1}"", ""{stringValue2}"", null, ""{stringValue3}""],
                ""NullableIntArray"": [{int.MinValue}, null, {int.MaxValue}],
                ""NullableLongArray"": [null, {int.MinValue}, {long.MaxValue}],
                ""NullableDoubleArray"": [{decimal.MinValue}, 0.3, null],
                ""Null"": null,
                ""True"": true,
                ""False"": false,
                ""Dictionary"": {{
                    ""IntMinValue"": {int.MinValue},
                    ""IntMaxValue"": {int.MaxValue},
                    ""LongMinValue"": {long.MinValue},
                    ""LongMaxValue"": {long.MaxValue},
                    ""DecimalMinValue"": {decimal.MinValue},
                    ""DecimalMaxValue"": {decimal.MaxValue},
                    ""DoubleValue"": 1.2,
                    ""String"": ""{stringValue1}"",
                    ""EmptyArray"": [],
                    ""NullArray"": [null, null, null],
                    ""IntArray"": [{int.MinValue}, 0, {int.MaxValue}],
                    ""LongArray"": [{long.MinValue}, {int.MinValue}, {long.MaxValue}],
                    ""DoubleArray"": [-0.1, 1, {long.MaxValue}],
                    ""StringArray"": [""{stringValue1}"", ""{stringValue2}"", null, ""{stringValue3}""],
                    ""NullableIntArray"": [{int.MinValue}, null, {int.MaxValue}],
                    ""NullableLongArray"": [null, {int.MinValue}, {long.MaxValue}],
                    ""NullableDoubleArray"": [{decimal.MinValue}, 0.3, null],
                    ""Null"": null,
                    ""True"": true,
                    ""False"": false
                }}
            }}
        }}";

        var obj = JsonSerializer.Deserialize<MyType>(json);
        obj.IsNotNull();
        validate(obj.Items);
        validate(obj.Items["Dictionary"].IsInstanceOf<Dictionary<string, object?>>());

        void validate(Dictionary<string, object?> items)
        {
            items["IntMinValue"].IsInstanceOf<int>().Is(int.MinValue);
            items["IntMaxValue"].IsInstanceOf<int>().Is(int.MaxValue);
            items["LongMinValue"].IsInstanceOf<long>().Is(long.MinValue);
            items["LongMaxValue"].IsInstanceOf<long>().Is(long.MaxValue);
            items["DecimalMaxValue"].IsInstanceOf<double>().Is((double)decimal.MaxValue);
            items["DecimalMinValue"].IsInstanceOf<double>().Is((double)decimal.MinValue);
            items["DoubleValue"].IsInstanceOf<double>().Is(1.2);
            items["String"].IsInstanceOf<string>().Is(stringValue1);

            items["NullArray"].IsInstanceOf<object?[]>().Is(new object?[] { null, null, null });
            items["EmptyArray"].IsInstanceOf<object[]>().Length.Is(0);
            items["IntArray"].IsInstanceOf<int[]>().Is(int.MinValue, 0, int.MaxValue);
            items["LongArray"].IsInstanceOf<long[]>().Is(long.MinValue, int.MinValue, long.MaxValue);
            items["DoubleArray"].IsInstanceOf<double[]>().Is(-0.1, 1.0, long.MaxValue);
            items["StringArray"].IsInstanceOf<string?[]>().Is(stringValue1, stringValue2, null, stringValue3);

            items["NullableIntArray"].IsInstanceOf<int?[]>().Is(int.MinValue, null, int.MaxValue);
            items["NullableLongArray"].IsInstanceOf<long?[]>().Is(null, int.MinValue, long.MaxValue);
            items["NullableDoubleArray"].IsInstanceOf<double?[]>().Is((double)decimal.MinValue, 0.3, null);

            items["Null"].IsNull();
            items["True"].IsInstanceOf<bool>().IsTrue();
            items["False"].IsInstanceOf<bool>().IsFalse();
        };
    }
}