using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using App.DTO.v1_0;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IAppUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.UserTeacher, App.Domain.Identity.AppUser> _mapper;


        public SubjectsController(AppDbContext context, IAppUnitOfWork uow, UserManager<AppUser> userManager, 
            IMapper autoMapper)
        {
            _context = context;
            _uow = uow;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.UserTeacher, App.Domain.Identity.AppUser>(autoMapper);
        }

        // GET: api/Subjects
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<App.DAL.DTO.Subject>>> GetSubjects()
        {
            var res = await _uow.Subjects.GetAllAsync(
                Guid.Parse(_userManager.GetUserId(User))
            );
            return Ok(res);
        }
        
        [HttpGet("WithoutLogin")]
        public async Task<ActionResult<List<App.DAL.DTO.Subject>>> GetSubjectsWithoutLogin()
        {
            var res = await _uow.Subjects.GetAllAsync();
            return Ok(res);
        }
        
        [HttpGet("Teachers")]
        public async Task<ActionResult<List<UserTeacher>>> GetTeachers()
        {
            var teachers = await _context.Users
                .Where(u => u.IsTeacher == true)
                .ToListAsync();

            var mappedTeachers = teachers
                .Select(e => _mapper.Map(e))
                .ToList();
            
            return Ok(mappedTeachers);
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DAL.DTO.Subject>> GetSubject(Guid id)
        {
            var subject = await _uow.Subjects.FirstOrDefaultAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }

        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(Guid id, Subject subject)
        {
            if (id != subject.Id)
            {
                return BadRequest();
            }

            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<App.DAL.DTO.Subject>?> PostSubject(App.DAL.DTO.Subject subject)
        {
            subject.Id = Guid.NewGuid();
            subject.AppUserId = Guid.Parse(_userManager.GetUserId(User));
            _uow.Subjects.Add(subject);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetSubject", new { id = subject.Id }, subject);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubjectExists(Guid id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
