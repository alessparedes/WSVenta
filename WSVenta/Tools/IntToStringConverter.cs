using System.Text.Json;
using System.Text.Json.Serialization;

namespace WSVenta.Tools;

public class IntToStringConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Caso 1: El JSON viene como string "123"
        if (reader.TokenType == JsonTokenType.String)
        {
            string? value = reader.GetString();
            if (int.TryParse(value, out int result))
            {
                return result;
            }
        }

        // Caso 2: El JSON viene como número puro 123
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        // Si llega algo que no es ni número ni string convertible, lanzamos excepción
        throw new JsonException($"No se pudo convertir el token {reader.TokenType} a Int32.");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        // Siempre enviamos al frontend como string "123"
        writer.WriteStringValue(value.ToString());
    }
}