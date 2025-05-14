using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Shared.JsonConverters;

public class DateTimeUtcJsonConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();

        if (string.IsNullOrWhiteSpace(stringValue))
        { 
            throw new JsonException("Cannot parse DateTime from empty string."); 
        }

        if (DateTimeOffset.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dto))
        {
            return dto.UtcDateTime;
        }

        throw new JsonException($"Invalid DateTime format: {stringValue}");
    }
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var utc = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        writer.WriteStringValue(utc.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}