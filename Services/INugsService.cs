using evantage.Models.Csv;

namespace evantage.Services
{
    public interface INugsService
    {
        // public Task<List<Part>> GetRecordsFromCSV();
        Task<List<T>> SearchAirtable<T>(string tablename, int max_records = 3);
        Task<List<T>> GetRecordsFromCSV<T>();
    }
}