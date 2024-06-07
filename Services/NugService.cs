using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using CodeMechanic.Airtable;
using CodeMechanic.Diagnostics;
using CodeMechanic.Reflection;
using CodeMechanic.RegularExpressions;
using CodeMechanic.Types;
using CsvHelper;
using evantage.Models.Csv;
using evantage.Pages.Sandbox.Airtable;
// using evantage.Pages.Sandbox.Airtable;
using AirtableSearchV2 = CodeMechanic.Airtable.AirtableSearchV2;

namespace evantage.Services;

public class NugsService : INugsService
{
    private IAirtableServiceV2 airtable;

    /// <summary>
    /// PropertyCache stores the properties we wish to use again so we only have to run Reflection once per property.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, ICollection<PropertyInfo>> _propertyCache =
        new ConcurrentDictionary<Type, ICollection<PropertyInfo>>();

    public NugsService(IAirtableServiceV2 airtable)
    {
        this.airtable = airtable;
    }

    public async Task<List<T>> SearchAirtable<T>(string tablename, int max_records = 3)
    {
        string baseid = Environment.GetEnvironmentVariable("NUGS_BASE_KEY");
        string pat = Environment.GetEnvironmentVariable("NUGS_PAT");
        var search = new AirtableSearchV2(pat, baseid, tablename, max_records, true);
        var results = await airtable.SearchRecords<T>(search);
        return results;
    }


    public async Task<List<T>> GetRecordsFromCSV<T>()
    {
        try
        {
            Console.WriteLine(nameof(GetRecordsFromCSV));
            string file_path = @"/home/nick/Downloads/Parts-Grid view.csv";
            var records = MyCSVReader<T>(file_path);
            // using var reader = new StreamReader(file_path);
            // using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            // var records = csv.GetRecords<T>().ToList();
            Console.WriteLine($"{records.Count} records red from {file_path}");
            return records;

            // var anonymousTypeDefinition = new T()
            // {
            //     // Id = default(int),
            //     // Name = string.Empty
            // };
            // var records = csv.GetRecords(anonymousTypeDefinition);

            // foreach (dynamic record in records)
            // {
            //     Console.WriteLine(record);
            // }

            // foreach (var record in records)
            // {
            //     Console.WriteLine("value : " + record);
            // }
            // records.First().Dump(); // dynamic cannot dump

            // Part part = DynamicExtensions.MapTo<Part>(records.FirstOrDefault());

            // part?.Dump("My part");
            // List<Part> parts = records.Select(dyn => DynamicExtensions.MapTo<Part>(dyn)).ToList();


            // return new List<T>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    /// <summary>
    /// CSVReader's just not doing it for me.... complains if there's even ONE property mismatch (count)!
    /// </summary>
    /// <param name="filePath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private List<T> MyCSVReader<T>(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        // https://regex101.com/r/eH1zP0/1
        string csv_reading_pattern = @"(?:\s*(?:""(?<quoted_content>[^""]*)""|(?<content>[^,]+))\s*,?)+?";

        var opts =
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase |
            RegexOptions.IgnorePatternWhitespace;

        var rgx = new Regex(csv_reading_pattern, opts);

        // List<CsvBit> bits = new List<CsvBit>();

        var props = _propertyCache.TryGetProperties<T>().ToArray();

        Console.WriteLine(props.Length);
        // var records = new List<T>();
        var headers = lines[0].Extract<CsvBit>(csv_reading_pattern).ToArray();

        Console.WriteLine("Line 1: " + lines[0]);
        Console.WriteLine("bits found in headers " + headers.Length);

        var header_names = headers.Select(h => h.content.NotEmpty() ? h.content : h.quoted_content).Dump("headers");
        Console.WriteLine(" prop ct : " + props.Length);
        var propnames = props.Select(p => p.Name).Dump("prop names ");

        // var joined_names = propnames.Join(header_names, pn => pn, hn => hn, (s, s1) => s.Equals(s1)).ToList();
        // joined_names.Dump("joined");

        var bits = new List<CsvBit>(0);
        foreach ((var line, int index) in lines.Skip(1).WithIndex())
        {
            var extracted_values = line.Extract<CsvBit>(rgx);
            
            // Console.WriteLine($"bits on line {index + 1}: " + extracted_values.Count);
            // var ordinal_prop = props[index];
            // string propname = ordinal_prop.Name;
            // Console.WriteLine(propname);
            bits.AddRange(extracted_values);
        }

        bits.Take(3).Dump("bits");
        // string line1 = lines[1];

        // var match = rgx.Match(line1);
        // var groups = match.Groups;
        // Console.WriteLine($"groups {groups.Count}");
        //
        // foreach (Group group in groups)
        // {
        //     Console.WriteLine($"{group.Name}: {group.Value}");
        // }


        // var bits = raw_sample.Extract<CsvBit>();
        // bits.Take(2).Dump("csv bits");
        // Console.WriteLine($"bits found : {bits.Count}");


        // foreach (var bit in bits)
        // {
        //     // string name = props.SingleOrDefault;
        // }

        return new List<T>();
    }

    private static void SetPropertyValue(PropertyInfo prop, object entity, object value)
    {
        if (value == null)
        {
            prop.SetValue(entity, null, null);
            return;
        }

        if (prop.PropertyType == typeof(string))
        {
            prop.SetValue(entity, value.ToString().Trim(), null);
        }
        else if (prop.PropertyType == typeof(char))
        {
            prop.SetValue(entity, value.ToString()[0], null);
        }
        else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
        {
            prop.SetValue(entity, ParseBoolean(value.ToString()), null);
        }
        else if (prop.PropertyType == typeof(long))
        {
            prop.SetValue(entity, long.Parse(value.ToString()), null);
        }
        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
        {
            prop.SetValue(entity, int.Parse(value.ToString()), null);
        }
        else if (prop.PropertyType == typeof(decimal))
        {
            prop.SetValue(entity, decimal.Parse(value.ToString()), null);
        }
        else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
        {
            double number;
            bool isValid = double.TryParse(value.ToString(), out number);
            if (isValid)
            {
                prop.SetValue(entity, double.Parse(value.ToString()), null);
            }
        }
        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
        {
            DateTime date;
            bool isValid = DateTime.TryParse(value.ToString(), out date);
            if (isValid)
            {
                prop.SetValue(entity, date, null);
            }
            else
            {
                isValid = DateTime.TryParseExact(value.ToString(), "MMddyyyy", new CultureInfo("en-US"),
                    DateTimeStyles.AssumeLocal, out date);
                if (isValid)
                {
                    prop.SetValue(entity, date, null);
                }
            }
        }
        else if (prop.PropertyType == typeof(Guid))
        {
            Guid guid;
            bool isValid = Guid.TryParse(value.ToString(), out guid);
            if (isValid)
            {
                prop.SetValue(entity, guid, null);
            }
            else
            {
                isValid = Guid.TryParseExact(value.ToString(), "B", out guid);
                if (isValid)
                {
                    prop.SetValue(entity, guid, null);
                }
            }
        }
    }

    public static bool ParseBoolean(object value)
    {
        if (value == null || value == DBNull.Value) return false;

        switch (value.ToString().ToLowerInvariant())
        {
            case "1":
            case "y":
            case "yes":
            case "true":
                return true;

            case "0":
            case "n":
            case "no":
            case "false":
            default:
                return false;
        }
    }
}

public class CsvBit
{
    public string raw_line { get; set; } = string.Empty;
    public string quoted_content { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
}

//
// public static class DynamicExtensions
// {
//     /// <summary>
//     /// PropertyCache stores the properties we wish to use again so we only have to run Reflection once per property.
//     /// </summary>
//     private static readonly ConcurrentDictionary<Type, ICollection<PropertyInfo>> _propertyCache =
//         new ConcurrentDictionary<Type, ICollection<PropertyInfo>>();
//
//     public static T MapTo<T>(
//         dynamic record
//         , List<PropertyInfo> props = null
//     )
//         where T : class, new()
//     {
//         var type = typeof(T);
//
//         // if no props passed as an override, get them from the cache
//         var properties = props.Count > 0
//             ? props
//             : _propertyCache.TryGetProperties<T>(true);
//
//
//         if (properties.Count == 0) return new T();
//
//         var obj = new T();
//
//         foreach (var prop in properties ?? Enumerable.Empty<PropertyInfo>())
//         {
//             string name = prop.Name /*.Dump("key")*/;
//             // var value = prop.GetValue(obj);
//             var value = record[name];
//
//             var next_value = CreateSafeValue(value, prop);
//
//             prop.SetValue(obj, next_value /*.Dump("value")*/, null);
//         }
//
//         return obj;
//     }
//
//     private static object CreateSafeValue(object value, PropertyInfo prop)
//     {
//         Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
//
//         object safeValue =
//             value == null
//                 ? null
//                 : Convert.ChangeType(value, propType);
//
//         return safeValue;
//     }
// }

// var bits = lines.Select(l => l.Extract<CsvBit>()).ToList();
// string raw_sample = """
//         
//         Name,Cost,Attachments,URL,Calibers,WeightInOz,Demo,Type,Kind,Notes,Combo,ComboCost,Builds,Requests,Donations,CreatedBy,Created,LastModifiedBy,LastModified,ProductCode,Id,Roles,Delimiter
//         80% Arms Billet Lower (Blemished),$79.00,image.png (https://v5.airtableusercontent.com/v2/23/23/1700503200000/u2po4Mvfs5IyQ_ZW_yiuGQ/wIX652rM_yhv6_A3N1yuNTIbtvknqaewNCLPQjEmC7oCzS3Z1vHqVNxdvj7VKSLsfwjdvu81S7zi2eP1AntBHLUPojB5krxOAlLPmTS8a6gh0RD3Ii_9XB6k5ofHsfxcb59U0keWAX77ri_04_RCvA/7ueSd3j93G78pKeXaJuMSs7NUYDg_aLvQ73CieZrlJg),https://www.80percentarms.com/products/type-iii-hard-anodized-billet-ar-15-80-lower-receiver-blemished/,Any,7.50,,Lower,,"Reccommended by Pew Pew Tactical as their #6, I believe this one could aircraft grade aluminum.",,$0.00,"Natasha,Banshee / Sirius Blackout,Spectre / Spectre Warlock ,Zev Striker,SPECTRE / Warlock (Lower Only),Cruise Cannon,The Minuteman,Nuremburg Needler (Ti),Nuremburg Needler (Budget),Sasha,Kitschy Kestrel,Budget Blaster,Economy Equine,Filly Folder,Kitschy Kestrel (DIY),Rangy RECCE,Prancing Poltergeist,Flyweight Filly (DIY),M-4 Replacement (Budget),M5/M249 Replacement (Budget),M-4 GRN Replacement (Budget),ARAK-Nid,Minuteman - SALE","11,24,31,33,27,9,47","29,45,63,91",Nicholas Preston,6/16/2021 8:09pm,Nicholas Preston,11/20/2023 8:14am,,87,,;!
// """;