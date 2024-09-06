using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notes.API.Data;
using Notes.API.Models.Entities;
using System.Security.Claims;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly NotesDbContext notesDbContext;

        public NotesController(NotesDbContext notesDbContext)
        {
            this.notesDbContext = notesDbContext;
        }

        [HttpGet]
        public async Task<IActionResult>GetAllNotes()
        {
            //Get the notes from db
            return Ok(await notesDbContext.Notes.ToListAsync());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult>GetNoteById(Guid id)
        {
            var note = await notesDbContext.Notes.FirstOrDefaultAsync(x => x.Id == id);
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
            await notesDbContext.Notes.AddAsync(note);
            await notesDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(AddNote), new {id = note.Id}, note);  
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] Note updatedNote)
        {
            var existingNote = await notesDbContext.Notes.FindAsync(id);
            if(existingNote == null)
            {
                return NotFound();
            }

            existingNote.Title = updatedNote.Title;
            existingNote.Description = updatedNote.Description;
            existingNote.IsVisible = updatedNote.IsVisible;

            await notesDbContext.SaveChangesAsync();

            return Ok(existingNote);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        
        public async Task<IActionResult>DeleteNote(Guid id)
        {
            var existingNote = await notesDbContext.Notes.FindAsync(id);
            if(existingNote == null)
            {
                return NotFound();
            }

            notesDbContext.Notes.Remove(existingNote);
            await notesDbContext.SaveChangesAsync();
            return Ok();
        }

        /*[HttpGet]
        [ActionName("SearchNote")]
        public async Task<IActionResult> SearchNote(string search)
        {
            var data = await notesDbContext.Notes.ToListAsync();
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                data = data.Where(x => x.Description.ToLower().Contains(search)).ToList();
            }
            return Ok(data);
        }*/

        [HttpGet("search")]
        public IActionResult SearchNotes(string query)
        {
            // Convert both the title/description and query to lowercase for case-insensitive search
            var lowerCaseQuery = query.ToLower();

            var notes = notesDbContext.Notes
                .Where(n => n.Title.ToLower().Contains(lowerCaseQuery) ||
                            n.Description.ToLower().Contains(lowerCaseQuery))
                .ToList();

            return Ok(notes);
        }

    }
}
