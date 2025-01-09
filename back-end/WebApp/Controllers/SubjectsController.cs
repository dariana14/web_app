using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly AppDbContext _context;

        public SubjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Subjects.Include(s => s.AppUser);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectName,SemesterNumber,AppUserId,Id")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.Id = Guid.NewGuid();
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", subject.AppUserId);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", subject.AppUserId);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SubjectName,SemesterNumber,AppUserId,Id")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", subject.AppUserId);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(Guid id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
