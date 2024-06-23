using Posterr.Presentation.Web.RestApi.Controllers.SharedModels.HATEOAS.HAL;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Posterr.Presentation.Web.RestApi.Tests.Controllers.SharedModels.HATEOAS.HAL;

public partial class LinksConverterTests
{
    private readonly JsonSerializerOptions options;

    public LinksConverterTests()
    {
        LinksConverter converter = new();
        options = new();
        options.Converters.Add(converter);
    }

    [Fact]
    public void GivenComplexValueToBeSerialized_ThenResultShouldBeTheExpectedJSON()
    {
        IDictionary<string, IList<APIResourceLinkDTO>> value = new Dictionary<string, IList<APIResourceLinkDTO>>()
        {
            // these should be ignored
            [string.Empty] = [new("http://localhost")],
            ["null"] = null!,
            ["empty"] = [],
            ["null_url"] = [new(null!)],
            ["empty_url"] = [new(string.Empty)],
            ["whitespace"] = [new(" ")],

            // these should result in a single link, which should not be inside an array
            ["single_url_1"] = [new("http://localhost")],
            ["single_url_2"] = [new("http://localhost"), null!],
            ["single_url_3"] = [new("http://localhost"), new(null!)],
            ["single_url_4"] = [new("http://localhost"), new(string.Empty)],
            ["single_url_4"] = [new("http://localhost"), new(" ")],

            // this should result in the links being inside an array
            ["two_urls"] = [new("http://localhost"), new("http://test")],
        };

        string actualJSON = JsonSerializer.Serialize(value, options);
        var expectedJSON = NewLinesAndSpaces().Replace("""
            {
                "single_url_1": {
                    "href": "http://localhost"
                },
                "single_url_2": {
                    "href": "http://localhost"
                },
                "single_url_3": {
                    "href": "http://localhost"
                },
                "single_url_4": {
                    "href": "http://localhost"
                },
                "two_urls": [
                    {
                        "href": "http://localhost"
                    },
                    {
                        "href": "http://test"
                    }
                ]
            } 
        """, string.Empty);

        Assert.Equal(expectedJSON, actualJSON);
    }

    [GeneratedRegex(@"[\r\n\t ]")]
    private static partial Regex NewLinesAndSpaces();

    [Fact]
    public void GivenEmptyDictionary_ThenResultShouldBeNullJSONValue()
    {
        IDictionary<string, IList<APIResourceLinkDTO>> value = new Dictionary<string, IList<APIResourceLinkDTO>>();
        string actualJSON = JsonSerializer.Serialize(value, options);
        Assert.Equal("null", actualJSON);
    }
}
