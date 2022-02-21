using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Orlfi.EgrulLibrary.JsonConverters;

public class IntConverter : JsonConverter<int>
{
    public override int Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(int));

        return int.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
    }

    public override void Write(
        Utf8JsonWriter writer,
        int value,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
}
