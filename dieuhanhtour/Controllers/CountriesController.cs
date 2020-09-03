using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Interfaces;

namespace dieuhanhtour.Controllers
{
    public class CountriesController : BaseController
    {
        private readonly ICountryRepository _countryRepository;

        public CountriesController( ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        // GET: Countries
        public async Task<IActionResult> Index(string searchString)
        {
            searchString = !String.IsNullOrEmpty(searchString) ? searchString : "";
            var model = await _countryRepository.FindAsync(x => x.Nation.Contains(searchString) || x.TelCode.Contains(searchString));
            @ViewData["currentFilter"] = searchString;
            return View(model);
        }

        //public async Task<IActionResult> Index()
        //{
        //    var model = await _countryRepository.GetAllAsync();
        //    return View(model);
        //}
        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Code,Codealpha,Nation,Natione,TelCode,Continent,Territory")] country country)
        {
            if (ModelState.IsValid)
            {
                var result = _countryRepository.Create(country);
                if (result == null)
                {
                    SetAlert("Thêm quốc gia không thành công", "error");
                }
                else
                {
                    SetAlert("Thêm quốc gia thành công", "success");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int id)
        {            
            var country =  await _countryRepository.GetByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Code,Codealpha,Nation,Natione,TelCode,Continent,Territory")] country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _countryRepository.Update(country);
                if (result == null)
                {
                    SetAlert("Cập nhật quốc gia không thành công", "error");
                }
                else
                {
                    SetAlert("Cập nhật quốc gia thành công", "success");
                }               
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int id)
        {            
            var quocgia = await _countryRepository.GetByIdAsync(id);
            if (quocgia == null)
            {
                return NotFound();
            }
            var result = _countryRepository.Delete(quocgia);
            if (result == null)
            {
                SetAlert("Xóa quốc gia không thành công", "error");
            }
            else
            {
                SetAlert("Xóa quốc gia thành công", "success");
            }
            return RedirectToAction(nameof(Index));
        }

       
       
    }
}
