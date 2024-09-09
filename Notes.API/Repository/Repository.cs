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

        public async Task<PaginatedList<Note>> GetNotes(int pageIndex, int pageSize)
        {
            var notes = await _context.Notes
            .OrderBy(x => x.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            var count = await _context.Notes.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<Note>(notes, pageIndex, totalPages);
        }
    }
}
