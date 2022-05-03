using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Converters
{
    /// <remarks>
    /// https://github.com/dotnet/aspnetcore/issues/17999
    /// </remarks>
    public class ProblemDetailsJsonConverter : JsonConverter<ProblemDetails>
    {
        private static readonly JsonEncodedText Type = JsonEncodedText.Encode("type");
        private static readonly JsonEncodedText Title = JsonEncodedText.Encode("title");
        private static readonly JsonEncodedText Status = JsonEncodedText.Encode("status");
        private static readonly JsonEncodedText Detail = JsonEncodedText.Encode("detail");
        private static readonly JsonEncodedText Instance = JsonEncodedText.Encode("instance");
        public override ProblemDetails Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var problemDetails = new ProblemDetails();

            if (!reader.Read())
            {
                throw new JsonException();
            }

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                ReadValue(ref reader, problemDetails, options);
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return problemDetails;
        }
        public override void Write(Utf8JsonWriter writer, ProblemDetails value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteProblemDetails(writer, value, options);
            writer.WriteEndObject();
        }
        internal static void ReadValue(ref Utf8JsonReader reader, ProblemDetails value, JsonSerializerOptions options)
        {
            if (TryReadStringProperty(ref reader, Type, out String propertyValue))
            {
                value.Type = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, Title, out propertyValue))
            {
                value.Title = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, Detail, out propertyValue))
            {
                value.Detail = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, Instance, out propertyValue))
            {
                value.Instance = propertyValue;
            }
            else if (reader.ValueTextEquals(Status.EncodedUtf8Bytes))
            {
                reader.Read();
                if (reader.TokenType == JsonTokenType.Null)
                {
                    // Nothing to do here.
                }
                else
                {
                    value.Status = reader.GetInt32();
                }
            }
            else
            {
                String key = reader.GetString();
                reader.Read();
                value.Extensions[key] = JsonSerializer.Deserialize(ref reader, typeof(Object), options);
            }
        }
        internal static Boolean TryReadStringProperty(ref Utf8JsonReader reader, JsonEncodedText propertyName, out String value)
        {
            if (!reader.ValueTextEquals(propertyName.EncodedUtf8Bytes))
            {
                value = default;
                return false;
            }

            reader.Read();
            value = reader.GetString();
            return true;
        }
        internal static void WriteProblemDetails(Utf8JsonWriter writer, ProblemDetails value, JsonSerializerOptions options)
        {
            if (value.Type != null)
            {
                writer.WriteString(Type, value.Type);
            }

            if (value.Title != null)
            {
                writer.WriteString(Title, value.Title);
            }

            if (value.Status != null)
            {
                writer.WriteNumber(Status, value.Status.Value);
            }

            if (value.Detail != null)
            {
                writer.WriteString(Detail, value.Detail);
            }

            if (value.Instance != null)
            {
                writer.WriteString(Instance, value.Instance);
            }

            foreach (KeyValuePair<String, Object> kvp in value.Extensions)
            {
                writer.WritePropertyName(options?.DictionaryKeyPolicy?.ConvertName(kvp.Key) ?? kvp.Key);
                JsonSerializer.Serialize(writer, kvp.Value, kvp.Value?.GetType() ?? typeof(Object), options);
            }
        }
    }
}