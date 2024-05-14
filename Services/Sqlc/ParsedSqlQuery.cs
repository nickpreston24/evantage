using System.Collections.Generic;
using System.Linq;
using CodeMechanic.Advanced.Regex;

namespace  CodeMechanic.Sqlc;

public class ParsedSqlQuery
{
    /* General */
    public string database { get; set; } = string.Empty;
    public string table_name { get; set; } = string.Empty;
    public string table_alias { get; set; } = string.Empty;
    public string language { get; set; } = string.Empty;

    /* Selects */
    public string where_clause { get; set; } = string.Empty;
    public string join_clause { get; set; } = string.Empty;
    public string select_fields { get; set; } = string.Empty;

    /* Inserts */
    public string insert_clause { get; set; } = string.Empty;

    /* Stored procs */
    public string procedure_name { get; set; } = string.Empty;
    public string raw_fields { get; set; } = string.Empty;
    public string[] procedure_fields => raw_fields?.Split(",")?.Select(raw_value => raw_value.Trim())?.ToArray();
    public List<SprocFieldPart> Fields => ExtractFieldParts();

    private List<SprocFieldPart> ExtractFieldParts()
    {
        return raw_fields.Extract<SprocFieldPart>(
            SqlRegexPattern.GetPattern(SqlRegexPattern.StoredProcedureFieldValues));
    }

    // private ParsedSqlQuery NestedQueries { get; set; } = ParsedSqlQuery.None();
    //
    // public bool IsValidQuery()
    // {
    //     return table_name.IsNullOrEmpty();
    // }
    //
    // private static ParsedSqlQuery None()
    // {
    //     return new();
    // }
}