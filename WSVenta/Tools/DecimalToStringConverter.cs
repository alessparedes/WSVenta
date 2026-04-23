using System.Text.Json;
using System.Text.Json.Serialization;

namespace WSVenta.Tools;

public class DecimalToStringConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Si el JSON viene como string "10.50"
        if (reader.TokenType == JsonTokenType.String)
        {
            if (decimal.TryParse(reader.GetString(), out decimal result))
                return result;
        }

        // Si el JSON viene como número 10.50
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDecimal();
        }

        throw new JsonException($"No se puede convertir {reader.TokenType} a decimal.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        // Esto envía "10.50" al frontend
        writer.WriteStringValue(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }
}
