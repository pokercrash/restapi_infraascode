using Microsoft.AspNetCore.Mvc;
using NoteApplication.Models;
using NoteApplication.Services;

namespace NoteApplication.Controllers
{
    [ApiController]
    [Route("notes")]
    public class NoteController : ControllerBase
    {
        private readonly INotesRepository _repository;
        private readonly ILogger<NoteController> _logger;

        public NoteController(INotesRepository repository, ILogger<NoteController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var note = await _repository.GetAsync(id);
            if (note == null)
                return NotFound();

            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Note note)
        {
            await _repository.CreateAsync(note);
            _logger.LogInformation("Created note {Id}", note.Id);

            return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Note note)
        {
            note.Id = id;
            await _repository.UpdateAsync(note);
            _logger.LogInformation("Updated note {Id}", id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Deleted note {Id}", id);

            return Ok();
        }
    }
}
