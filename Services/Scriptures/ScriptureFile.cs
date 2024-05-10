using System.Net.Mime;
using System.Text;

namespace evantage.Models;

public class ScriptureFile
{
    public string file_name { get; set; } = string.Empty;

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(file_name)}: {file_name}")
            .ToString();
    }
}