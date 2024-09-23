using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;
using CodeMechanic.Airtable;
using CodeMechanic.Diagnostics;
using CodeMechanic.Reflection;
using CodeMechanic.RegularExpressions;
using CodeMechanic.Types;
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


    public async Task<List<T>> GetRecordsFromCSV<T>(string file_path)
    {
        if (file_path.IsEmpty()) throw new ArgumentNullException(nameof(file_path));
        try
        {
            // using var reader = new StreamReader(file_path);
            // using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            // var records = csv.GetRecords<dynamic>().ToList();
            var records = MyCSVReader<T>(file_path);
            // Console.WriteLine($"{records.Count} records red from {file_path}");

            var results = new List<T>();
            // foreach (dynamic record in records)
            // {
            //     var res = DynamicExtensions.ToStatic<T>(record);
            //     results.Add(res);
            // }

            return records;
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
        var lines = File.ReadAllLines(filePath)
                // Fix the commas for now...
                .Select(line => Regex.Replace(line, @",,", ", ,"))
                .ToArray()
            ;
        // https://regex101.com/r/poh8Es/1
        // (?:\s*(?:\"(?<quoted_content>[^\"]*)\"|(?<content>[^,]+))\s*,?)+?
        string csv_reading_pattern = """
            (?:\s*(?:\"(?<quoted_content>[^\"]*)\"|(?<content>[^,]+|\s*?))\s*,?)+?
        """;
        // string csv_reading_pattern = """
        //     (?:\s*(?:\"([^\"]*)\"|([^,]+))\s*,?)+?
        // """;

        var opts =
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase |
            RegexOptions.IgnorePatternWhitespace;

        var rgx = new Regex(csv_reading_pattern, opts);
        var props = _propertyCache.TryGetProperties<T>().ToArray();

        var line1 = lines[0];
        var headers = line1.Extract<CsvBit>(csv_reading_pattern).ToArray();

        // Console.WriteLine("Line 1 headers: " + line1);
        Console.WriteLine("bits found in headers " + headers.Length);
        // Console.WriteLine("commas : " + Regex.Count(line1, ","));

        var header_names = headers
            .Select(h => h.content
                .NotEmpty()
                ? h.content
                : h.quoted_content)
            .Dump("headers")
            .ToArray();
        Console.WriteLine(" prop ct : " + props.Length);
        // var propnames = props.Select(p => p.Name).Dump("prop names ");

        // var joined_names = propnames.Join(header_names, pn => pn, hn => hn, (s, s1) => s.Equals(s1)).ToList();
        // joined_names.Dump("joined");

        // var bits = new List<CsvBit>(0);
        var records = new List<T>();

        foreach ((var line, int index) in lines.Skip(1).WithIndex())
        {
            var bits = line.Extract<CsvBit>(rgx);

            // int commas = Regex.Count(line, ",");
            // Console.WriteLine($"commas for line {index} " + commas);

            Console.WriteLine($"bits on line {index + 1}: " + bits.Count);
            // var ordinal_prop = props[index];
            // string propname = ordinal_prop.Name;
            // Console.WriteLine(propname);
            // bits.AddRange(extracted_values);


            var instance = Activator.CreateInstance<T>();


            // Console.WriteLine("total header names " + header_names.Length);

            foreach ((var bit, int i) in bits.WithIndex())
            {
                try
                {
                    // Console.WriteLine("index : " + index);
                    // string header = headers.Select(x => x.content).ToArray()[index];
                    string header = header_names[index];
                    Console.WriteLine("header: " + header);
                    string value = bit.content.NotEmpty() ? bit.content : bit.quoted_content ?? string.Empty;
                    Console.WriteLine("bit value: " + value);

                    var prop = props.SingleOrDefault(x => x.Name.Equals(header, StringComparison.OrdinalIgnoreCase));
                    // instance.SetPropertyValue(prop, value);
                    // records.Add(instance);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        // bits.Dump("bits");


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

        return records;
    }
}

public class CsvBit
{
    // public string raw_line { get; set; } = string.Empty;
    public string quoted_content { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
}