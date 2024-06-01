using System.Text.RegularExpressions;
using CodeMechanic.Types;

namespace CodeMechanic.Sqlc;

public class SqlRegexPattern : Enumeration
{
    public static readonly IDictionary<SqlRegexPattern, System.Text.RegularExpressions.Regex> compiled_patterns =
        new Dictionary<SqlRegexPattern, System.Text.RegularExpressions.Regex>();

    public System.Text.RegularExpressions.Regex CompiledPattern { get; }

    public static System.Text.RegularExpressions.Regex GetPattern(SqlRegexPattern pattern)
    {
        bool is_found = compiled_patterns.TryGetValue(pattern, out var found);
        return is_found ? found : throw new Exception("Pattern with name '" + pattern.Name + "' could not be found!");
    }

    public SqlRegexPattern(int id, string name, string pattern, RegexOptions options = RegexOptions.None) : base(id,
        name)
    {
        CompiledPattern =
            new System.Text.RegularExpressions.Regex(pattern,
                RegexOptions.Compiled | RegexOptions.IgnoreCase | options);
        compiled_patterns.TryAdd(this, CompiledPattern);
    }

    public static SqlRegexPattern EmptyLine =
        new SqlRegexPattern(1
            , nameof(EmptyLine)
            , @"^\s*$"
            , RegexOptions.IgnorePatternWhitespace);

    // https://regex101.com/r/gNPxsc/1
    public static SqlRegexPattern InsertInto =
        new SqlRegexPattern(2
            , nameof(InsertInto)
            , @"(INSERT\s*INTO)\s*(?<table_name>\w+)\s*(?<column_names>\((\w+,?(\s*))+\))");

    // https://regex101.com/r/MwmSqk/1
    public static SqlRegexPattern CreateProcedure =
        new SqlRegexPattern(3, nameof(CreateProcedure), @"create.*procedure\s*(?<procedure_name>\w+)\(");

    //https://regex101.com/r/6aEMHe/1
    public static SqlRegexPattern AlterTable =
        new SqlRegexPattern(4, nameof(AlterTable), @"alter\s*table\s*(?<table_name>\w+)");

    //https://regex101.com/r/mroIgT/1
    public static SqlRegexPattern SimpleSelect = new SqlRegexPattern(5
        , nameof(SimpleSelect)
        , @"select\s*(distinct|count)?\s.*\nfrom\s*(?<table_name>\w+)\s*;");

    //https://regex101.com/r/XQdY06/1
    public static SqlRegexPattern SelectWithFields = new SqlRegexPattern(6
        , nameof(SelectWithFields)
        , @"select\s*(?<select_fields>(.*(as)?\s*\w+,?\n)*)from\s*(?<table_name>\w+)\s*;");

    //https://regex101.com/r/Xr0jew/1
    public static SqlRegexPattern StoredProcedureFields = new SqlRegexPattern(7,
        nameof(StoredProcedureFields),
        @"create\s*(or\s*replace)?\s*procedure\s(?<procedure_name>\w+)\s*\((?<raw_fields>(\s+(?<field_name>\w+)\s*(?<field_type>[a-zA-z\d()]+)(\s*=\s*(?<field_default_value>['a-zA-z\d]+),?))*\s*)\)");

    // https://regex101.com/r/hVGH7H/1
    public static SqlRegexPattern StoredProcedureFieldValues = new SqlRegexPattern(8,
        nameof(StoredProcedureFieldValues),
        @"(?<field_name>\w+)\s*(?<field_type>[\w)(]+)(\s*=\s*)?(?<field_value>[\w\d'_]+)");

    // https://regex101.com/r/6xlYEo/1
    public static SqlRegexPattern StoredProcedureFull = new SqlRegexPattern(9, nameof(StoredProcedureFull),
        @"create\s*(or\s*replace)?\s*procedure\s(?<procedure_name>\w+)\s*\((?<raw_fields>(\s+(\w+)\s*([a-zA-z\d()]+)(\s*=\s*(['a-zA-z\d]+),?))*\s*)\)(?<language>\s*language\s*plpgsql)\s*(as)?\s*(\$\$)?\s*begin\s*select\s*(?<selected_fields>[a-zA-Z*_,\s]+)\s*from\s*(?<database>\w+\.)?(?<table_name>\w+)\s*(?<table_alias>\w+)\s*(?<where_clause>where)");
}