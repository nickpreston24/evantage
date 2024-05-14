using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMechanic.FileSystem;
using CodeMechanic.Types;

namespace CodeMechanic.Sqlc;

public class SQLCService : IGenerateSQLTypes
{
    private static Dictionary<string, string> patterns;
    public string OutFolderPath { get; set; } = string.Empty;

    public async Task<ProposedScript> ScriptTypeAs<T>(ScriptOptions scriptOptions)
    {
        var props = typeof(T).GetProperties();

        var result = new ProposedScript()
        {
            Options = scriptOptions
        };

        // Return a proposed table object that can be ToString()'d
        // or edited before it's ToString()'d (using with() and a custom object for edits

        return result;
    }

    public List<ParsedSqlQuery> FindAllProcs(string directory_path)
    {
        throw new NotImplementedException("Not finished");
        var grepper = new Grepper()
        {
            RootPath = directory_path,
            Recursive = true,
        };

        var matching_files = grepper.GetFileNames();
        var matching_line_files = grepper.GetMatchingFiles();

        return default;
    }

    public List<SqlParameter> ParseParametersFromSQL(string sql)
    {
        throw new NotImplementedException();
    }

    public List<SqlParameter> ParseTypesFromSQL(string sql)
    {
        throw new NotImplementedException();
    }

    public bool ValidateSQL()
    {
        throw new NotImplementedException();
    }

    public SQLCService GenerateTypes(
        List<ParsedSqlQuery> queries
        , Action<string> printFn = null
    )
    {
        string extension = ".cs";
        foreach (var query in queries.Where(q => q.Fields.Count > 0))
        {
            string file_name = !query.database.IsNullOrEmpty()
                ? query.database + "." + query.table_name
                : query.table_name;

            string file_path = Path.Combine(OutFolderPath, file_name + extension);
            printFn(file_path);

            var sb = new StringBuilder()
                .AppendLine($"{CsharpPart.Public.Text} class {file_name}")
                .AppendLine(@"{")
                .AppendEach(query.Fields,
                    field => "\t" + ToPropertyType(field.field_type) + " " + field.field_name + " { get; set; } " +
                             GetSafeDefaultValue(field.field_value),
                    delimiter: "\n")
                .AppendLine()
                .AppendLine(@"}");
            string text = sb.ToString();
            printFn(text);
            File.WriteAllText(file_path, text);
        }

        return this;
    }

    private string GetSafeDefaultValue(string value)
    {
        if (value.Contains("''"))
            return "= string.Empty;";
        return string.Empty;
    }

    private string ToPropertyType(string fieldFieldType)
    {
        var is_varchar = fieldFieldType.ToLower().Contains("varchar");
        if (is_varchar) return "string";

        switch (fieldFieldType)
        {
            case "int":
            case "float":
                return fieldFieldType; // todo: add more!
            default:
                return "string";
        }
    }
}

public class SqlScriptFlavor : Enumeration
{
    public static SqlScriptFlavor Sqlite = new SqlScriptFlavor(1, nameof(Sqlite));
    public static SqlScriptFlavor MySql = new SqlScriptFlavor(2, nameof(MySql));

    public SqlScriptFlavor(int id, string name) : base(id, name)
    {
    }
}

public class SqlScriptType : Enumeration
{
    public static SqlScriptType Insert = new SqlScriptType(1, nameof(Insert), prefix: @"Insert Into");
    public static SqlScriptType BulkUpsert = new SqlScriptType(2, nameof(BulkUpsert), prefix: @"Insert Into");

    public static SqlScriptType StoredProcedure =
        new SqlScriptType(3, nameof(StoredProcedure), prefix: @"create procedure");

    public SqlScriptType(int id, string name, string prefix) : base(id, name)
    {
        Prefix = prefix;
    }

    public string Prefix { get; set; }
}

public class ProposedScript
{
    public ScriptOptions Options { get; set; }

    public override string ToString()
    {
        var op = Options.script_type;
        var prefix = op.Prefix;

        var start_line = $"{prefix} {Options.table_name}";

        return $"select * from {Options.table_name}";
    }
}

public class ScriptOptions
{
    public SqlScriptFlavor flavor { get; set; } = SqlScriptFlavor.MySql;

    public SqlScriptType script_type { get; set; } = SqlScriptType.Insert;

    // public string SavePath { get; set; } = "./";
    public string table_name = string.Empty;
}