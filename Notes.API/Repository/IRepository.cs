using Notes.API.Models;
using Notes.API.Models.Entities;

namespace Notes.API.Repository
{
    public interface IRepository
    {
        Task<PaginatedList<Note>>GetNotes (int pageIndex, int pageSize);    
    }
}
