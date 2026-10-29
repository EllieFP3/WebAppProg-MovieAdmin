
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;
using MoviesAdmin.Services;
[Authorize]
public class MoviesController : Controller
{
    private readonly MoviesAdminContext _context;
    private readonly AzureStorageService _blobService;

    public MoviesController(MoviesAdminContext context, AzureStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index(string sortOrder)    
    {
        if (sortOrder == "ReleaseDate")
        {
            ViewData["DateSort"] = "ReleaseDate_Desc";
        }
        else { 
            ViewData["DateSort"] = "ReleaseDate";
        }
        var movies = from m in _context.Movie select m;
        switch (sortOrder)
        {
            case "ReleaseDate":
                movies = movies.OrderBy(m => m.ReleaseDate);
                break;
            case "ReleaseDate_Desc":
                movies = movies.OrderByDescending(m => m.ReleaseDate);
                break;
            default:
                movies = movies.OrderByDescending(m => m.ReleaseDate);
                break;
        }
        return View(await movies.ToListAsync());
    }

    // GET: MOVIES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
        return View();
    }

    //
    // TO-DO: handle image being required
    //

    // POST: MOVIES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create([Bind("Id,Title,Synopsys,Genre,Rating,RuntimeInMinutes,ReleaseDate")] Movie movie, IFormFile? imageFile) // collect inputs and file input from form
    {

        if (imageFile == null)
        {
            ModelState.AddModelError("ImageFile", "The image file upload is required.");
        }

        if (ModelState.IsValid) // check for valid model state before adding to db
        {
            //create guid for file name
            var extension = Path.GetExtension(imageFile!.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            // save file to azure
            string imageUrl = await _blobService.UploadFileAsync(imageFile, uniqueFileName); // save file to azure
            
            // save image url in movie record
            movie.ImageUrl = imageUrl;
            
            // save record in db
            _context.Add(movie);
            await _context.SaveChangesAsync();
            
            // redirect to the index action after creating movie record
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Edit/5
    [Authorize(Roles = "Administrator")]

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    // POST: MOVIES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Synopsys,Genre,Rating,RuntimeInMinutes,ReleaseDate,ImageUrl")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
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
        return View(movie);
    }

    // GET: MOVIES/Delete/5
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}
