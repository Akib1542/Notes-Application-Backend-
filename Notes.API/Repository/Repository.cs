using Microsoft.EntityFrameworkCore;
using Notes.API.Data;
using Notes.API.Models.Entities;

namespace Notes.API.Repository
{
    public class Repository : IRepository
    {
        private readonly NotesDbContext _context;

        public Repository(NotesDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Note>> GetNotesAsync()
        {
            var notes =  _context.Notes.AsQueryable();
            return notes;
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(x => x.Id == id);
            return note;
        }

        public async Task<Note> AddNoteAsync(Note note)
        {

            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task UpdateNoteAsync(Note updatedNote, Guid id)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(Note existingNote)
        {
            _context.Remove(existingNote);
            await _context.SaveChangesAsync();
        }
    }
}
