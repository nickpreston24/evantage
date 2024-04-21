using System.Reflection;
using System.Text;
using AirtableApiClient;
using CodeMechanic.Diagnostics;
using CodeMechanic.Reflection;
using CodeMechanic.Types;

namespace CodeMechanic.RazorHAT.Services;

public class AirtableSearchV2
{
    private readonly bool debugMode;

    public AirtableSearchV2(
        string baseId
        , string tableName
        , bool debug_mode = false)
    {
        debugMode = debug_mode;
        this.base_id = baseId;
        this.table_name = tableName;
    }

    private static string[] prop_names = { };
    private static PropertyInfo[] props { get; set; } = { };
    public string base_id { get; set; } = string.Empty;
    public string table_name { get; set; } = string.Empty;
    public string offset { get; set; } = string.Empty;
    public List<string> fields { get; set; } = new List<string>();
    public string filterByFormula { get; set; } = string.Empty;
    public int maxRecords { get; set; } = 20;
    public int pageSize { get; set; } = 10;
    public List<Sort> sort { get; set; } = new List<Sort>();
    public string view { get; set; } = string.Empty;
    public string cellFormat { get; set; } = string.Empty;
    public string timeZone { get; set; } = string.Empty;
    public string userLocale { get; set; } = string.Empty;

    public bool returnFieldsByFieldId { get; set; } = true;
    // public bool includeCommentCount { get; set; } = true;

    public void Deconstruct(
        out string table_name,
        out string offset,
        out List<string> fields,
        out string filterByFormula,
        out int maxRecords,
        out int pageSize,
        out List<Sort> sort,
        out string view,
        out string cellFormat,
        out string timeZone,
        out string userLocale,
        out bool returnFieldsByFieldId
    )
    {
        table_name = this?.table_name;
        offset = this?.offset;
        fields = this?.fields;
        filterByFormula = this?.filterByFormula;
        maxRecords = this.maxRecords;
        pageSize = this.pageSize;
        sort = this?.sort;
        view = this?.view;
        cellFormat = this?.cellFormat;
        timeZone = this?.timeZone;
        userLocale = this?.userLocale;
        returnFieldsByFieldId = this.returnFieldsByFieldId;
    }

    // string query = $"https://api.airtable.com/v0/{base_id}/{table_name}?maxRecords={maxRecords}&filterByFormula={filterByFormula}"
    public string AsQuery()
    {

        if (props?.Length == 0)
            props = typeof(AirtableSearchV2).GetProperties();

        if (prop_names?.Length == 0)
            prop_names = props?.Select(prop => prop.Name.Trim()).ToArray();

        if (table_name.IsEmpty())
            throw new ArgumentNullException(nameof(table_name));

        if (base_id.IsEmpty())
            throw new ArgumentNullException(nameof(base_id));

        var prop_values = props.ToPropertyValueDictionary(this);
        if (debugMode) prop_values.Dump("All search values");

        var blacklist = new[] { nameof(table_name) };

        string query =
            new StringBuilder($"https://api.airtable.com/v0/{base_id}/{table_name}?")
                .AppendEach(
                    prop_names.Except(blacklist)
                    , name =>
                    {
                        string line =
                                name == nameof(base_id)
                                    ? name + "=" + prop_values[name].Trim()
                                    : "&" + name + "=" + prop_values[name].Trim()
                            ;
                        return line;
                    }, delimiter: "")
                .ToString()
                .Trim();

        if (debugMode) query.Dump("generated query");

        return query;
    }
}