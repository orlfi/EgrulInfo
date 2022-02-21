using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Orlfi.EgrulLibrary.JsonConverters;

public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(DateTime));

        return DateTimeOffset.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTimeOffset dateTimeValue,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(dateTimeValue.ToString());
}
