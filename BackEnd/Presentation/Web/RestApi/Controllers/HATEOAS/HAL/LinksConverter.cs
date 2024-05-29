using System.Text.Json;
using System.Text.Json.Serialization;

namespace Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

public class LinksConverter : JsonConverter<IDictionary<string, IList<APIResourceLinkDTO>>>
{
    public override IDictionary<string, IList<APIResourceLinkDTO>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IDictionary<string, IList<APIResourceLinkDTO>> value, JsonSerializerOptions options)
    {
        if (value is null || value.Count == 0)
        {
            writer.WriteNullValue();
            return;
        }

        var linksDictionary = new Dictionary<string, IList<APIResourceLinkDTO>>();

        foreach(var entry in value)
        {
            if (string.IsNullOrWhiteSpace(entry.Key) || entry.Value is null || entry.Value.Count == 0)
            {
                continue;
            }

            var linksList = entry.Value.Where(x =>
                x is not null && !string.IsNullOrWhiteSpace(x.Href)
            ).ToList();

            if (linksList.Count > 0)
            {
                linksDictionary.Add(entry.Key, linksList);
            }
        }

        if (linksDictionary.Count == 0)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        foreach (var entry in linksDictionary)
        {
            writer.WritePropertyName(entry.Key);

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
