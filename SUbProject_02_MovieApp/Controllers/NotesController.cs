using DataAccessLayer.DataModels;
using DataAccessLayer.Repository.NewFolder;
using Microsoft.AspNetCore.Mvc;
using SUbProject_02_MovieApp.DTOModels.NotesDTOs;
using System.Security.Claims;

namespace SUbProject_02_MovieApp.Controllers
{
    [ApiController]
    [Route("api/Notes")]
    public class NotesController : BaseController
    {
        private readonly INotesRepository _notesRepository;
        private readonly LinkGenerator _linkGenerator;
        public NotesController(INotesRepository notesRepository, LinkGenerator linkGenerator) : base(linkGenerator)
        {
            _notesRepository = notesRepository;
            _linkGenerator = linkGenerator;
        }
        [HttpPost]
        public async Task<IActionResult> AddNote(string Userid,string MovieId,string Notes)
        {
            var userExists = await _notesRepository.UserExistsCheck(Userid);
            var movieExists = await _notesRepository.MovieExistsCheck(MovieId);
            var noteExists = await _notesRepository.NoteExistsCheck(Userid,MovieId);
            if(!userExists)
            {
                return NotFound($"User with ID {Userid} does not exist.");
            }
            if(!movieExists)
            {
                return NotFound($"Movie with ID {MovieId} does not exist.");
            }
            if (noteExists)
            {
                return NotFound($"Note already added for user with id {Userid} and Movie with ID {MovieId}.");
            }
            var note = new Notes
            {
                UserId = Userid,
                Tconst = MovieId,
                Note = Notes,
                CreatedDate = DateTime.UtcNow
            };
            await _notesRepository.AddNotes(note);

            return Ok(note);
        }
        [HttpPut("notes/update")]
        public async Task<IActionResult> UpdateNote(string userId, string movieId, string newNoteContent)
        {
            var note = await _notesRepository.GetNoteByUserAndMovieAsync(userId, movieId);
            if (note == null)
            {
                return NotFound($"Note for User ID {userId} and Movie ID {movieId} does not exist.");
            }

         
            note.Note = newNoteContent;
            note.UpdatedDate = DateTime.UtcNow;
            note.CreatedDate = DateTime.UtcNow;
            await _notesRepository.UpdateNoteAsync(note);

            return Ok("Note updated successfully");
        }

        [HttpDelete("delete/{Userid}/{MovieId}")]
        public async Task<IActionResult> DeleteNote(string Userid,string MovieId)
        {
            
            await _notesRepository.DeleteNotes(Userid, MovieId);
            return Ok("Note Deleted Successfully");
        }

        [HttpGet("notes/{UserId}/{MovieId}")]
        public async Task<IActionResult> GetAllNotesForUserandMovie(string UserId,string MovieId)
        {

            if (string.IsNullOrWhiteSpace(MovieId))
            {
                return BadRequest("Movie Id is not provided.");
            }
            if (string.IsNullOrWhiteSpace(UserId))
            {
                return BadRequest("User Id is not provided.");
            }
            var notes = await _notesRepository.GetAllNotesForUserandMovie(UserId,MovieId);
           
            if (notes == null)
            {
                return NotFound("No movies found");
            }
          
            return Ok(notes);
        }
        [HttpGet("notes/{UserId}", Name = nameof(GetAllMoviesByUser))]
        public async Task<IActionResult> GetAllMoviesByUser(string UserId, int page = 0, int pagesize = 10)
        {

            if (string.IsNullOrWhiteSpace(UserId))
            {
                return BadRequest("User is not provided.");
            }
            var notes = await _notesRepository.GetAllNotesByUser(UserId, page, pagesize);
            var numberofnotes = await _notesRepository.CountNotesByUser(UserId);
            if (notes == null || !notes.Any())
            {
                return NotFound($"No notes found for user:{UserId}");
            }
            var result = CreatePaging(nameof(GetAllMoviesByUser),
                page,
                pagesize,
                numberofnotes,
            notes,
                new { UserId });
            return Ok(result);
        }
        [HttpGet("{Movie}", Name = nameof(GetAllMoviesByMovie))]
        public async Task<IActionResult> GetAllMoviesByMovie(string Movie, int page = 0, int pagesize = 10)
        {

            if (string.IsNullOrWhiteSpace(Movie))
            {
                return BadRequest("Movie id is not provided.");
            }
            var notes = await _notesRepository.GetAllNotesByMovie(Movie, page, pagesize);
            var numberofnotes = await _notesRepository.CountNotesByMovie(Movie);
            if (notes == null || !notes.Any())
            {
                return NotFound($"No notes found for movie:{Movie}");
            }
            var result = CreatePaging(nameof(GetAllMoviesByMovie),
                page,
                pagesize,
                numberofnotes,
            notes,
                new { Movie });
            return Ok(result);
        }


    }
}
