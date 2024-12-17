using DataAccessLayer.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.NewFolder
{
    public interface INotesRepository
    {
        Task AddNotes(Notes notes);
        public Task<Notes?> GetNoteByUserAndMovieAsync(string userId, string movieId);
        public  Task UpdateNoteAsync(Notes note);
        public Task<bool> UserExistsCheck(string Userid);
        public Task<bool> MovieExistsCheck(string MovieId);
        public Task<bool> NoteExistsCheck(string Userid,string MovieId);
        Task DeleteNotes(string Userid,string movieId);
        Task<Notes> GetAllNotesForUserandMovie(string UserId,string MovieId);
        Task<IEnumerable<Notes>> GetAllNotesByUser(string UserId, int page, int pagesize);
        Task<IEnumerable<title_basics>> GetAllNotesByMovie(string MovieId, int page, int pagesize);
        Task<int> CountNotesByUser(string UserId);
        Task<int> CountNotesByMovie(string UserId);
    }
}
