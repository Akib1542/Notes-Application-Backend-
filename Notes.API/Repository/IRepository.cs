using Notes.API.Models;
using Notes.API.Models.Entities;

namespace Notes.API.Repository
{
    public interface IRepository
    {
        Task<PaginatedList<Note>> GetNotesAsync(int pageIndex, int pageSize, IQueryable<Note> afterSearch, int cou);
        Task<Note> GetNoteByIdAsync(Guid id);
        Task<bool> AddNoteAsync(Note note);
        Task<bool> UpdateNoteAsync(Note updatedNote, Guid id);
        Task<bool> DeleteNoteAsync(Guid id);
        Task<IQueryable<Note>> SearchNotesAsync(string? search);
        int CountNotes(string? search);
    }
}
