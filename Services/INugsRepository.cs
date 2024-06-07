using evantage.Models.Csv;

public interface INugsRepository
{
    Task<IEnumerable<Part>> GetAll();
    Task<List<Part>> Search(Part search);
    Task<Part> GetById(int id);
    Task Create(Part model);
    Task Update(int id, Part model);
    Task Delete(int id);
}