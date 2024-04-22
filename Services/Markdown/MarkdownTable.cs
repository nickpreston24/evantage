namespace  evantage.Services;

public class MarkdownTable
{
    public static string table_pattern = """
        # TODO: Untested
        /((\r?\n){2}|^)([^\r\n]*\|[^\r\n]*(\r?\n)?)+(?=(\r?\n){2}|$)/  # https://regex101.com/r/8pNnaG/1/codegen?language=csharp
    """;
}