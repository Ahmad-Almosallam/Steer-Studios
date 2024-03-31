using MongoDB.Bson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Steer.Api.JsonConverters
{
    public class ObjectIdJsonConverter : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var q = jsonDoc.RootElement.GetString();
                //ObjectId.
                return ObjectId.Parse(q);
            }
        }

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        {
            var q = value.ToString();
            writer.WriteStringValue(q);
        }
    }
}
