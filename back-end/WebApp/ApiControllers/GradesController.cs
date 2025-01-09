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
using Asp.Versioning;
using Microsoft.AspNetCore.Identity;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IAppUnitOfWork _uow;

        public  GradesController(AppDbContext context, IAppUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }

        [HttpGet("studentSubject/{studentSubjectId}")]
        public async Task<ActionResult<IEnumerable<App.DAL.DTO.Grade>>> GetGrades(Guid studentSubjectId)
        {
            var res = await _uow.Grades.GetAllByStudentSubjectIdAsync(studentSubjectId);
            return Ok(res);
        }

        // GET: api/Grades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DAL.DTO.Grade>?> GetGrade(Guid id)
        {
            
            var grade = await _uow.Grades.FirstOrDefaultAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            return grade;
        }

        // PUT: api/Grades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrade(Guid id, Grade grade)
        {
            if (id != grade.Id)
            {
                return BadRequest();
            }

            _context.Entry(grade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(id))
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

        // POST: api/Grades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<App.DAL.DTO.Grade>> PostGrade(App.DAL.DTO.Grade grade)
        {
            
            grade.Id = Guid.NewGuid();
            _uow.Grades.Add(grade);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetGrade", new { id = grade.Id }, grade);
        }

        // DELETE: api/Grades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(Guid id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GradeExists(Guid id)
        {
            return _context.Grades.Any(e => e.Id == id);
        }
    }
}
