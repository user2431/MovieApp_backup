using DataAccessLayer.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.NewFolder
{
    
    public class NotesRepository : INotesRepository
    {
        private readonly MovieDBContext _dBContext;
        public NotesRepository(MovieDBContext dBContext)
        {
            _dBContext = dBContext;
            
        }

        public async Task AddNotes(Notes notes)
        {
            _dBContext.Notes.Add(notes);
            await _dBContext.SaveChangesAsync();
            
        }
        public async Task<Notes?> GetNoteByUserAndMovieAsync(string userId, string movieId)
        {
            return await _dBContext.Notes
                .FirstOrDefaultAsync(n => n.UserId == userId && n.Tconst == movieId);
        }

      
        public async Task UpdateNoteAsync(Notes note)
        {
            _dBContext.Notes.Update(note);
            await _dBContext.SaveChangesAsync();
        }


        public async Task<bool> UserExistsCheck(string Userid)
        {
            return await _dBContext.user_info.AnyAsync(u => u.user_id == Userid);
        }
        public async Task<bool> MovieExistsCheck(string MovieId)
        {
            return await _dBContext.title_basics.AnyAsync(u => u.tconst == MovieId);
        }
        public async Task<bool> NoteExistsCheck(string UserId,string MovieId)
        {
            return await _dBContext.Notes.AnyAsync(u => u.UserId == UserId &&  u.Tconst == MovieId);
        }
        public async Task DeleteNotes(string userid,string MovieId)
        {
            var note = await _dBContext.Notes.FirstOrDefaultAsync(n => n.UserId==userid && n.Tconst == MovieId);
            if (note != null)
            {
                _dBContext.Notes.Remove(note);
                await _dBContext.SaveChangesAsync();
            }

        }
        public async Task<Notes> GetAllNotesForUserandMovie(string UserId,string MovieId)
        {
            return await _dBContext.Notes
                .SingleOrDefaultAsync(m => m.UserId == UserId && m.Tconst == MovieId);
              
        }

        public async Task<IEnumerable<Notes>> GetAllNotesByUser(string UserId, int page, int pagesize)
        {
            return await _dBContext.Notes
                .Where(m => m.UserId == UserId)
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }

        public async Task<IEnumerable<title_basics>> GetAllNotesByMovie(string MovieId, int page, int pagesize)
        {
            return await _dBContext.title_basics
                .Where(m => m.tconst == MovieId)
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }
        public async Task<int> CountNotesByUser(string UserId)
        {
            return await _dBContext.Notes.Where(m => m.UserId == UserId)
            .CountAsync();
        }
        public async Task<int> CountNotesByMovie(string MovieId)
        {
            return await _dBContext.title_basics.Where(m => m.tconst == MovieId)
            .CountAsync();
        }
    }
}
