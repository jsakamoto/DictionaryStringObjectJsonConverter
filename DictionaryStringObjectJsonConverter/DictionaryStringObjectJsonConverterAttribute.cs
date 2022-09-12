using System.Text.Json.Serialization;

namespace Toolbelt.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DictionaryStringObjectJsonConverterAttribute : JsonConverterAttribute
{
	public DictionaryStringObjectJsonConverterAttribute() : base(converterType: typeof(DictionaryStringObjectJsonConverter))
	{
	}
}
