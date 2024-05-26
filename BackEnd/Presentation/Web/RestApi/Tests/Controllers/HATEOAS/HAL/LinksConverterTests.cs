using System.Text;
using System.Text.Json;
using Posterr.Presentation.Web.RestApi.Controllers.HATEOAS.HAL;

namespace Posterr.Presentation.Web.RestApi.Tests.Controllers.HATEOAS.HAL;

public class LinksConverterTests {
    private readonly LinksConverter converter;
    private readonly Utf8JsonWriter writer;
    private readonly Stream stream;

    public LinksConverterTests()
    {
        converter = new();
        stream = new MemoryStream();
        writer = new(stream);
    }

    [Fact]
    public void GivenADictionaryWithOneKey_WhenTheValueIsAListWithTwoElements_ThenResultingJSONShouldHaveAnArrayWithTwoElement()
    {
        Dictionary<string, List<APIResourceLinkDTO>> value = [];
        value.Add("key", [
            new("foo"),
            new("bar")
        ]);
        converter.Write(writer, value, new());

        var bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        string json = Encoding.UTF8.GetString(bytes);

        Assert.Equal(true, false);
    }
}
