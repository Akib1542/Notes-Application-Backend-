using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.API.Data;
using Notes.API.Models.Entities;
using Notes.API.Repository;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NotesController : Controller
    {
        private readonly IRepository _repository;

        public NotesController(NotesDbContext notesDbContext, IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllNotesAsync(string? search, int pageIndex = 1, int pageSize = 10, int count = 0)
        {
            var notes = await _repository.GetSearchingNotes(search);
            int dataCount = _repository.CountNotes(search);
            var note = await _repository.GetNotes(pageIndex, pageSize, notes, dataCount);

            return Ok(note);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNoteByIdAsync")]
        public async Task<IActionResult> GetNoteByIdAsync(Guid id)
        {
            var note = await _repository.GetNoteById(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> AddNoteAsync(Note note)
        {
            bool res = await _repository.GetAddNote(note);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(res == false)
            {
                return NotFound("Data Not Found!");
            }
            return CreatedAtAction(nameof(GetNoteByIdAsync), new {id = note.Id}, note);  
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNoteAsync(Guid id, [FromBody] Note updatedNote)
        {
            bool res = await _repository.GetUpdateNotes(updatedNote, id);
            if(res == false)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult>DeleteNoteAsync(Guid id)
        {
            bool res = await _repository.GetDeleteNotes(id);
            if(res == false)
            {
                return NotFound();
            }
            return Ok("Notes Deleted!");
        }
    }
}
