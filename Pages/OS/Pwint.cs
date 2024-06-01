namespace evantage.Models;

public class Pwint
{
    public Pwint(string path) => file_path = path;
    public string file_path { get; set; } = string.Empty;
    public string file_name => System.IO.Path.GetFileName(file_path);

    public override string ToString() => file_path;
}