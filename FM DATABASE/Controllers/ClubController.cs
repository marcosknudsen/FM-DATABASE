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
        public IActionResult Index()
        {
            return View();
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
    }
}
