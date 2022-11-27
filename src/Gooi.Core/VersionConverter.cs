using System.Text.Json;
using System.Text.Json.Serialization;

namespace Skinder.Gooi.Core;
public class VersionConverter : JsonConverter<Version>
{
  public override Version Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var value = reader.GetString();
    if (string.IsNullOrEmpty(value))
      value = "0.0.0";
    return new Version(value);
  }

  public override void Write(Utf8JsonWriter writer, Version value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.ToString());
  }
}