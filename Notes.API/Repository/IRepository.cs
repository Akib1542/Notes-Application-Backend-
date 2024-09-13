using Notes.API.Models;
using Notes.API.Models.Entities;

namespace Notes.API.Repository
{
    public interface IRepository
    {
        int CountNotes(string? search);
        Task<bool> GetAddNote(Note note);
        Task<bool> GetDeleteNotes(Guid id);
        Task<PaginatedList<Note>>GetNotes (int pageIndex, int pageSize, IQueryable<Note> afterSearch, int cou);
        Task<IQueryable<Note>> GetSearchingNotes(string? search);
        Task<Note> GetNoteById(Guid id);
        Task<bool> GetUpdateNotes(Note updatedNote, Guid id);
    }
}
