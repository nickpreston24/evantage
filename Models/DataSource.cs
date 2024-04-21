using CodeMechanic.Types;

namespace evantage.Models;

public class DataSource : Enumeration
{
    public static DataSource Airtable = new DataSource(1, nameof(Airtable));
    public static DataSource Neo4j = new DataSource(2, nameof(Neo4j));
    public static DataSource Mysql = new DataSource(3, nameof(Mysql));

    public DataSource(int id, string name) : base(id, name)
    {
    }
}