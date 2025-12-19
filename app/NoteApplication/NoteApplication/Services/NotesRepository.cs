using Amazon.DynamoDBv2.DataModel;
using NoteApplication.Models;

namespace NoteApplication.Services
{
    public class NotesRepository : INotesRepository
    {
        private readonly IDynamoDBContext _context;

        public NotesRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<Note?> GetAsync(string id)
        {
            return await _context.LoadAsync<Note>(id);
        }

        public async Task CreateAsync(Note note)
        {
            await _context.SaveAsync(note);
        }

        public async Task UpdateAsync(Note note)
        {
            await _context.SaveAsync(note);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteAsync<Note>(id);
        }
    }
}
