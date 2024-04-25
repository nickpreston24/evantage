namespace CodeMechanic.Airtable;

public interface IAirtableServiceV2
{
    Task<List<T>> SearchRecords<T>(AirtableSearchV2 search, bool debug = false);
}