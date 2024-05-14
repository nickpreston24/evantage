using System.Collections.Generic;
using System.Threading.Tasks;
using evantage.Models;

namespace evantage.Services;

public interface INotesService
{
    Task<List<Note>> GetAll();
    Task<List<Note>> Search(Note search);
    Task<Note> GetById(int id);
    Task<int> Create(params Note[] model);
    Task Update(int id, Note model);
    Task Delete(int id);
    Task<int> GetCount();
}