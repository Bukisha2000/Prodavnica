using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNotesApp.Data;

namespace MyNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class NoteController : ControllerBase
    {

        private readonly DataContext _context;

        private static List<Note> notes = new List<Note>
      {  };

        public NoteController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok( _context.Notes.ToList());
        }

        [HttpPost]
        public ActionResult SetNote([FromBody] Note note)
        {
            _context.Notes.Add(note);

             _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteNote(int id)
        {
            var oneNote = _context.Notes.SingleOrDefault(X => X.id == id);
            if(oneNote != null)
            {
                _context.Notes.Remove(oneNote);
                _context.SaveChanges();
                return NoContent();
               
            }
            return NotFound();

        }
        [HttpPut]
        public ActionResult UpdateNote( Note note)
        {
            var oneNote = _context.Notes.SingleOrDefault(x=> x.id == note.id);
            if(oneNote != null)
            {
                oneNote.NoteDescription = note.NoteDescription;
                oneNote.NoteName = note.NoteName;
                _context.Notes.Update(oneNote);
                _context.SaveChanges();
                return NoContent();

            }

            return NotFound();
        }
        [HttpGet("name")]
        public IEnumerable<Note> GetAllByName(String noteName)
        {

            var notesAll = _context.Notes.Where(Note => Note.NoteName.StartsWith(noteName)).AsEnumerable().ToList();

            return notesAll;
        }



    }
}
