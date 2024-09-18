using Microsoft.EntityFrameworkCore;
using Notes.API.Data;
using Notes.API.Models;
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

        public async Task<PaginatedList<Note>> GetNotesAsync(int pageIndex, int pageSize, IQueryable<Note> afterSearch, int dataCount)
        {
            var notes = await afterSearch
            .OrderBy(x => x.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            var count = dataCount;
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<Note>(notes, pageIndex, totalPages, count);
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(x => x.Id == id);
            if (note == null)
            {
                return null;
            }
            return note;
        }

        public async Task<bool> AddNoteAsync(Note note)
        {
            note.Id = Guid.NewGuid();

            if (note == null || note.Description == "" || note.Title == "")
            {
                return false;
            }
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNoteAsync(Note updatedNote, Guid id)
        {
            var existingNote = await _context.Notes.FindAsync(id);

            if (existingNote == null)
            {
                return false;
            }
            existingNote.Title = updatedNote.Title;
            existingNote.Description = updatedNote.Description;
            existingNote.IsVisible = updatedNote.IsVisible;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            var existingNote = await _context.Notes.FindAsync(id);
            if (existingNote == null)
            {
                return false;
            }
            _context.Remove(existingNote);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IQueryable<Note>> SearchNotesAsync(string? search)
        {
            var notes = _context.Notes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                notes = notes.Where(n =>
                    n.Title.ToLower().Contains(search) ||
                    n.Description.ToLower().Contains(search));
            }
            int count = notes.Count();
            return notes;
        }

        public int CountNotes(string? search)
        {
            var notes = _context.Notes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                notes = notes.Where(n =>
                    n.Title.ToLower().Contains(search) ||
                    n.Description.ToLower().Contains(search));
            }
            int count = notes.Count();
            return count;
        }  
    }
}
