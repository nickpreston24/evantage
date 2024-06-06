namespace evantage;

public class CsProjUpdate
{
    public int id { set; get; } = -1;
    public CsProj Project { get; set; } = new();
    public MyGetPackage Package { get; set; } = new();
}