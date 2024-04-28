using System.ComponentModel;
using System.Reflection;
using System.Text;
using AirtableApiClient;
using CodeMechanic.Diagnostics;
using CodeMechanic.Types;

namespace CodeMechanic.Airtable;

public class AirtableSearchV2
{
    private readonly bool debugMode;

    public AirtableSearchV2(
        string baseId
        , string tableName
        , int maxRecords = 100
        , bool debug_mode = false
    )
    {
        debugMode = debug_mode;
        this.base_id = baseId;
        this.table_name = tableName;
        this.maxRecords = maxRecords;
    }

    // private static string[] prop_names = { };
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
        sort = this?.sort;

        cellFormat = this?.cellFormat;
        timeZone = this?.timeZone;
        userLocale = this?.userLocale;
        returnFieldsByFieldId = this.returnFieldsByFieldId;
    }

    // string query = $"https://api.airtable.com/v0/{base_id}/{table_name}?maxRecords={maxRecords}&filterByFormula={filterByFormula}"
    public string AsQuery()
    {
        // if (props?.Length == 0)
        //     props = typeof(AirtableSearchV2).GetProperties();
        //
        // if (prop_names?.Length == 0)
        //     prop_names = props?.Select(prop => prop.Name.Trim()).ToArray();
        //
        // if (table_name.IsEmpty())
        //     throw new ArgumentNullException(nameof(table_name));
        //
        // if (base_id.IsEmpty())
        //     throw new ArgumentNullException(nameof(base_id));

        // var prop_values = props.ToPropertyValueDictionary(this);
        // var prop_values = props.ToDictionary(x => x.Name);
        // if (debugMode) prop_values.Dump("All search values");

        var prop_values = this.ToDictionary();
        var props = typeof(AirtableSearchV2).GetProperties();
        string[] prop_names = props.Select(p => p.Name).ToArray();
        var blacklist = new[] { nameof(table_name), "sort", "fields" };

        string query =
            new StringBuilder($"https://api.airtable.com/v0/{base_id}/{table_name}?")
                .AppendEach(
                    prop_names.Except(blacklist)
                    , name =>
                    {
                        string line =
                                name == nameof(base_id)
                                    ? name + "=" + prop_values[name].ToString().Trim()
                                    : "&" + name + "=" + prop_values[name].ToString().Trim()
                            ;
                        // Console.WriteLine("query line :>> " + line);
                        return line;
                    }, delimiter: "")
                .ToString()
                .Trim();

        if (debugMode) query.Dump("generated query");

        return query;
    }
}

public static class ObjectToDictionaryHelper
{
    public static IDictionary<string, object> ToDictionary(this object source)
    {
        return source.ToDictionary<object>();
    }

    public static IDictionary<string, T> ToDictionary<T>(this object source)
    {
        if (source == null)
            ThrowExceptionWhenSourceArgumentIsNull();

        var dictionary = new Dictionary<string, T>();
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            AddPropertyToDictionary<T>(property, source, dictionary);

        return dictionary;
    }

    private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source,
        Dictionary<string, T> dictionary)
    {
        object value = property.GetValue(source);
        if (IsOfType<T>(value))
            dictionary.Add(property.Name, (T)value);
    }

    private static bool IsOfType<T>(object value)
    {
        return value is T;
    }

    private static void ThrowExceptionWhenSourceArgumentIsNull()
    {
        throw new ArgumentNullException("source",
            "Unable to convert object to a dictionary. The source object is null.");
    }
}


/// <summary>
///  todo : use this to get shapes of data if you have to:
///  https://airtable.com/api/meta
///  :'(
/// </summary>
public class AirtableFieldsCollection
{
    /**
     * 
     */
}