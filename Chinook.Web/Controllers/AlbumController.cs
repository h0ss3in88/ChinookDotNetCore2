using Chinook.Model.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Web.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AlbumQuery _query;

        public AlbumController(AlbumQuery query)
        {
            _query = query;
        }
        // GET
        public IActionResult Index()
        {
            var result = _query.GetAll();
            return View(result);
        }

        public IActionResult View(int artistId)
        {
            var result = _query.GetByArtistId(artistId);
            return View(result);
        }
    }
}