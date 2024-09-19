using Notes.API.Models.Entities;

namespace Notes.API.Repository
{
    public interface IRepository
    {
        Task<IQueryable<Note>> GetNotesAsync();
        Task<Note> GetNoteByIdAsync(Guid id);
        Task<Note> AddNoteAsync(Note note);
        Task UpdateNoteAsync(Note updatedNote, Guid id);
        Task DeleteNoteAsync(Note note);
    }
}
