using System.Linq;
using Chinook.Model.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Web.Controllers
{
    public class FilmController : Controller
    {
        private readonly FilmQuery _query;

        public FilmController(FilmQuery query)
        {
            _query = query;
        }
        // GET
        public IActionResult Index()
        {
            var result = _query.GetAllMovies();
            ViewBag.Categories = _query.GetAllCategories();
            return View(result);
        }

        public IActionResult Details(int filmId)
        {
            var result = _query.GetMoviesDetails(filmId);
            return View(result);
        }

        public IActionResult Limited(int limit)
        {
            var result = _query.GetAllMovies().Take(limit);
            return View("Index", result);
        }
    }
}