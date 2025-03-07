using Microsoft.AspNetCore.Mvc;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly IActorRepository actorRepository;
        public ActorController(IActorRepository actorRepository)
        {
            this.actorRepository = actorRepository;
        }
        public IActionResult Index()
        {
            var actors = actorRepository.Get();
            return View(actors.ToList());
        }
        public IActionResult Delete(int id)
        {
            var movie = actorRepository.GetOne(e => e.Id == id);
            if(movie != null)
            {
                actorRepository.Delete(movie);
                actorRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Actors actor)
        {
            if (ModelState.IsValid)
            {
                actorRepository.Create(actor);
                actorRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(actor);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var actor = actorRepository.GetOne(e => e.Id == id);
            return View(actor);
        }
        [HttpPost]
        public IActionResult Edit(Actors actor)
        {
            if (ModelState.IsValid)
            {
                actorRepository.Edit(actor);
                actorRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(actor);
        }
    }
}
