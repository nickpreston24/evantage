using evantage.Models.Csv;


public interface IPartsRepository
{
    Task<IEnumerable<Part>> GetAll();
    Task<List<Part>> Search(Part search);
    Task<Part> GetById(int id);
    Task<int> Create(Part model);
    Task Update(int id, Part model);
    Task Delete(int id);
}