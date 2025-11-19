using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Converters;

// DateOnly JSON Converter
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy-MM-dd";
    private readonly string[] _formats = { "yyyy-MM-dd", "yyyy/MM/dd", "MM/dd/yyyy", "dd/MM/yyyy" };

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return default;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            // Try parsing with different formats
            foreach (var format in _formats)
            {
                if (DateOnly.TryParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var result))
                {
                    return result;
                }
            }

            // Try standard parsing as fallback
            if (DateOnly.TryParse(value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var fallbackResult))
            {
                return fallbackResult;
            }
        }

        // If we can't parse, return default (0001-01-01)
        return default;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        // Handle default/minimum DateOnly value
        if (value == default || value.Year < 1900)
        {
            writer.WriteNullValue();
            return;
        }
        writer.WriteStringValue(value.ToString(DateFormat));
    }
}

// Nullable DateOnly JSON Converter
public class DateOnlyNullableJsonConverter : JsonConverter<DateOnly?>
{
    private const string DateFormat = "yyyy-MM-dd";
    private readonly string[] _formats = { "yyyy-MM-dd", "yyyy/MM/dd", "MM/dd/yyyy", "dd/MM/yyyy" };

    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            // Try parsing with different formats
            foreach (var format in _formats)
            {
                if (DateOnly.TryParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var result))
                {
                    return result;
                }
            }

            // Try standard parsing as fallback
            if (DateOnly.TryParse(value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var fallbackResult))
            {
                return fallbackResult;
            }
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString(DateFormat));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}

