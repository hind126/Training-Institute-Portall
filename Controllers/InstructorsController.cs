
using FinalAssignmentBrief.Data;
using FinalAssignmentBrief.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class InstructorsController : Controller
{
    private readonly ApplicationDbContext _context;

    public InstructorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: INSTRUCTORS
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var instructors = await _context.Instructors
            .Include(i => i.Courses)
            .ToListAsync();
        return View(instructors);
    }

    // GET: INSTRUCTORS/Details/5
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .Include(i => i.Courses)
            .ThenInclude(c => c.Department)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    // GET: INSTRUCTORS/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: INSTRUCTORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,FullName,Email,Specialization,HireDate")] Instructor instructor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(instructor);
    }

    // GET: INSTRUCTORS/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(m => m.Id == id);

        ViewBag.Courses = _context.Courses;
        if (instructor == null)
        {
            return NotFound();
        }
        return View(instructor);
    }

    // POST: INSTRUCTORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FullName,Email,Specialization,HireDate")] Instructor instructor, int[] selectedCourses)
    {
        if (id != instructor.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var instructorToUpdate = await _context.Instructors
                    .Include(i => i.Courses)
                    .FirstOrDefaultAsync(i => i.Id == id);
                if (instructorToUpdate == null)
                {
                    return NotFound();
                }

                instructorToUpdate.FullName = instructor.FullName;
                instructorToUpdate.Email = instructor.Email;
                instructorToUpdate.Specialization = instructor.Specialization;
                instructorToUpdate.HireDate = instructor.HireDate;

                instructorToUpdate.Courses.Clear();

                var courses = await _context.Courses.Where(c => selectedCourses.Contains(c.Id)).ToListAsync();

                foreach (var course in courses)
                {
                    instructorToUpdate.Courses.Add(course);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(instructor.Id))
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
        ViewBag.Courses = await _context.Courses.ToListAsync();
        return View(instructor);
    }

    // GET: INSTRUCTORS/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    // POST: INSTRUCTORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor != null)
        {
            _context.Instructors.Remove(instructor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool InstructorExists(int? id)
    {
        return _context.Instructors.Any(e => e.Id == id);
    }
}
