using Microsoft.AspNetCore.Mvc;
using Notes.API.Manager;
using Notes.API.Models.Entities;
using Notes.API.Repository;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NotesController : Controller
    {
        private readonly INoteService _noteService;

        public NotesController(IRepository repository, INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllNotesAsync(string? search, int pageIndex = 1, int pageSize = 10, int count = 0)
        {
            var notes = await _noteService.GetAllNotesAsync(pageIndex, pageSize, search);
            return Ok(notes);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNoteByIdAsync")]
        public async Task<IActionResult> GetNoteByIdAsync(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> AddNoteAsync(Note note)
        {
            var noteAdded = await _noteService.AddAsync(note);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(GetNoteByIdAsync), new {id = noteAdded.Id}, noteAdded);  
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNoteAsync(Guid id, [FromBody] Note updatedNote)
        {
            var note = await _noteService.UpdateAsync(updatedNote, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult>DeleteNoteAsync(Guid id)
        {
            await _noteService.DeleteAsync(id);
            return Ok();
        }
    }
}
