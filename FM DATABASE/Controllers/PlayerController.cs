using AutoMapper;
using FM_DATABASE.Models;
using FM_DATABASE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace FM_DATABASE.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IRepositoryClubs repositoryClubs;
        private readonly IRepositoryPlayers repositoryPlayers;
        private readonly IMapper mapper;

        public PlayerController(IRepositoryClubs repositoryClubs,IRepositoryPlayers repositoryPlayers,IMapper mapper)
        {
            this.repositoryClubs = repositoryClubs;
            this.repositoryPlayers = repositoryPlayers;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var players = await repositoryPlayers.GetAll();
            PlayerListViewModel pvm;
            var pvmArray = new List<PlayerListViewModel>();

            foreach(Player p in players)
            {
                pvm = new PlayerListViewModel();
                pvm.Id=p.Id;
                pvm.FirstName = p.FirstName;
                pvm.LastName = p.LastName;
                pvm.ClubName = (await repositoryClubs.GetById(p.ClubId)).Name;
                pvmArray.Add(pvm);
            }

            return View(pvmArray);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var modelo = new PlayerCreationViewModel();
            modelo.Clubs = await GetClubs();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerCreationViewModel player)
        {
            var club = await repositoryClubs.GetById(player.ClubId);

            if (player is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                player.Clubs = await GetClubs();
                return View(player);
            }

            await repositoryPlayers.Create(player);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var player = await repositoryPlayers.GetById(id);

            if (player is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var modelo = mapper.Map<PlayerCreationViewModel>(player);
            modelo.Clubs = await GetClubs();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlayerCreationViewModel player)
        {
            var club = await repositoryClubs.GetById(player.ClubId);

            if (player is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                player.Clubs = await GetClubs();
                return View(player);
            }

            await repositoryPlayers.Edit(player);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var player=await repositoryPlayers.GetById(id);
            return View(player);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player=await repositoryPlayers.GetById(id);

            if (player is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositoryPlayers.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetClubs()
        {
            var clubs = await repositoryClubs.GetAll();
            return clubs.Select(x => new SelectListItem(x.Name,x.Id.ToString()));
        }
    }
}
