using System.Threading.Tasks;
using Chinook.Model.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Web.Controllers
{
    public class PlayListController : Controller
    {
        private readonly PlayListQuery _query;
        public PlayListController(PlayListQuery query)
        {
            _query = query;
        }
        // GET
        public IActionResult Index()
        {
            var result = _query.GetAll();
            return View(result);
        }

        public async Task<IActionResult> View(int id)
        {
            var result = await _query.GetTracksById(id);
            return View(result);
        }
    }
}