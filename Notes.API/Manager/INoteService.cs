using Notes.API.Models;
using Notes.API.Models.Entities;

namespace Notes.API.Manager
{
    public interface INoteService
    {
        Task<Note> AddAsync(Note note);
        Task DeleteAsync(Guid id);
        Task<PaginatedList<Note>> GetAllNotesAsync(int pageIndex, int pageSize, string? search);
        Task<Note> GetNoteByIdAsync(Guid id);
        Task<Note> UpdateAsync(Note note, Guid id);
    }
}
