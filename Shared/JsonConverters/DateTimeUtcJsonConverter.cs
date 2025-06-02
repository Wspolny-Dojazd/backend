using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.JsonConverters;

/// <summary>
/// Represents a JSON converter for <see cref="DateTime"/> values that ensures
/// all date-times are serialized in UTC.
/// </summary>
public class DateTimeUtcJsonConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var utc = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        writer.WriteStringValue(utc.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}
