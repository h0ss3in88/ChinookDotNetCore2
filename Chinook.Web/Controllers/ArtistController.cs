using System;
using System.Collections.Generic;
using Chinook.Model;
using Chinook.Model.Commands.Artists;
using Chinook.Model.Queries;
using Chinook.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Web.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ArtistQuery _query;
        private readonly ArtistCommand _command;

        public ArtistController(ArtistQuery query,ArtistCommand command)
        {
            _query = query;
            _command = command;
        }
        public IActionResult Index()
        {
            var result = _query.GetAll();
            return View(result);
        }

        public IActionResult View(int id)
        {
//            if (id == null || id.GetType() != typeof(Int32)) return View("Index");
            if (id.GetType() != typeof(Int32)) return View("Index");
            var result = _query.GetByAlbums(id);
            return View(result);
        }
        [HttpGet]
        public IActionResult Edit(int artistId)
        {
            var artist = _query.GetById(artistId);
            if (artist != null)
            {
                return View(artist);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] ArtistViewModel artist)
        {
            if (ModelState.IsValid)
            {
                var newArtist = new Artist {Name = artist.Name };
                var result = _command.AddArtist(newArtist);
                return Redirect(string.Format("/Artist/?result={0}", result));
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(int artistId,[FromForm] Artist artist)
        {
            if (ModelState.IsValid)
            {
                artist.Name = artist.Name;
                var result = _command.UpdateArtist(artist);
                return Redirect(string.Format("/artist/index/?result={0}", result));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Remove(int artistId)
        {
            var result = _command.Remove(artistId);
            return Redirect(string.Format("/artist/index/?result={0}", result));
        }
    }
}