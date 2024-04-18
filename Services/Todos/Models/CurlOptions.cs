using System.Text;
using CodeMechanic.Advanced.Regex;

namespace CodeMechanic.Todoist;

public class CurlOptions
{
    // public string curl_name { get; set; } = string.Empty;
    public string bearer_token { get; set; } = string.Empty;

    public string execution_method { get; set; } = string.Empty;

    // public List<string> headers { get; set; }
    public string raw_headers { get; set; } = string.Empty;

    public CurlHeader[] Headers => raw_headers.Extract<CurlHeader>(CurlRegex.Find(CurlRegex.HEADERS)).ToArray();

    public string uri { get; set; } = string.Empty;

    // public static CurlOptions None = new();
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("headers=" + this.raw_headers);
        sb.AppendLine("bt=" + this.bearer_token);
        sb.AppendLine("method=" + this.execution_method);
        return sb.ToString();
    }
}