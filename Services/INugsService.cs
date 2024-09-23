namespace evantage.Services
{
    public interface INugsService
    {
        Task<List<T>> SearchAirtable<T>(string tablename, int max_records = 3);
        Task<List<T>> GetRecordsFromCSV<T>(string file_path);
    }
}