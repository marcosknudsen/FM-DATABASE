using AutoMapper;
using FM_DATABASE.Models;
using FM_DATABASE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FM_DATABASE.Controllers
{
    public class ClubController : Controller
    {
        private readonly IRepositoryClubs repositoryClubs;
        private readonly IRepositoryPlayers repositoryPlayers;
        private readonly IRepositoryLeagues repositoryLeagues;
        private readonly IMapper mapper;

        public ClubController(IRepositoryClubs repositoryClubs,IRepositoryPlayers repositoryPlayers,IRepositoryLeagues repositoryLeagues,IMapper mapper)
        {
            this.repositoryClubs = repositoryClubs;
            this.repositoryPlayers = repositoryPlayers;
            this.repositoryLeagues = repositoryLeagues;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clubs = await repositoryClubs.GetAll();
            ClubListViewModel lvm;
            var clvArray = new List<ClubListViewModel>();

            foreach (Club c in clubs)
            {
                lvm = new ClubListViewModel();
                lvm.Id = c.Id;
                lvm.Name = c.Name;
                lvm.LeagueId = c.LeagueId;
                lvm.LeagueName = (await repositoryLeagues.GetById(c.LeagueId)).Name;
                clvArray.Add(lvm);
            }

            return View(clvArray);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var modelo = new ClubCreationViewModel();
            modelo.Leagues=await GetLeagues();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClubCreationViewModel club)
        {
            if (!ModelState.IsValid)
            {
                club.Leagues = await GetLeagues();
                return View(club);
            }

            await repositoryClubs.Create(club);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var club=await repositoryClubs.GetById(id);
            var modelo = mapper.Map<ClubCreationViewModel>(club);
            modelo.Leagues = await GetLeagues();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClubCreationViewModel club)
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

            var players = await repositoryPlayers.GetPlayersByClub(club);

            if (players.Count()!=0)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositoryClubs.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetLeagues()
        {
            var leagues = await repositoryLeagues.GetAll();
            return leagues.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
    }
}
