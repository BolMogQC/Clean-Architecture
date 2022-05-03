using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            String? read = reader.GetString();
            return String.IsNullOrEmpty(read) ? DateTime.MinValue.ToUniversalTime() : DateTime.Parse(read).ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
    }
}