using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StudentSubjectsController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        private readonly IAppUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;

        public StudentSubjectsController(AppDbContext context, IAppUnitOfWork uow, UserManager<AppUser> userManager)
        {
            _context = context;
            _uow = uow;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<App.DAL.DTO.StudentSubject>>> GetAllStudentSubjects()
        {
            var res = await _uow.StudentSubjects.GetAllAsync();
            return Ok(res);
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<App.DAL.DTO.StudentSubject>>> GetStudentSubjects()
        {
            var res = await _uow.StudentSubjects.GetAllAsync(
                Guid.Parse(_userManager.GetUserId(User))
            );
            return Ok(res);
        }

        [HttpGet("subject/{id}")]
        public async Task<ActionResult<List<App.DAL.DTO.StudentSubject>>> GetStudentSubjects(Guid id)
        {
            var res = await _uow.StudentSubjects.GetAllBySubjectIdAsync(id);
            return Ok(res);
        }
        
        // GET: api/StudentSubjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentSubject>> GetStudentSubject(Guid id)
        {
            var studentSubject = await _context.StudentSubjects.FindAsync(id);

            if (studentSubject == null)
            {
                return NotFound();
            }

            return studentSubject;
        }

        // PUT: api/StudentSubjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentSubject(Guid id, App.DAL.DTO.StudentSubject studentSubject)
        {
            if (id != studentSubject.Id)
            {
                return BadRequest();
            }

            var sub1 = await _uow.StudentSubjects.FirstOrDefaultAsync(id);
            
            if (sub1 == null)
            {
                return NotFound();
            }
            
            _uow.StudentSubjects.Update(studentSubject);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentSubjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StudentSubjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<App.DAL.DTO.StudentSubject>> PostStudentSubject(App.DAL.DTO.StudentSubject subject)
        {
            var isDeclared = await _uow.IsAlreadyDeclaredInThisSemesterAsync(subject.SubjectId, Guid.Parse(_userManager.GetUserId(User)));
            
            if (isDeclared)
            {
                return BadRequest();
            }
            
            subject.Id = Guid.NewGuid();
            subject.AppUserId = Guid.Parse(_userManager.GetUserId(User));
            _uow.StudentSubjects.Add(subject);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetSubject", new { id = subject.Id }, subject);
        }

        // DELETE: api/StudentSubjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentSubject(Guid id)
        {
            var studentSubject = await _context.StudentSubjects.FindAsync(id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            _context.StudentSubjects.Remove(studentSubject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentSubjectExists(Guid id)
        {
            return _context.StudentSubjects.Any(e => e.Id == id);
        }
    }
}
