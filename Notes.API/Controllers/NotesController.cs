using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notes.API.Data;
using Notes.API.Models.Entities;
using Notes.API.Repository;
using System.Security.Claims;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly NotesDbContext _notesDbContext;
        private readonly IRepository _repository;

        public NotesController(NotesDbContext notesDbContext, IRepository repository)
        {
            _notesDbContext = notesDbContext;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult>GetAllNotes()
        {
            //Get the notes from db
            return Ok(await _notesDbContext.Notes.ToListAsync());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult>GetNoteById(Guid id)
        {
            var note = await _notesDbContext.Notes.FirstOrDefaultAsync(x => x.Id == id);
            if(note == null)
            {
                return NotFound();
            }
            return Ok(note); 
        }

        [HttpPost]
        public async Task<IActionResult>AddNote(Note note)
        {
            note.Id = Guid.NewGuid();
            await _notesDbContext.Notes.AddAsync(note);
            await _notesDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(AddNote), new {id = note.Id}, note);  
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] Note updatedNote)
        {
            var existingNote = await _notesDbContext.Notes.FindAsync(id);
            if(existingNote == null)
            {
                return NotFound();
            }

            existingNote.Title = updatedNote.Title;
            existingNote.Description = updatedNote.Description;
            existingNote.IsVisible = updatedNote.IsVisible;

            await _notesDbContext.SaveChangesAsync();

            return Ok(existingNote);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        
        public async Task<IActionResult>DeleteNote(Guid id)
        {
            var existingNote = await _notesDbContext.Notes.FindAsync(id);
            if(existingNote == null)
            {
                return NotFound();
            }

            _notesDbContext.Notes.Remove(existingNote);
            await _notesDbContext.SaveChangesAsync();
            return Ok();
        }

   
        [HttpGet("search")]
        public IActionResult SearchNotes(string query)
        {
            // Convert both the title/description and query to lowercase for case-insensitive search
            var lowerCaseQuery = query.ToLower();

            var notes = _notesDbContext.Notes
                .Where(n => n.Title.ToLower().Contains(lowerCaseQuery) ||
                            n.Description.ToLower().Contains(lowerCaseQuery))
                .ToList();

            return Ok(notes);
        }

        [HttpGet("GetPaginatedNotes")]
        public async Task<ActionResult<object>> GetNotes(int pageIndex = 1, int pageSize = 10)
        {
            var notes = await _repository.GetNotes(pageIndex, pageSize);
            return Ok(notes);
        }

    }
}
