using AutoMapper;
using FM_DATABASE.Models;
using FM_DATABASE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FM_DATABASE.Controllers
{
    public class LeagueController : Controller
    {
        private readonly IRepositoryLeagues repositoryLeagues;
        private readonly IRepositoryCountries repositoryCountries;
        private readonly IMapper mapper;

        public LeagueController(IRepositoryLeagues repositoryLeagues,IRepositoryCountries repositoryCountries,IMapper mapper)
        {
            this.repositoryLeagues = repositoryLeagues;
            this.repositoryCountries = repositoryCountries;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var leagues = await repositoryLeagues.GetAll();
            LeagueListViewModel lvm;
            var lvmArray = new List<LeagueListViewModel>();

            foreach (LeagueCreationViewModel l in leagues)
            {
                lvm=new LeagueListViewModel();
                lvm.Id=l.Id;
                lvm.Name=l.Name;
                lvm.Code=l.Code;
                lvm.Rank=l.Rank;
                lvm.CountryName = (await repositoryCountries.GetById(l.CountryId)).Name;
                lvmArray.Add(lvm);
            }

            return View(lvmArray);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var modelo = new LeagueCreationViewModel();
            modelo.Countries = await GetCountries();
            modelo.Ranks = GetRanks();
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var league= await repositoryLeagues.GetById(id);

            var modelo = mapper.Map<LeagueCreationViewModel>(league);
            modelo.Countries = await GetCountries();
            modelo.Ranks = GetRanks();
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var league=await repositoryLeagues.GetById(id);
            return View(league);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeagueCreationViewModel league)
        {
            if (!ModelState.IsValid)
            {
                return View(league);
            }

            await repositoryLeagues.Create(league);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LeagueCreationViewModel league)
        {
            if (!ModelState.IsValid)
            {
                return View(league);
            }

            await repositoryLeagues.Edit(league);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NotFound","Home");
            }

            await repositoryLeagues.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetCountries()
        {
            var countries = await repositoryCountries.GetAll();
            return countries.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        private IEnumerable<SelectListItem> GetRanks()
        {
            var ranks= new int[] { 1, 2, 3, 4, 5, 6 };
            return ranks.Select(x => new SelectListItem(x.ToString(),x.ToString()));
        }
    }
}
