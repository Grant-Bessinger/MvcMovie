using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Migrations;
using MvcMovie.Models;



namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly IWebHostEnvironment hostingenvironment;

        public MoviesController(MvcMovieContext context, IWebHostEnvironment host)
        {
            _context = context;
             hostingenvironment = host;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            var movies = from m in _context.Movie
                         select m;
            
            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };


            return View(movieGenreVM);
        }

        
        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
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

     

        // GET: Movies/Create
        public IActionResult Create()
        {
            List<string> Mgenre = new List<string>()
                {
                    "Action",
                    "Animation",
                    "Comedy",
                    "Crime",
                    "Drama",
                    "Fantasy",
                    "Historical",
                    "Horror",
                    "Romance",
                    "Science",
                    "Thriller",
                    "Western",
                    "Other"
                };
            List<string> MRate = new List<string>()
            {
                "G",
                "PG",
                "PG-13",
                "R",
                "NC-17"
            };
            MovieViewModel viewModel = new MovieViewModel();
            viewModel.MovieRate = new List<SelectListItem>();
            viewModel.GenreMovies = new List<SelectListItem>();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<SelectListItem> selectListItem1 = new List<SelectListItem>();

            foreach (var genre in Mgenre)
            {
                var selectItem = new SelectListItem {
                Text = genre,
                Value = genre

                };
                selectListItem.Add(selectItem);

            }
            viewModel.GenreMovies = selectListItem;

            foreach (var rate in MRate)
            {
                var selectItem1 = new SelectListItem
                {
                    Text = rate,
                    Value = rate

                };
                selectListItem1.Add(selectItem1);

            }
            viewModel.MovieRate = selectListItem1;
            return View(viewModel);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel movie1)
        {
           
            if (ModelState.IsValid)
            {
                if (movie1.FileFrame != null)
                {
                    string folder = "Photos/Movie/";
                    string UploadPath= Path.Combine(hostingenvironment.WebRootPath, folder);
                    string filename = Guid.NewGuid().ToString() + "_" + movie1.FileFrame.FileName;
                    string filepath = Path.Combine(UploadPath, filename);
                    movie1.FileFrame.CopyTo(new FileStream(filepath, FileMode.Create));
                    movie1.pathFile = "/" + folder + filename;
                } 
             

                Movie movie = new Movie
                {
                    Title = movie1.Title,
                    ReleaseDate = movie1.ReleaseDate,
                    Genre = movie1.Genre,
                    Price = movie1.Price,
                    Rating = movie1.Rating,
                    pathFile = movie1.pathFile
                };

                _context.Movie.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            
            return View();
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();

            }


            List<string> Mgenre = new List<string>()
                {
                    "Action",
                    "Animation",
                    "Comedy",
                    "Crime",
                    "Drama",
                    "Fantasy",
                    "Historical",
                    "Horror",
                    "Romance",
                    "Science",
                    "Thriller",
                    "Western",
                    "Other"
                };

            List<string> MRate = new List<string>()
            {
                "G",
                "PG",
                "PG-13",
                "R",
                "NC-17"
            };

            MovieViewModel movie1 = new MovieViewModel();
            movie1.MovieRate = new List<SelectListItem>();
            movie1.GenreMovies = new List<SelectListItem>();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<SelectListItem> selectListItem1 = new List<SelectListItem>();

            foreach (var genre in Mgenre)
            {
                var selectItem = new SelectListItem
                {
                    Text = genre,
                    Value = genre

                };
                selectListItem.Add(selectItem);

            }
            foreach (var rate in MRate)
            {
                var selectItem1 = new SelectListItem
                {
                    Text = rate,
                    Value = rate

                };
                selectListItem1.Add(selectItem1);

            }

         
           
                movie1.GenreMovies = selectListItem;
                movie1.MovieRate = selectListItem1;
                movie1.Title = movie.Title;
                movie1.ReleaseDate = movie.ReleaseDate;
                movie1.Genre = movie.Genre;
                movie1.Price = movie.Price;
                movie1.Rating = movie.Rating;
                movie1.pathFile = movie.pathFile;
           

                return View(movie1);
            
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieViewModel movie1)
        {

            if (id != movie1.Id)
            {
                return NotFound();
            }
         
          

            if (ModelState.IsValid)
            {
                var movie = await _context.Movie.FindAsync(id);

                if (movie == null)
                {
                    return BadRequest();

                }
                if (movie1.FileFrame != null)
                {
                    string folder = "Photos/Movie/";
                    string UploadPath = Path.Combine(hostingenvironment.WebRootPath, folder);
                    string filename = Guid.NewGuid().ToString() + "_" + movie1.FileFrame.FileName;
                    string filepath = Path.Combine(UploadPath, filename);
                    movie1.FileFrame.CopyTo(new FileStream(filepath, FileMode.Create));
                    movie1.pathFile = "/" + folder + filename;

                }

                movie.Title = movie1.Title;
                movie.ReleaseDate = movie1.ReleaseDate;
                movie.Genre = movie1.Genre;
                movie.Price = movie1.Price;
                movie.Rating = movie1.Rating;
                

                if (movie1.pathFile != null)
                {
                    movie.pathFile = movie1.pathFile;
                }
               
                try
                {
                    _context.Movie.Update(movie);
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
            return View(movie1);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
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

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
