using FM_DATABASE.Models;
using FM_DATABASE.Services;
using Microsoft.AspNetCore.Mvc;

namespace FM_DATABASE.Controllers
{
    public class ClubController : Controller
    {
        private readonly IRepositoryClubs repositoryClubs;

        public ClubController(IRepositoryClubs repositoryClubs)
        {
            this.repositoryClubs = repositoryClubs;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clubs = await repositoryClubs.GetAll();
            return View(clubs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Club club)
        {
            if (!ModelState.IsValid)
            {
                return View(club);
            }

            await repositoryClubs.Create(club);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var club=await repositoryClubs.GetById(id);
            return View(club);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Club club)
        {
            if (!ModelState.IsValid)
            {
                return View(club);
            }

            await repositoryClubs.Edit(club);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var club = await repositoryClubs.GetById(id);
            return View(club);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await repositoryClubs.GetById(id);
            
            if (club is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositoryClubs.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
