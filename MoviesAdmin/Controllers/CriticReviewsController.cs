
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Models;

[Authorize(Roles = "Critic")]
public class CriticReviewsController : Controller
{
    private readonly MoviesAdminContext _context;

    private string? _userId;

    public CriticReviewsController(MoviesAdminContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        _userId = User.GetUserId();
    }

    // GET: CRITICREVIEWS
    public async Task<IActionResult> Index()    
    {
        var reviews = await _context.CriticReview
            .Where(m => m.CreatedBy == _userId)
            .Include(m => m.Movie)
            .ToListAsync();
        return View(reviews);
    }

    // GET: CRITICREVIEWS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticreview = await _context.CriticReview
            .Where(m => m.CreatedBy == _userId)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (criticreview == null)
        {
            return NotFound();
        }

        return View(criticreview);
    }

    // GET: CRITICREVIEWS/Create
    public IActionResult Create(int movieId)
    {
        ViewBag.MovieId = movieId;
        return View();
    }

    // POST: CRITICREVIEWS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MovieId,Movie,Description,Rating,IsPublished")] CriticReview criticReview)
    {

        criticReview.CreatedBy = _userId;
        criticReview.CreatedDate = DateTime.Now;

        if (ModelState.IsValid)
        {
            _context.Add(criticReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(criticReview);
    }

    // GET: CRITICREVIEWS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticreview = await _context.CriticReview.FindAsync(id);
        if (criticreview == null)
        {
            return NotFound();
        }
        return View(criticreview);
    }

    // POST: CRITICREVIEWS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MovieId,Movie,Description,Rating,IsPublished,CreatedBy,CreatedDate")] CriticReview criticreview)
    {
        if (id != criticreview.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(criticreview);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriticReviewExists(criticreview.Id))
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
        return View(criticreview);
    }

    // GET: CRITICREVIEWS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticreview = await _context.CriticReview
            .FirstOrDefaultAsync(m => m.Id == id);
        if (criticreview == null)
        {
            return NotFound();
        }

        return View(criticreview);
    }

    // POST: CRITICREVIEWS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var criticreview = await _context.CriticReview.FindAsync(id);
        if (criticreview != null)
        {
            _context.CriticReview.Remove(criticreview);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CriticReviewExists(int? id)
    {
        return _context.CriticReview.Any(e => e.Id == id);
    }
}
