using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Infrastructure.Converters.ProblemDetailsJsonConverter;

namespace Infrastructure.Converters
{
    /// <remarks>
    /// https://github.com/dotnet/aspnetcore/issues/17999
    /// </remarks>
    public class ValidationProblemDetailsJsonConverter : JsonConverter<ValidationProblemDetails>
    {
        private static readonly JsonEncodedText Errors = JsonEncodedText.Encode("errors");
        public override ValidationProblemDetails Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var problemDetails = new ValidationProblemDetails();

            if (!reader.Read())
            {
                throw new JsonException();
            }

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.ValueTextEquals(Errors.EncodedUtf8Bytes))
                {
                    var errors = JsonSerializer.Deserialize<Dictionary<String, String[]>>(ref reader, options);
                    foreach (KeyValuePair<String, String[]> item in errors)
                    {
                        problemDetails.Errors[item.Key] = item.Value;
                    }
                }
                else
                {
                    ReadValue(ref reader, problemDetails, options);
                }
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return problemDetails;
        }
        public override void Write(Utf8JsonWriter writer, ValidationProblemDetails value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteProblemDetails(writer, value, options);

            writer.WriteStartObject(Errors);
            foreach (KeyValuePair<String, String[]> kvp in value.Errors)
            {
                writer.WritePropertyName(options?.DictionaryKeyPolicy?.ConvertName(kvp.Key) ?? kvp.Key);
                JsonSerializer.Serialize(writer, kvp.Value, kvp.Value?.GetType() ?? typeof(Object), options);
            }
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}