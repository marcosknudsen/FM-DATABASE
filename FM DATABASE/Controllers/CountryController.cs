using FM_DATABASE.Models;
using FM_DATABASE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FM_DATABASE.Controllers
{
    public class CountryController : Controller
    {
        private readonly IRepositoryCountries repositoryCountries;

        public CountryController(IRepositoryCountries repositoryCountries)
        {
            this.repositoryCountries = repositoryCountries;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var countries = await repositoryCountries.GetAll();
            return View(countries);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(Country country)
        {
            if (!ModelState.IsValid)
            {
                return View(country);
            }

            await repositoryCountries.Create(country);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var country = await repositoryCountries.GetById(id);
            return View(country);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Country country)
        {
            if (!ModelState.IsValid)
            {
                return View(country);
            }

            await repositoryCountries.Edit(country);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var country=await repositoryCountries.GetById(id);
            return View(country);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Country country)
        {
            await repositoryCountries.Delete(country);
            return RedirectToAction("Index");
        }
    }
}
