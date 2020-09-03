using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dieuhanhtour.Controllers
{
    public class ThanhphoController : Controller
    {
        private IThanhphoRepository _thanhphoRepository;

        public ThanhphoController(IThanhphoRepository thanhphoRepository)
        {
            _thanhphoRepository = thanhphoRepository;
        }
        public IActionResult index()
        {
            var listTinh = _thanhphoRepository.ListTinh().OrderBy(x=>x.Matinh);
            return View(listTinh);
        }
        public IActionResult listThanhphoByTinh(string matinh)
        {
            var listtp = _thanhphoRepository.ListThanhphoByTinh(matinh);
            var tinh = _thanhphoRepository.ListTinh().Where(x => x.Matinh == matinh).FirstOrDefault();
            ViewBag.tentinh = tinh.Tentinh;
            return PartialView(listtp);
        }
        public ActionResult EditThanpho(string matp)
        {
            var thanhpho = _thanhphoRepository.getThanhphoById(matp);
            return PartialView(thanhpho);
        }
        [HttpPost]
        public ActionResult EditThanhpho(Thanhpho thanhpho)
        {
            int result = _thanhphoRepository.capnhatThanhpho(thanhpho);
            return Json(JsonConvert.SerializeObject(result));
        }
        public ActionResult ThemThanhpho(string matinh)
        {
            var thanhpho = new Thanhpho();
            thanhpho.Matp = _thanhphoRepository.newMatp(matinh);
            thanhpho.Matinh = matinh;
            return PartialView(thanhpho);
        }
        [HttpPost]
        public ActionResult ThemThanhpho(Thanhpho thanhpho)
        {
            int result = _thanhphoRepository.themThanhpho(thanhpho);
            return Json(JsonConvert.SerializeObject(result));
        }
    }
}