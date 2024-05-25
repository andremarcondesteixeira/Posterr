using System.Text.Json;
using System.Text.Json.Serialization;

namespace Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

public class LinksConverter : JsonConverter<Dictionary<string, List<APIResourceLinkDTO>>>
{
    public override Dictionary<string, List<APIResourceLinkDTO>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, List<APIResourceLinkDTO>> value, JsonSerializerOptions options)
    {
        if (value.Count == 0) return;

        writer.WriteStartObject();

        foreach (var entry in value) {
            writer.WritePropertyName(entry.Key);

            if (entry.Value.Count == 0) continue;

            if (entry.Value.Count == 1)
            {
                writer.WriteStartObject();
                writer.WriteString("href", entry.Value[0].Href);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartArray();
                foreach (var link in entry.Value)
                {
                    writer.WriteStartObject();
                    writer.WriteString("href", link.Href);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
        }

        writer.WriteEndObject();
    }
}
