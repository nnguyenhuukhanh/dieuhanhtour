using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using dieuhanhtour.ViewModel;

namespace dieuhanhtour.Controllers
{
    public class CompanyController : BaseController
    {
      
        private readonly ICompanyRepository _companyRepository;
        private readonly IQuocgiaRepository _qgRepository;

        public CompanyController(ICompanyRepository companyRepository, IQuocgiaRepository qgRepository)
        {
            _companyRepository = companyRepository;
            _qgRepository = qgRepository;
        }

        public void listQuocgia(string selected)
        {
            List<Quocgia> qg = _qgRepository.ListQuocgia().ToList();
            ViewBag.quocgia = new SelectList(qg, "Nation", "Nation", selected);
        }

       

        public IActionResult Index(string searchString, int? page)
        {
            searchString = string.IsNullOrEmpty(searchString) ? "" : searchString;
            HttpContext.Session.SetString("urlCompany", UriHelper.GetDisplayUrl(Request));
            var company = _companyRepository.ListCompany(searchString, page);
            ViewBag.company = company;
            ViewData["CurrentFilter"] = searchString;
            return View(company);
        }

        public IActionResult Create()
        {
           
            Company cp = new Company();
            cp.companyId = _companyRepository.nextCompanyCode();
            var model = new CompanyViewModel {
                Company=cp,
                Referer= Request.Headers["Referer"].ToString()
            };
          
            listQuocgia("");
           
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Company.chinhanh = HttpContext.Session.GetString("chinhanh");
                }catch
                {
                    model.Company.chinhanh = "STS";
                }
               
                _companyRepository.Create(model.Company);

                if (model.Referer != null)
                {
                    return Redirect(model.Referer);
                }
                return RedirectToAction("Index", "Company");
            }

            return View(model);
        }

        public IActionResult Edit(string id)
        {

            if (id == null)
            {
                SetAlert("Thêm  đối tác không thành công", "error");
            }

            Company ncu = _companyRepository.GetById(id);

            if (ncu == null)
            {
                SetAlert("Thêm đối tác không thành công", "error");
            }

            var model = new CompanyViewModel
            {
                Company = ncu
            };
            if (model.Company != null)
            {
                listQuocgia(model.Company.nation);
            }
            else
            {
                listQuocgia("");
            }
           
           
            ViewBag.Id = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {

                 _companyRepository.Update(model.Company);

                if (model.Referer != null)
                {
                    return Redirect(model.Referer);
                }
                return RedirectToAction("Index", "Company");
            }

            return View(model);
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qg = _companyRepository.GetById(id);
            if (qg == null)
            {
                return NotFound();
            }
            var result = _companyRepository.Delete(qg);
            if (result == null)
            {
                SetAlert("Xóa đối tác không thành công.", "error");
            }
            else
            {
                SetAlert("Xóa  đối tác thành công.", "success");
            }
            return Redirect(HttpContext.Session.GetString("urlCompany"));
        }

    }
}