using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using dieuhanhtour.ViewModel;
using Newtonsoft.Json;

namespace dieuhanhtour.Controllers
{
    public class TourTemplateController : BaseController
    {
        private readonly ITourTempRepository _tourTempRepository;
        private readonly IChinhanhRepository _chinhanhRepository;
        private readonly ITourProgTempRepository _tourProgTempRepository;
        private readonly INgoaiteRepository _ngoaiteRepository;
        private readonly IDichvuRepository _dichvuRepository;
        private readonly ITourTempNoteRepository _tourTempNoteRepository;
        private readonly ITuyentqRepository _tuyentqRepository;
        private readonly ITourkindRepository _tourkindRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IHotelTempRepository _hotelTempRepository;
        private readonly ISightseeingTempRepository _seeTempRepository;
        private readonly IDiemtqRepository _diemtqRepository;
        private readonly IThanhphoRepository _thanhphoRepository;

        public TourTemplateController(ITourTempRepository tourTempRepository, IChinhanhRepository chinhanhRepository, ITourProgTempRepository tourProgTempRepository, 
                                        INgoaiteRepository ngoaiteRepository, IDichvuRepository dichvuRepository, ITourTempNoteRepository tourTempNoteRepository, 
                                        ITuyentqRepository tuyentqRepository, ITourkindRepository tourkindRepository, ISupplierRepository supplierRepository, 
                                        IHotelTempRepository hotelTempRepository, ISightseeingTempRepository seeTempRepository, IDiemtqRepository diemtqRepository,IThanhphoRepository thanhphoRepository
                                        )
        {
            _tourTempRepository = tourTempRepository;
            _chinhanhRepository = chinhanhRepository;
            _tourProgTempRepository = tourProgTempRepository;
            _ngoaiteRepository = ngoaiteRepository;
            _dichvuRepository = dichvuRepository;
            _tourTempNoteRepository = tourTempNoteRepository;
            _tuyentqRepository = tuyentqRepository;
            _tourkindRepository = tourkindRepository;
            _supplierRepository = supplierRepository;
            _hotelTempRepository = hotelTempRepository;
            _seeTempRepository = seeTempRepository;
            _diemtqRepository = diemtqRepository;
            _thanhphoRepository = thanhphoRepository;
        }



        // GET: TourTemplate
        public IActionResult Index(string searchString, int page = 1)
        {
            searchString = string.IsNullOrEmpty(searchString) ? "" : searchString;
            HttpContext.Session.SetString("urlTourtemplate", UriHelper.GetDisplayUrl(Request));
            var tourtemp = _tourTempRepository.GetTourtemplate(searchString,HttpContext.Session.GetString("chinhanh"),page);
            ViewBag.tourtemp = tourtemp;
            ViewData["CurrentFilter"] = searchString;
            return View(tourtemp);
        }

        #region Thêm tour template

        // GET: TourTemplate/Create
        public IActionResult Create()
        {
            var tourtemp = new TourTemplate();
            tourtemp.Code = _tourTempRepository.newTourTempId();
            tourtemp.Songay = 1;
            listTuyentq("");
            listTourkind("1");
            listChinhanh(HttpContext.Session.GetString("chinhanh"));
            return View(tourtemp);
        }

        // POST: TourTemplate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Code,Tourkind,Tentour,Tuyentq,Chudetour,Songay,Chinhanh,nguoitao")] TourTemplate tourTemplate)
        {
            if (ModelState.IsValid)
            {
                tourTemplate.nguoitao = HttpContext.Session.GetString("username");
                tourTemplate.Chudetour = tourTemplate.Chudetour ?? "";
                var result = _tourTempRepository.Create(tourTemplate);
               
                if (result == null)
                {
                    SetAlert("Thêm tour template không thành công", "error");
                    return View(tourTemplate);
                }
                else
                {
                    //TourTempNote t = new TourTempNote();
                    //t.Code = tourTemplate.Code;
                    //_tourTempNoteRepository.Create(t);
                    SetAlert("Thêm tour template thành công", "success");
                    return RedirectToAction("Edit", new { id = result.Code });
                }
                
            }
            return View(tourTemplate);
        }
        #endregion
        // GET: TourTemplate/Edit/5
        #region Cập nhật tour template
        public async Task< IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var tourtemp = await _tourTempRepository.GetByIdAsync(id);
            var t = _tourTempRepository.GetById(tourtemp.Code);
            ViewBag.nguoitao = t.nguoitao;
            if (tourtemp == null)
            {
                return NotFound();
            }
            else if (tourtemp.Chinhanh != HttpContext.Session.GetString("chinhanh"))
            {
                SetAlert("Bạn không có quyền cập nhật tour này", "error");
                return Redirect(HttpContext.Session.GetString("urlTourtemplate"));
            }
            else
            {
                listTuyentq(tourtemp.Tuyentq);
                listChinhanh(tourtemp.Chinhanh);
                listTourkind(tourtemp.Tourkind);
                return View(tourtemp);
            }
        }

        // POST: TourTemplate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Code,Tourkind,Tentour,Tuyentq,Chudetour,Songay,Chinhanh")] TourTemplate tourTemplate)
        {
            if (id != tourTemplate.Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _tourTempRepository.Update(tourTemplate);
                if (result == null)
                {
                    SetAlert("Cập nhật tour template không thành công", "error");
                }
                else
                {
                    SetAlert("Cập nhật tour template thành công", "success");
                }
                return Redirect(HttpContext.Session.GetString("urlTourtemplate"));
            }
            return View(tourTemplate);
        }
        #endregion
        #region Xoá tour template
        public ActionResult getTourTempByCode(string code)
        {
            var result = _tourTempRepository.getTourTempByCode(code);
            return Json(result);
        }
        // GET: TourTemplate/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourTemplate = _tourTempRepository.GetById(id);
            if (tourTemplate == null)
            {
                return NotFound();
            }
            var result = _tourTempRepository.Delete(tourTemplate);
            if (result == null)
            {
                SetAlert("Xóa tour template không thành công", "error");
            }
            else
            {
                SetAlert("Xóa tour template thành công", "success");
            }
            return Redirect(HttpContext.Session.GetString("urlTourtemplate"));
        }
        #endregion
        #region List các dropdownlist
        public void listHanhtrinhTu(string selected)
        {
            var hanhtrinhtu = _thanhphoRepository.ListTinh().ToList();
            var empty = new vTinh
            {
                Matinh = "",
                Tentinh = " -- Chọn thông tin --"
            };
            hanhtrinhtu.Insert(0, empty);
            ViewBag.hanhtrinhtu = new SelectList(hanhtrinhtu, "Matinh", "Tentinh", selected);
        }
        public void listHanhtrinhDen(string selected)
        {
            var hanhtrinhden = _thanhphoRepository.ListTinh().ToList();
            var empty = new vTinh
            {
                Matinh = "",
                Tentinh = " -- Chọn thông tin --"
            };
            hanhtrinhden.Insert(0, empty);
            ViewBag.hanhtrinhden = new SelectList(hanhtrinhden, "Matinh", "Tentinh", selected);
        }

        public void listTuyentq(string select)
        {
            List<Tuyentq> tuyentq = _tuyentqRepository.GetAll().ToList();            
            ViewBag.tuyentq = new SelectList(tuyentq, "Code", "Tentuyen", select);
        }
        public void listChinhanh(string selected)
        {
            List<Dmchinhanh> chinhanh = _chinhanhRepository.ListChinhanh().ToList();
            ViewBag.listChinhanh = new SelectList(chinhanh, "Macn", "Macn", selected);
        }
        public void listNgoaite(string selected)
        {
            List<Ngoaite> ngoaite = _ngoaiteRepository.GetAll().ToList();
            ViewBag.ngoaite = new SelectList(ngoaite, "MaNT", "MaNT", selected);
        }
        public void listDichvu(string selected)
        {
            List<Dichvu> dichvu = _dichvuRepository.Find(x => x.Trangthai == true).ToList();
            ViewBag.dichvu = new SelectList(dichvu, "Iddichvu", "Tendv", selected);
        }
        public void listTourkind(string selected)
        {
            List<Tourkind> tourkind = _tourkindRepository.GetAll().ToList();
            ViewBag.tourkind = new SelectList(tourkind, "Id", "TourkindInf", selected);
        }
        public void listSupplier(string selected)
        {
            List<Supplier> supplier = _supplierRepository.ListSupplier().ToList();
            var empty = new Supplier
            {
                Code = "",
                Tengiaodich = "-- Chọn Supplier -- "
            };
            supplier.Insert(0, empty);
            ViewBag.supplier = new SelectList(supplier, "Code", "Tengiaodich", selected);
        }
        public void listSrvcode(string selected)
        {
            List<Supplier> srvcode = _supplierRepository.ListSupplier().ToList();
            var empty = new Supplier
            {
                Code = "",
                Tengiaodich = "-- Chọn thông tin -- "
            };
            srvcode.Insert(0, empty);
            ViewBag.srvcode = new SelectList(srvcode, "Code", "Tengiaodich", selected);
        }
       
        public void listAirType(string selected = "")
        {
            try
            {
                List<SelectListItem> airtype = new List<SelectListItem>();
                airtype.Add(new SelectListItem { Text = " ", Value = "" });
                airtype.Add(new SelectListItem { Text = "DON", Value = "ĐÓN" });
                airtype.Add(new SelectListItem { Text = "TIEN", Value = "TIỄN" });
                airtype.Add(new SelectListItem { Text = "BAY", Value = "BAY" });
                ViewBag.loaihinh = new SelectList(airtype, "Text", "Value", selected);
            }
            catch { return; }
        }

        public void listHttt(string selected="")
        {
            List<SelectListItem> select = new List<SelectListItem>();
           
            select.Add(new SelectListItem {Text="",Value="" });
            select.Add(new SelectListItem { Text = "Tiền mặt", Value = "TIEN MAT" });
            select.Add(new SelectListItem { Text = "Chuyển khoản", Value = "CHUYEN KHOAN" });
            ViewBag.httt = new SelectList(select, "Value", "Text", selected);
        }

        #endregion
        #region Thao tác thêm, xoá  dịch vụ trong chương trình tour


        //-----------Tour Programe Template------------
        public IActionResult listTourProgTemp(string id)
        {
            var progtemp = _tourProgTempRepository.ListTourProgTemp(id);
            var t = _tourTempRepository.GetById(id);
            if (t != null)
            {
                ViewBag.nguoitao = t.nguoitao;
            }
            foreach (var item in progtemp)
            {
                switch (item.srvtype)
                {
                    case "LUN":                    
                    case "SHP":
                        if (item.supplierid != null)
                        {
                            var supplier = _supplierRepository.GetById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich + " - " + supplier.Diachi + " phone: " + supplier.Dienthoai;
                        }
                        else
                        {
                            item.tour_item = item.tour_item;
                        }
                        break;
                    case "OVR":
                    case "PAC":
                        if (item.supplierid != null)
                        {
                            var supplier = _supplierRepository.GetById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich ;
                        }
                        else
                        {
                            item.tour_item = item.tour_item;
                        }
                        break;
                    case "ITI":
                        string tu = "", den = "";
                        if (!string.IsNullOrEmpty(item.tour_item))
                        {
                            tu = _thanhphoRepository.ListTinh().Where(x => x.Matinh == item.tour_item).SingleOrDefault().Tentinh;
                        }
                        if (!string.IsNullOrEmpty(item.srvnode))
                        {
                            den = _thanhphoRepository.ListTinh().Where(x => x.Matinh == item.srvnode).SingleOrDefault().Tentinh ?? "";
                        }
                        item.tour_item = "Hành trình: " + (string.IsNullOrEmpty(tu) ? "" : tu) + (string.IsNullOrEmpty(den) ? "" : " - " + den);
                        item.srvnode = "";
                        break;
                    case "AIR":
                         item.tour_item= item.tour_item + " chặng: " + item.dep + "-" + item.arr + " chuyến: " + item.carrier + " * " + String.Format("{0:#,##0.0}", item.unitpricea) + " " + item.currency;
                        break;
                    case "HTL":
                    case "CRU":
                        if (item.supplierid != null)
                        {
                            string gia = "";
                            var supplier = _supplierRepository.GetById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich + " - " + supplier.Diachi + " phone: " + supplier.Dienthoai;
                            var hotel = _hotelTempRepository.Find(x => x.Code == item.Code);
                            if (hotel != null)
                            {
                                foreach(var a in hotel)
                                {
                                 
                                    if (a.sgl > 0)
                                    {
                                        gia += a.sgl + "SGN" + "*" + string.Format("{0:#,##0.0}", a.sglcost)+a.currency;
                                    }
                                    if (a.extsgl > 0)
                                    {
                                        gia += ","+ a.extsgl + "EXT-SGN" + "*" + string.Format("{0:#,##0.0}", a.extsglcost) + a.currency;
                                    }
                                   
                                    if (a.dbl > 0)
                                    {
                                        gia += "," + a.dbl + "DBL" + "*" + string.Format("{0:#,##0.0}", a.dblcost) + a.currency;
                                    }
                                    if (a.extdbl > 0)
                                    {
                                        gia += "," + a.extdbl + "EXT-DBL" + "*" + string.Format("{0:#,##0.0}", a.extdblcost) + a.currency;
                                    }
                                   
                                    if (a.twn > 0)
                                    {
                                        gia += "," + a.twn + "TWN" + "*" + string.Format("{0:#,##0.0}", a.twncost) + a.currency;
                                    }
                                    if (a.exttwn > 0)
                                    {
                                        gia += "," + a.exttwn + "EXT-TWN" + "*" + string.Format("{0:#,##0.0}", a.exttwncost) + a.currency;
                                    }
                                   
                                    if (a.homestay > 0)
                                    {
                                        gia += "," + a.homestay + "TPL" + "*" + string.Format("{0:#,##0.0}", a.homestaycost) + a.currency;
                                    }
                                   
                                    if (a.oth > 0)
                                    {
                                        gia += "," +a.oth+ " OTH-"+ a.othpax + "pax" +  "*" + string.Format("{0:#,##0.0}", a.othcost) + a.currency+"-"+ a.othtype;
                                    }
                                    
                                }
                              
                            }
                            item.tour_item = item.tour_item + " " + gia;
                        }
                        else
                        {
                            item.tour_item = item.tour_item;
                        }
                        break;
                    case "SSE":
                        var sse = _seeTempRepository.Find(x => x.Code == item.Code);
                        var diemtq = _diemtqRepository.GetById(item.tour_item);
                        string tq = "";
                        if (diemtq != null)
                        {
                            tq = diemtq.Diemtq;
                        }
                        if (sse.Count() > 0)
                        {
                            string tendtq = "";
                            foreach (var d in sse)
                            {
                                if (tendtq == "")
                                    tendtq = _diemtqRepository.GetById(d.Codedtq).Diemtq;
                                else
                                    tendtq += ", " + _diemtqRepository.GetById(d.Codedtq).Diemtq;
                            }
                            item.tour_item = "Tham quan " + (string.IsNullOrEmpty(tq) ? "" : tq) + "  " + tendtq;
                        }
                        else
                        {
                            item.tour_item = tq;
                        }
                        
                        break;
                    default:
                        item.tour_item = item.tour_item;
                        break;
                }
                
            }
            return PartialView("listTourProgTemp",progtemp);
        }
        // Thêm các dịch vụ cho chương trình tour

        public ActionResult AddTourSrvProgTemp(string id)
        {
            var a = new TourProgTemp();
            a.Code = id;
            a.stt = _tourProgTempRepository.newSttTourProgTemp(id);
            a.date = 0;
            a.vatin = 10;
            a.vatout = 10;
            listDichvu("TXT");
            listNgoaite("VND");
            listHanhtrinhTu("");
            listHanhtrinhDen("");
            return PartialView("AddTourSrvProgTemp",a);
        }
        [HttpPost]
        public ActionResult AddTourSrvProgTemp([Bind ("Code,stt,date,tour_item,srvnode,srvtype,currency,vatin,vatout,chinhanh")] TourProgTemp tourProgTemp)
        {
            tourProgTemp.chinhanh = HttpContext.Session.GetString("chinhanh");
            var result =_tourProgTempRepository.Create(tourProgTemp);
            if (result != null && (tourProgTemp.srvtype == "HTL"|| tourProgTemp.srvtype == "CRU"))               
            {
                Hoteltemp h = new Hoteltemp();
                h.Code = tourProgTemp.Code;
                h.stt = tourProgTemp.stt;
                h.currency = "VND";
                h.note = "";              
                AddHotelTemp(h);
            }
            //return Json(true);
            return Json(JsonConvert.SerializeObject(result));
        }
       // Xoá các dịch vụ của chương trình tour
        [HttpPost]
        public ActionResult DelTourSrvProgTemp(decimal id)
        {
            bool status = true;
            var tourProgTemp = _tourProgTempRepository.GetById(id);
            if (tourProgTemp == null)
            {
                return NotFound();
            }
            var result = _tourProgTempRepository.Delete(tourProgTemp);
            if (result == null)
            {
                status = false;
            }
            return Json(status);
        }
        public IActionResult ngayExists(string sgtcode, int ngay)
        {
            bool result = false;
            var u =_tourProgTempRepository.Find(x => x.Code == sgtcode && x.date == ngay).FirstOrDefault();
            if (ngay == 0 || u == null)
                result = true;

            return Json(result);
        }
        public int newDateTourProgTemp(string sgtcode)
        {
            return _tourProgTempRepository.newDateTourProg(sgtcode);
        }
        #endregion
        #region Thao tác với dịch vụ LUN


        //---------- Cập nhật dịch vụ ăn trưa trong chương trình tour-----------
        public ActionResult EditLunchProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            switch (tourprog.srvtype)
            {
                case "BRK":
                    ViewBag.tieude = "Ăn sáng tại nhà hàng";
                    break;
                case "LUN":
                    ViewBag.tieude = "Ăn trưa tại nhà hàng";
                    break;
                case "DIN":
                    ViewBag.tieude = "Ăn tối tại nhà hàng";
                    break;
                default:
                    ViewBag.tieude = "Ăn tại nhà hàng";
                    break;
            }
            return PartialView("EditLunchProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditLunchProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            //a.Code = tourProgTemp.Code;
            //a.stt = tourProgTemp.stt;
            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode.ToUpper();
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.amount = tourProgTemp.amount;
            a.debit = tourProgTemp.debit;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ OVR (oversea), WPU, TRS (vận chuyển khác), Các dịch vụ còn lại


        //---------- Cập nhật dịch vụ over sea trong chương trình tour-----------
        public ActionResult EditOverSeaProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            if (tourprog.srvtype == "OVR")
            {
                ViewBag.ghichu = "Ghi chú nối tour";
                ViewBag.cartitle = "CHI TIẾT VỀ NỐI TOUR QUỐC TẾ";
                ViewBag.legend = "Đơn vị bán tour";
                ViewBag.donvi = "Đơn vị";
            }
            else if (tourprog.srvtype == "WPU")
            {
                ViewBag.ghichu = "Ghi chú múa rối nước";
                ViewBag.cartitle = "CHI TIẾT VỀ MÚA RỐI NƯỚC";
                ViewBag.legend = "Thông tin về nhóm múa rối nước";
                ViewBag.donvi = "Nhóm";
            }
            else if (tourprog.srvtype == "TRS")
            {
                ViewBag.ghichu = "Ghi chú dịch vụ";
                ViewBag.cartitle = "CHI TIẾT VỀ CÁC DỊCH VỤ VẬN CHUYỂN KHÁC";
                ViewBag.legend = "Thông tin nơi thuê xích lô, xe đạp...";
                ViewBag.donvi = "Diễn giải";
            }
            else
            {
                var dichvu = _dichvuRepository.GetById(tourprog.srvtype);

                ViewBag.ghichu = "Ghi chú dịch vụ";
                ViewBag.cartitle = "CHI TIẾT VỀ  DỊCH VỤ " + dichvu.Tendv;
                ViewBag.legend = "Thông tin dịch vụ";
                ViewBag.donvi = "Đơn vị";
            }
            return PartialView("EditOverSeaProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditOverSeaProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            //a.Code = tourProgTemp.Code;
            //a.stt = tourProgTemp.stt;
            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode.ToUpper();
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.amount = tourProgTemp.amount;
            a.debit = tourProgTemp.debit;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với hành trình Itinerary
        public ActionResult EditItineraryProg(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            var t = _tourTempNoteRepository.GetById(tourprog.Code);
            listHanhtrinhTu(tourprog.tour_item);
            listHanhtrinhDen(tourprog.srvnode);

            return PartialView("EditItineraryProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditItineraryProg(TourProgTemp tourProgTemp)
        {
           
            var a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            a.Code = tourProgTemp.Code;
            a.stt = tourProgTemp.stt;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ Train
        public ActionResult EditTrainProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listAirType(tourprog.airtype);
            listNgoaite(tourprog.currency);
            listSrvcode(tourprog.srvcode);
            return PartialView("EditTrainProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditTrainProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.currency = tourProgTemp.currency;
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.arr = string.IsNullOrEmpty(tourProgTemp.arr) ? "" : tourProgTemp.arr.ToUpper();
            a.dep = string.IsNullOrEmpty(tourProgTemp.dep) ? "" : tourProgTemp.dep.ToUpper();
            a.debit = tourProgTemp.debit;
            a.carrier = tourProgTemp.carrier;
            a.airtype = tourProgTemp.airtype;
            //a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ PAC

        //---------- Cập nhật dịch vụ tour trọn gói trong chương trình tour-----------
        public ActionResult EditPacProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            return PartialView("EditPacProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditPacProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            //a.Code = tourProgTemp.Code;
            //a.stt = tourProgTemp.stt;
            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode.ToUpper();
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.amount = tourProgTemp.amount;
            a.debit = tourProgTemp.debit;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ TXT


        // Cập nhật dịch vụ text của chương trình tour
        public ActionResult EditTextProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            if (tourprog.srvtype == "TXT")
            {
                ViewBag.ghichu = "Ghi chú";
                ViewBag.cartitle = "ĐIỀU CHỈNH TEXT";
                ViewBag.legend = "Nhập câu text mới để làm rõ nghĩa tour";
                ViewBag.tour_item = "Nhập text";
            }
            else if (tourprog.srvtype == "GUI")
            {
                ViewBag.ghichu = "Ghi chú về hướng dẫn";
                ViewBag.cartitle = "CHI TIẾT VỀ HƯỚNG DẪN";
                ViewBag.legend = "Yêu cầu về hướng dẫn";
                ViewBag.tour_item = "Yêu cầu";
            }
            return PartialView("EditTextProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditTextProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            a.Code = tourProgTemp.Code;
            a.stt = tourProgTemp.stt;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item)?"":tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode)?"":tourProgTemp.srvnode;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ OTH


        //---------- Cập nhật dịch vụ tour trọn gói trong chương trình tour-----------
        public ActionResult EditOtherProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            return PartialView("EditOtherProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditOtherProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            //a.Code = tourProgTemp.Code;
            //a.stt = tourProgTemp.stt;
            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.unitpricea =  tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.amount = tourProgTemp.amount;
            a.debit = tourProgTemp.debit;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ HTL trong chương trình tour


        //---------- Cập nhật dịch vụ hotel trong chương trình tour-----------
        public ActionResult EditHotelProgTemp(decimal id)
        {
            TourprogHotelTempViewModel t = new TourprogHotelTempViewModel();
            var tourprog = _tourProgTempRepository.GetById(id);
            t.TourProgTemp = tourprog;
            listSupplier(tourprog.supplierid);
            listSrvcode(t.TourProgTemp.srvcode);
            listNgoaite(t.TourProgTemp.currency);
            ViewBag.Code = t.TourProgTemp.Code;
            ViewBag.stt = t.TourProgTemp.stt;
           // ViewBag.listhotel = _hotelTempRepository.Find(x => x.Code == t.TourProgTemp.Code && x.stt == t.TourProgTemp.stt);
            Hoteltemp h = _hotelTempRepository.Find(x => x.Code == t.TourProgTemp.Code && x.stt == t.TourProgTemp.stt).FirstOrDefault();
            t.Hoteltemp = h;
            return PartialView("EditHotelProgTemp", t);
        }
        [HttpPost]
        public ActionResult EditHotelProgTemp(TourprogHotelTempViewModel tourProgHotelTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgHotelTemp.TourProgTemp.Id);
            a.Id = tourProgHotelTemp.TourProgTemp.Id;
            //a.Code = tourProgTemp.Code;
            //a.stt = tourProgTemp.stt;
            a.supplierid = tourProgHotelTemp.TourProgTemp.supplierid;
            a.srvcode = tourProgHotelTemp.TourProgTemp.srvcode;
            a.currency = tourProgHotelTemp.TourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgHotelTemp.TourProgTemp.tour_item) ? "" : tourProgHotelTemp.TourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgHotelTemp.TourProgTemp.srvnode) ? "" : tourProgHotelTemp.TourProgTemp.srvnode.ToUpper();
            a.unitpricea = tourProgHotelTemp.TourProgTemp.unitpricea;
            a.unitpricec = tourProgHotelTemp.TourProgTemp.unitpricec;
            a.amount = tourProgHotelTemp.TourProgTemp.amount;
            a.vatin = tourProgHotelTemp.TourProgTemp.vatin;
            a.vatout = tourProgHotelTemp.TourProgTemp.vatout;
            a.amount = tourProgHotelTemp.TourProgTemp.amount;
            a.debit = tourProgHotelTemp.TourProgTemp.debit;
            _tourProgTempRepository.Update(a);
            EditHotelTemp(tourProgHotelTemp.Hoteltemp);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ CAR, CAG
        // Cập nhật dịch vụ text của chương trình tour
        public ActionResult EditCarProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            if (tourprog.srvtype == "CAR")
            {
                ViewBag.ghichu = "Ghi chú";
                ViewBag.cartitle = "CHI TIẾT VỀ XE";
            }
            else
            {
                ViewBag.ghichu = "Ghi chú về loại xe và ngoại ngữ HD";
                ViewBag.cartitle = "CHI TIẾT VỀ XE VÀ HƯỚNG DẪN";
            }
            return PartialView("EditCarProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditCarProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            a.Code = tourProgTemp.Code;
            a.stt = tourProgTemp.stt;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ AIR
        public ActionResult EditAirProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listAirType(tourprog.airtype);
            listNgoaite(tourprog.currency);
            listSrvcode(tourprog.srvcode);
            return PartialView("EditAirProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditAirProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;          
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.currency = tourProgTemp.currency;
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.arr = string.IsNullOrEmpty(tourProgTemp.arr)?"":tourProgTemp.arr.ToUpper();
            a.dep = string.IsNullOrEmpty(tourProgTemp.dep)?"":tourProgTemp.dep.ToUpper();
            a.debit = tourProgTemp.debit;
            a.carrier = tourProgTemp.carrier;
            a.airtype = tourProgTemp.airtype;
            //a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ SHP (shopping)
        public ActionResult EditShopProgTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            return PartialView("EditShopProgTemp", tourprog);
        }
        [HttpPost]
        public ActionResult EditShopProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode.ToUpper();
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.amount = tourProgTemp.amount;
            a.debit = tourProgTemp.debit;
            _tourProgTempRepository.Update(a);
            return Json(true);
        }
        #endregion

        #region Thao tác với dịch vụ SSE

        public void listDiemtq(string selected)
        {
            List<Dmdiemtq> lst = _diemtqRepository.GetLstDiemtq().ToList();
            var empty = new Dmdiemtq
            {
                Code = "",
                Diemtq = "-- Chọn thông tin --"
            };
            lst.Insert(0, empty);
            ViewBag.dtq = new SelectList(lst, "Code", "Diemtq", selected);
        }

        public ActionResult EditSightseeingTemp(decimal id)
        {
            var tourprog = _tourProgTempRepository.GetById(id);
            listDiemtq("");
            listSrvcode(tourprog.srvcode == null ? "" : tourprog.srvcode);
            listHttt("");

            List<SightseeingTemp> lst = _seeTempRepository.GetByCodeAndStt(tourprog.Code, (int)tourprog.stt);
            var model = new List<SightseeingTempViewModel>();
            string sDiemtq = "", sTengiaodich = "";
            foreach (SightseeingTemp s in lst)
            {
                SightseeingTempViewModel m = new SightseeingTempViewModel();
                m.SightseeingTemp = s;

                Dmdiemtq dtq = _diemtqRepository.GetById(s.Codedtq);
                if (dtq != null) sDiemtq = dtq.Diemtq;
                m.Diemtq = sDiemtq;

                Supplier sup = _supplierRepository.GetById(s.Debit);
                if (sup != null) sTengiaodich = sup.Tengiaodich;
                m.Tengiaodich = sTengiaodich;

                model.Add(m);
            }

            ViewBag.listsee = model;

            return PartialView("EditSightseeingTemp", tourprog);
        }

        /// <summary>
        /// Code:    stt
        /// </summary>
        /// <param name="tourProgTemp"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSightseeingProgTemp(TourProgTemp tourProgTemp)
        {
            TourProgTemp a = _tourProgTempRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = "VND";// tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode.ToUpper();
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.amount = tourProgTemp.amount;
            a.debit = tourProgTemp.debit;                 
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            _tourProgTempRepository.Update(a);

            return Json(true);
        }

        //Cap nhat THam quan Temp
        [HttpPost]
        public ActionResult AddSeetemp(SightseeingTemp entity)
        {
            var see = new SightseeingTemp();
            see.Code = entity.Code;
            see.Stt = entity.Stt;
            see.Codedtq = entity.Codedtq;
            see.Serial = entity.Serial;
            //   see.Debit = entity.Debit;
            see.PaxPrice = string.IsNullOrEmpty(entity.PaxPrice.ToString()) ? 0 : entity.PaxPrice;
            
            see.ChildernPrice = string.IsNullOrEmpty(entity.ChildernPrice.ToString()) ? 0 : entity.ChildernPrice;
            //   see.Amount = see.Unitprice +  see.Unipricev* see.Paxv;// string.IsNullOrEmpty(entity.Amount.ToString()) ? 0 : entity.Amount;
            see.Vatin = string.IsNullOrEmpty(entity.Vatin.ToString()) ? 0 : entity.Vatin;
            see.Vatout = string.IsNullOrEmpty(entity.Vatout.ToString()) ? 0 : entity.Vatout;
            see.httt = entity.httt;
            see.chinhanh = HttpContext.Session.GetString("chinhanh");
            _seeTempRepository.Create(see);
            return Json(see);
        }

        [HttpPost]
        public ActionResult EditSeetemp(SightseeingTemp entity)
        {
            var see = _seeTempRepository.GetById(entity.Id);
            see.Code = entity.Code;
            see.Stt = entity.Stt;
            see.Codedtq = entity.Codedtq;
            see.Serial = entity.Serial;
            //  see.Debit = entity.Debit;
            see.PaxPrice = string.IsNullOrEmpty(entity.PaxPrice.ToString()) ? 0 : entity.PaxPrice;
            //  see.Paxv = string.IsNullOrEmpty(entity.Paxv.ToString()) ? 0 : entity.Paxv;
            see.ChildernPrice = string.IsNullOrEmpty(entity.ChildernPrice.ToString()) ? 0 : entity.ChildernPrice;
            //  see.Amount = string.IsNullOrEmpty(entity.Amount.ToString()) ? 0 : entity.Amount;
            see.Vatin = string.IsNullOrEmpty(entity.Vatin.ToString()) ? 0 : entity.Vatin;
            see.Vatout = string.IsNullOrEmpty(entity.Vatout.ToString()) ? 0 : entity.Vatout;
            see.httt = entity.httt;
            see.chinhanh = HttpContext.Session.GetString("chinhanh");
            _seeTempRepository.Update(see);
            return new EmptyResult();
        }

        //Xoa hotel temp
        [HttpPost]
        public ActionResult DelSeeTemp(decimal id)
        {
            var seetemp = _seeTempRepository.GetById(id);
            if (seetemp == null)
            {
                return NotFound();
            }
            var result = _seeTempRepository.Delete(seetemp);
            return Json(true);
        }

        #endregion

        #region Thoa tac voi HotelTemp


        //Cap nhat Hotel Temp
        [HttpPost]
        public Hoteltemp EditHotelTemp(Hoteltemp entity)
        {
            var hotel = _hotelTempRepository.GetById(entity.Id);
            hotel.currency = string.IsNullOrEmpty( entity.currency)?"VND":entity.currency;
            hotel.note = entity.note;
            hotel.sgl = string.IsNullOrEmpty(entity.sgl.ToString()) ? 0 : entity.sgl;
            hotel.sglpax = string.IsNullOrEmpty(entity.sglpax.ToString()) ? 0 : entity.sglpax;
            hotel.sglcost = string.IsNullOrEmpty(entity.sglcost.ToString()) ? 0 : entity.sglcost;
           // hotel.sglfoc = string.IsNullOrEmpty(entity.sglfoc.ToString()) ? 0 : entity.sglfoc;
            hotel.extsgl = string.IsNullOrEmpty(entity.extsgl.ToString()) ? 0 : entity.extsgl;
            hotel.extsglcost = string.IsNullOrEmpty(entity.extsglcost.ToString()) ? 0 : entity.extsglcost;

            hotel.dbl = string.IsNullOrEmpty(entity.dbl.ToString()) ? 0 : entity.dbl;
            hotel.dblpax = string.IsNullOrEmpty(entity.dblpax.ToString()) ? 0 : entity.dblpax;
            hotel.dblcost = string.IsNullOrEmpty(entity.dblcost.ToString()) ? 0 : entity.dblcost;
          //  hotel.dblfoc = string.IsNullOrEmpty(entity.dblfoc.ToString()) ? 0 : entity.dblfoc;
            hotel.extdbl = string.IsNullOrEmpty(entity.extdbl.ToString()) ? 0 : entity.extdbl;
            hotel.extdblcost = string.IsNullOrEmpty(entity.extdblcost.ToString()) ? 0 : entity.extdblcost;

            hotel.twn = string.IsNullOrEmpty(entity.twn.ToString()) ? 0 : entity.twn;
            hotel.twnpax = string.IsNullOrEmpty(entity.twnpax.ToString()) ? 0 : entity.twnpax;
            hotel.twncost = string.IsNullOrEmpty(entity.twncost.ToString()) ? 0 : entity.twncost;
           // hotel.twnfoc = string.IsNullOrEmpty(entity.twnfoc.ToString()) ? 0 : entity.twnfoc;
            hotel.exttwn = string.IsNullOrEmpty(entity.exttwn.ToString()) ? 0 : entity.exttwn;
            hotel.exttwncost = string.IsNullOrEmpty(entity.exttwncost.ToString()) ? 0 : entity.exttwncost;

            hotel.homestay = string.IsNullOrEmpty(entity.homestay.ToString()) ? 0 : entity.homestay;
            hotel.homestaypax = string.IsNullOrEmpty(entity.homestaypax.ToString()) ? 0 : entity.homestaypax;
            hotel.homestaycost = string.IsNullOrEmpty(entity.homestaycost.ToString()) ? 0 : entity.homestaycost;
           // hotel.tplfoc = string.IsNullOrEmpty(entity.tplfoc.ToString()) ? 0 : entity.tplfoc;
            hotel.homestaynote = string.IsNullOrEmpty(entity.homestaynote) ? "" : entity.homestaynote;

            hotel.oth = string.IsNullOrEmpty(entity.oth.ToString()) ? 0 : entity.oth;
            hotel.othpax = string.IsNullOrEmpty(entity.othpax.ToString()) ? 0 : entity.othpax;
            hotel.othcost = string.IsNullOrEmpty(entity.othcost.ToString()) ? 0 : entity.othcost;
            hotel.othtype = entity.othtype;
            _hotelTempRepository.Update(hotel);
            return hotel;
        }
        //Cap nhat Hotel Temp
        [HttpPost]
        //public ActionResult AddHotelTemp(Hoteltemp entity)
        public Hoteltemp AddHotelTemp(Hoteltemp entity)
        {
            var hotel = new Hoteltemp();
            hotel.Code = entity.Code;
            hotel.stt = entity.stt;
            hotel.currency = string.IsNullOrEmpty(entity.currency) ? "VND" : entity.currency.ToUpper();
            hotel.note = entity.note;
            hotel.sgl = string.IsNullOrEmpty(entity.sgl.ToString()) ? 0 : entity.sgl;
            hotel.sglpax = string.IsNullOrEmpty(entity.sglpax.ToString()) ? 0 : entity.sglpax;
            hotel.sglcost = string.IsNullOrEmpty(entity.sglcost.ToString()) ? 0 : entity.sglcost;
           // hotel.sglfoc = string.IsNullOrEmpty(entity.sglfoc.ToString()) ? 0 : entity.sglfoc;
            hotel.extsgl = string.IsNullOrEmpty(entity.extsgl.ToString()) ? 0 : entity.extsgl;
            hotel.extsglcost = string.IsNullOrEmpty(entity.extsglcost.ToString()) ? 0 : entity.extsglcost;

            hotel.dbl = string.IsNullOrEmpty(entity.dbl.ToString()) ? 0 : entity.dbl;
            hotel.dblpax = string.IsNullOrEmpty(entity.dblpax.ToString()) ? 0 : entity.dblpax;
            hotel.dblcost = string.IsNullOrEmpty(entity.dblcost.ToString()) ? 0 : entity.dblcost;
          //  hotel.dblfoc = string.IsNullOrEmpty(entity.dblfoc.ToString()) ? 0 : entity.dblfoc;
            hotel.extdbl = string.IsNullOrEmpty(entity.extdbl.ToString()) ? 0 : entity.extdbl;
            hotel.extdblcost = string.IsNullOrEmpty(entity.extdblcost.ToString()) ? 0 : entity.extdblcost;

            hotel.twn = string.IsNullOrEmpty(entity.twn.ToString()) ? 0 : entity.twn;
            hotel.twnpax = string.IsNullOrEmpty(entity.twnpax.ToString()) ? 0 : entity.twnpax;
            hotel.twncost = string.IsNullOrEmpty(entity.twncost.ToString()) ? 0 : entity.twncost;
           // hotel.twnfoc = string.IsNullOrEmpty(entity.twnfoc.ToString()) ? 0 : entity.twnfoc;
            hotel.exttwn = string.IsNullOrEmpty(entity.exttwn.ToString()) ? 0 : entity.exttwn;
            hotel.exttwncost = string.IsNullOrEmpty(entity.exttwncost.ToString()) ? 0 : entity.exttwncost;

            hotel.homestay = string.IsNullOrEmpty(entity.homestay.ToString()) ? 0 : entity.homestay;
            hotel.homestaypax = string.IsNullOrEmpty(entity.homestaypax.ToString()) ? 0 : entity.homestaypax;
            hotel.homestaycost = string.IsNullOrEmpty(entity.homestaycost.ToString()) ? 0 : entity.homestaycost;
            hotel.homestaynote = string.IsNullOrEmpty(entity.homestaynote) ? "" : entity.homestaynote;
         
            hotel.oth = string.IsNullOrEmpty(entity.oth.ToString()) ? 0 : entity.oth;
            hotel.othpax = string.IsNullOrEmpty(entity.othpax.ToString()) ? 0 : entity.othpax;
            hotel.othcost = string.IsNullOrEmpty(entity.othcost.ToString()) ? 0 : entity.othcost;
            hotel.othtype = entity.othtype;
            _hotelTempRepository.Create(hotel);
            //return Json(hotel);
            return hotel;
        }
        //Xoa hotel temp
        [HttpPost]
        public ActionResult DelHotelTemp(decimal id)
        {
            var hoteltemp = _hotelTempRepository.GetById(id);
            if (hoteltemp == null)
            {
                return NotFound();
            }
            var result = _hotelTempRepository.Delete(hoteltemp);           
            return Json(true);
        }
        #endregion
     
        #region Thao tac ghi chu dau tour, cuoi tour 


        public ActionResult AddEditTourTempNote(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tournote = _tourTempNoteRepository.GetById(id);
            return PartialView("AddEditTourTempNote", tournote);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddEditTourTempNote(TourTempNote tourTempNote)   
        {
            var temp = _tourTempNoteRepository.GetSingleNoTracking(x=>x.Code==tourTempNote.Code);
            var result = new object();
            if (temp == null)
            {
                result= _tourTempNoteRepository.Create(tourTempNote);               
            }
            else
            {
                result= _tourTempNoteRepository.Update(tourTempNote);                
            }
            //if (result != null)
            //{
            //    SetAlert("Cập nhật tour Note thành công", "success");
            //}
            //else
            //{
            //    SetAlert("Cập nhật tour Note không thành công", "error");
            //}
            return Json(result);
        }

        #endregion
        #region Thao tac keo thả trên danh sách tour program template

        
        [HttpPost]
        // public JsonResult Updatevitrinew([FromBody]string[] sortedIDs)

        public JsonResult CapNhatSTT([FromBody]string[] sortedIDs)
        {
            string lst = "";

            try
            {

                //cap nhap lai vi tri
                for (int i = 0; i < sortedIDs.Length; i++)
                {
                    decimal id = 0;

                    try
                    {
                        string sId = sortedIDs[i];
                        if (sId.Contains("row_"))
                        {
                            sId = sId.Substring(4, sId.Length - 4);
                        }
                        id = decimal.Parse(sId);
                    }
                    catch
                    {

                    }

                    if (id > 0)
                    {
                        int iIdOld = 0;
                        TourProgTemp d = _tourProgTempRepository.GetById(id);
                        iIdOld = d.stt;
                        d.stt = i + 1;
                        _tourProgTempRepository.Update(d);//update thutu


                        //cap nhat stt vao table dich vu tuong ung
                        switch (d.srvtype)
                        {
                            case "HTL": //hotel
                                List<Hoteltemp> ht = _hotelTempRepository.GetLstHTLByCodeAndOrder(d.Code, iIdOld);

                                foreach (Hoteltemp htl in ht)
                                {
                                    htl.stt = d.stt;//cap nhat stt moi
                                    _hotelTempRepository.Update(htl);
                                }

                                break;
                            case "SSE": //hotel
                                List<SightseeingTemp> see = _seeTempRepository.GetByCodeAndStt(d.Code, iIdOld);

                                foreach (SightseeingTemp s in see)
                                {
                                    s.Stt = d.stt;//cap nhat stt moi
                                    _seeTempRepository.Update(s);
                                }

                                break;
                        }
                    }

                }

                return Json(lst);
            }
            catch //(Exception ex)
            {

                lst = "ERR";
            }

            return Json(lst);

        }
        #endregion
    }
}
