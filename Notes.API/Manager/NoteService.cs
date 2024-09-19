using Microsoft.EntityFrameworkCore;
using Notes.API.Models;
using Notes.API.Models.Entities;
using Notes.API.Repository;

namespace Notes.API.Manager
{
    public class NoteService : INoteService
    {
        private readonly IRepository _repository;

        public NoteService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<Note>>GetAllNotesAsync(int pageIndex, int pageSize, string? search)
        {
            var totalNotes = await _repository.GetNotesAsync();
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                totalNotes = totalNotes.Where(n =>
                    n.Title.ToLower().Contains(search) ||
                    n.Description.ToLower().Contains(search));
            }

            int dataCount = totalNotes.Count();

            var notes = await totalNotes
              .OrderBy(x => x.Id)
              .Skip((pageIndex - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();

            var totalPages = (int)Math.Ceiling(dataCount / (double)pageSize);
            return new PaginatedList<Note>(notes,pageIndex,pageSize,dataCount);
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            var note = await _repository.GetNoteByIdAsync(id);
            if (note == null)
            {
                return null;
            }
            return note;
        }

        public async Task<Note> AddAsync(Note note)
        {
            note.Id = Guid.NewGuid();

            if (note == null || note.Description == "" || note.Title == "")
            {
                return null;
            }
            var notes = await _repository.AddNoteAsync(note);
            return notes;
        }

        public async Task<Note>UpdateAsync(Note updatedNote, Guid id)
        {
            var existingNote = await _repository.GetNoteByIdAsync(id);
            if (existingNote == null)
            {
                return null;
            }
            existingNote.Title = updatedNote.Title;
            existingNote.Description = updatedNote.Description;
            existingNote.IsVisible = updatedNote.IsVisible;
            await _repository.UpdateNoteAsync(updatedNote,id);
            return existingNote;
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingNote = await _repository.GetNoteByIdAsync(id);
            await _repository.DeleteNoteAsync(existingNote);
        }
    }
}
