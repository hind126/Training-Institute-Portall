using FinalAssignmentBrief.Data;
using FinalAssignmentBrief.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class StudentProfilesController : Controller
{
    private readonly ApplicationDbContext _context;

    public StudentProfilesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: STUDENTPROFILES
    public async Task<IActionResult> Index()
    {
        var profiles = _context.StudentProfiles
            .Include(sp => sp.Student)
            .ToListAsync();
        return View(await profiles);
    }

    // GET: STUDENTPROFILES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var studentprofile = await _context.StudentProfiles
            .Include(sp => sp.Student)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (studentprofile == null)
        {
            return NotFound();
        }

        return View(studentprofile);
    }

    // GET: STUDENTPROFILES/Create
    public IActionResult Create()
    {
        ViewBag.Students = new SelectList(_context.Students, "Id", "FullName");
        return View();
    }

    // POST: STUDENTPROFILES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,PhoneNumber,Address,EmergencyContact,StudentId")] StudentProfile studentprofile)
    {
        bool stdExists = _context.StudentProfiles.Any(sp => sp.StudentId == studentprofile.StudentId);
        if (stdExists)
        {
            ModelState.AddModelError("StudentId", "A profile for this student already exists.");
        }
        if (ModelState.IsValid)
        {
            _context.Add(studentprofile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(studentprofile);
    }

    // GET: STUDENTPROFILES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var studentprofile = await _context.StudentProfiles.FindAsync(id);
        if (studentprofile == null)
        {
            return NotFound();
        }
        ViewBag.Students = new SelectList(_context.Students, "Id", "FullName", studentprofile.StudentId);
        return View(studentprofile);
    }

    // POST: STUDENTPROFILES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,PhoneNumber,Address,EmergencyContact,StudentId")] StudentProfile studentprofile)
    {
        if (id != studentprofile.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(studentprofile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentProfileExists(studentprofile.Id))
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
        return View(studentprofile);
    }

    // GET: STUDENTPROFILES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var studentprofile = await _context.StudentProfiles
            .FirstOrDefaultAsync(m => m.Id == id);
        if (studentprofile == null)
        {
            return NotFound();
        }

        return View(studentprofile);
    }

    // POST: STUDENTPROFILES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var studentprofile = await _context.StudentProfiles.FindAsync(id);
        if (studentprofile != null)
        {
            _context.StudentProfiles.Remove(studentprofile);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentProfileExists(int? id)
    {
        return _context.StudentProfiles.Any(e => e.Id == id);
    }
}
