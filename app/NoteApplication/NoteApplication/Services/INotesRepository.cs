using NoteApplication.Models;

namespace NoteApplication.Services
{
    public interface INotesRepository
    {
        Task<Note?> GetAsync(string id);
        Task CreateAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(string id);
    }
}
