using System;
using Chinook.Model;
using Chinook.Model.Commands.Actors;
using Chinook.Model.Queries;
using Chinook.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Web.Controllers
{
    public class ActorController : Controller
    {
        private readonly ActorQuery _query;
        private readonly ActorCommand _command;

        public ActorController(ActorQuery query,ActorCommand command)
        {
            _query = query;
            _command = command;
        }
        // GET
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var actors = _query.GetAll();
            return View(actors);
        }
        [HttpGet]
        public IActionResult Details(int actorId)
        {
            var actor = _query.GetActorsMovies(actorId);
            return View(actor);
        }
        [HttpGet]
        public IActionResult Edit(int actorId)
        {
            var actor = _query.GetById(actorId);
            if (actor != null)
            {
                return View(actor);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] ActorViewModel actor)
        {
            if (ModelState.IsValid)
            {
                var newActor = new Actor {FirstName = actor.FirstName, LastName = actor.LastName};
                var result = _command.AddActor(newActor);
                return Redirect(string.Format("/Actor/?result={0}", result));
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(int actorId,[FromForm] Actor actor)
        {
            if (ModelState.IsValid)
            {
                actor.ModifiedAt = DateTime.Now;
                var result = _command.UpdateActor(actor);
                return Redirect(string.Format("/actor/index/?result={0}", result));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Remove(int actorId)
        {
            var result = _command.Remove(actorId);
            return Redirect(string.Format("/actor/index/?result={0}", result));
        }
    }
}