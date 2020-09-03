using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.ViewModel;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.IO;
using OfficeOpenXml;
using Novacode;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting.Internal;
using System.Drawing;
using Microsoft.CodeAnalysis.Operations;

namespace dieuhanhtour.Controllers
{
    public class TourController : BaseController
    {
        private readonly ITourinfRepository _tourinfRepository;
        private readonly IPhongbanRepository _phongbanRepository;
        private readonly ITourTempRepository _tourTempRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ITourkindRepository _tourkindRepository;
        private readonly INgoaiteRepository _ngoaiteRepository;
        private readonly IPasstypeRepository _passtypeRepository;
        private readonly IUserinfoRepository _userinfoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITuyentqRepository _tuyentqRepository;
        private readonly ITourProgTempRepository _tourProgTempRepository;
        private readonly ITourprogRepository _tourprogRepository;
        private readonly IHotelTempRepository _hotelTempRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ISightseeingRepository _sightseeingRepository;
        private readonly ISightseeingTempRepository _sightseeingTempRepository;
        private readonly ITournodeRepository _tournodeRepository;
        private ITourTempNoteRepository _tourTempNoteRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IDiemtqRepository _diemtqRepository;
        private readonly IDichvuRepository _dichvuRepository;
        private readonly IChinhanhRepository _chinhanhRepository;
        private readonly IDieuxeRepository _dieuxeRepository;
        private readonly IHuongdanRepository _huongdanRepository;
        private readonly ILoaixeRepository _loaixeRepository;
        private readonly IKhachTourRepository _khachtourRepository;
        private readonly ILoaiphongRepository _loaiphongRepository;
        private readonly IChiphiRepository _chiphiRepository;
        private readonly IBookedRepository _bookedRepository;
        private readonly IQuocgiaRepository _quocgiaRepository;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly IThanhphoRepository _thanhphoRepository;
        string temp = "", log = "";// profile="";

        public TourController(ITourinfRepository tourinfRepository, IPhongbanRepository phongbanRepository, ITourTempRepository tourTempRepository, ICompanyRepository companyRepository, ITourkindRepository tourkindRepository, INgoaiteRepository ngoaiteRepository, IPasstypeRepository passtypeRepository, IUserinfoRepository userinfoRepository, ITuyentqRepository tuyentqRepository, ITourProgTempRepository tourProgTempRepository, ITourprogRepository tourprogRepository,
                                IHotelTempRepository hotelTempRepository, IHotelRepository hotelRepository, ISightseeingRepository sightseeingRepository, ISightseeingTempRepository sightseeingTempRepository, ITournodeRepository tournodeRepository, ITourTempNoteRepository tourTempNoteRepository, ISupplierRepository supplierRepository, IDiemtqRepository diemtqRepository, IDichvuRepository dichvuRepository, IChinhanhRepository chinhanhRepository, IDieuxeRepository dieuxeRepository,
                                IHuongdanRepository huongdanRepository, ILoaixeRepository loaixeRepository, IKhachTourRepository khachtourRepository, ILoaiphongRepository loaiphongRepository, IChiphiRepository chiphiRepository, IBookedRepository bookedRepository, IQuocgiaRepository quocgiaRepository, HostingEnvironment hostingEnvironment, IUserRepository userRepository, IThanhphoRepository thanhphoRepository)
        {
            _tourinfRepository = tourinfRepository;
            _phongbanRepository = phongbanRepository;
            _tourTempRepository = tourTempRepository;
            _companyRepository = companyRepository;
            _tourkindRepository = tourkindRepository;
            _ngoaiteRepository = ngoaiteRepository;
            _passtypeRepository = passtypeRepository;
            _userinfoRepository = userinfoRepository;
            _userRepository = userRepository;
            _tuyentqRepository = tuyentqRepository;
            _tourProgTempRepository = tourProgTempRepository;
            _tourprogRepository = tourprogRepository;
            _hotelTempRepository = hotelTempRepository;
            _hotelRepository = hotelRepository;
            _sightseeingRepository = sightseeingRepository;
            _sightseeingTempRepository = sightseeingTempRepository;
            _tournodeRepository = tournodeRepository;
            _tourTempNoteRepository = tourTempNoteRepository;
            _supplierRepository = supplierRepository;
            _diemtqRepository = diemtqRepository;
            _dichvuRepository = dichvuRepository;
            _chinhanhRepository = chinhanhRepository;
            _dieuxeRepository = dieuxeRepository;
            _huongdanRepository = huongdanRepository;
            _loaixeRepository = loaixeRepository;
            _khachtourRepository = khachtourRepository;
            _loaiphongRepository = loaiphongRepository;
            _chiphiRepository = chiphiRepository;
            _bookedRepository = bookedRepository;
            _quocgiaRepository = quocgiaRepository;
            _hostingEnvironment = hostingEnvironment;
            _thanhphoRepository = thanhphoRepository;
        }



        // GET: Tour
        public IActionResult Index(string searchString, string chinhanh, string dieuhanh, string fromDate, string toDate, int page = 1)
        {
            searchString = searchString ?? "";// HttpContext.Session.GetString("username");// string.IsNullOrEmpty(searchString) ? "" : searchString;
            chinhanh = chinhanh ?? HttpContext.Session.GetString("chinhanh");
            HttpContext.Session.SetString("urlTour", UriHelper.GetDisplayUrl(Request));
            dieuhanh = "";
            if (HttpContext.Session.GetString("Admin") != "True")
            {
                dieuhanh = HttpContext.Session.GetString("username");
            }
            var tourinf = _tourinfRepository.ListTour(searchString, chinhanh, dieuhanh, Convert.ToBoolean(HttpContext.Session.GetString("khachle")), Convert.ToBoolean(HttpContext.Session.GetString("khachdoan")), fromDate, toDate, page);
            ViewData["CurrentFilter"] = searchString;
            ViewBag.tourinf = tourinf;
            ViewBag.chinhanh = chinhanh ?? HttpContext.Session.GetString("chinhanh");
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            ViewBag.count = tourinf.Count();
            foreach (var item in tourinf)
            {
                if (item.departoperator != null)
                {
                    var phongban = _phongbanRepository.getPhongbanById(item.departoperator);
                    item.departoperator = phongban.tenphong;
                }
                else
                {
                    item.departoperator = item.departoperator;
                }
                var userinfo = _userinfoRepository.GetUserByUsername(item.concernto);
                if (userinfo != null)
                {
                    item.concernto = userinfo.hoten;
                }
                else
                {
                    item.concernto = item.concernto;
                }

            }
            return View(tourinf);

        }

        public IActionResult ListTourNoOperator(string chinhanh, int page = 1)
        {

            chinhanh = chinhanh ?? HttpContext.Session.GetString("chinhanh");
            HttpContext.Session.SetString("urlTour", UriHelper.GetDisplayUrl(Request));

            var tourinf = _tourinfRepository.ListTourNoOperator(chinhanh, Convert.ToBoolean(HttpContext.Session.GetString("khachle")), Convert.ToBoolean(HttpContext.Session.GetString("khachdoan")), page);
            ViewBag.tourinf = tourinf;
            ViewBag.chinhanh = HttpContext.Session.GetString("chinhanh");
            // ViewBag.chinhanh = chinhanh ?? HttpContext.Session.GetString("chinhanh");
            foreach (var item in tourinf)
            {
                if (item.departoperator != null)
                {
                    var phongban = _phongbanRepository.getPhongbanById(item.departoperator);
                    item.departoperator = phongban.tenphong;
                }
                else
                {
                    item.departoperator = item.departoperator;
                }
                var userinfo = _userinfoRepository.GetUserByUsername(item.concernto);
                if (userinfo != null)
                {
                    item.concernto = userinfo.hoten;
                }
                else
                {
                    item.concernto = item.concernto;
                }
            }
            return View(tourinf);
        }
        // GET: Tour/Create
        #region Thêm tour
        public IActionResult Create()
        {
            var tourinf = new CreateTourModel();
            // string chinhanh = HttpContext.Session.GetString("chinhanh");
            // string macode = HttpContext.Session.GetString("macode");
            // tourinf.sgtcode = _tourinfRepository.newSgtcode(System.DateTime.Now, chinhanh, macode);
            tourinf.rate = 1;
            listCompany("");
            listTourKind("");
            listTourTemp("");
            listNgoaite("VND");
            listPhongban("DH");
            listPassType("");
            listTuyentq("");
            listVisa("");
            listMacode();
            listChinhanh(HttpContext.Session.GetString("chinhanh"));
            return View(tourinf);
        }

        // POST: Tour/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateTourModel entity)
        {
            string chinhanh = HttpContext.Session.GetString("chinhanh");
            //string macode = HttpContext.Session.GetString("macode");
            if (string.IsNullOrEmpty(entity.macode))
            {
                SetAlert("Bạn không có quyền tạo tour", "error");
                return View(entity);
            }
            string username = HttpContext.Session.GetString("username");

            if (ModelState.IsValid)
            {
                Tourinf t = new Tourinf();
                t.sgtcode = _tourinfRepository.newSgtcode(entity.arr.HasValue ? Convert.ToDateTime(entity.arr) : System.DateTime.Now, chinhanh, entity.macode);
                t.tourkindId = entity.tourkindId;
                t.companyId = entity.companyId;
                t.arr = Convert.ToDateTime(entity.arr);
                t.dep = Convert.ToDateTime(entity.dep);
                t.pax = entity.pax;
                t.childern = entity.childern;
                t.reference = string.IsNullOrEmpty(entity.reference) ? "" : entity.reference;
                t.departoperator = entity.departoperator;
                t.departcreate = HttpContext.Session.GetString("phong");
                t.operators = "";
                t.concernto = username;
                t.routing = string.IsNullOrEmpty(entity.routing) ? "" : entity.routing;
                t.passtypeId = entity.passtype;
                t.visa = string.IsNullOrEmpty(entity.visa) ? "" : entity.visa;
                t.revenue = entity.revenue;
                t.currency = entity.currency;
                t.rate = entity.rate;
                t.chinhanh = entity.chinhanh;
                t.chinhanhtao = chinhanh;
                t.createtour = System.DateTime.Now;
                t.khachle = Convert.ToBoolean(HttpContext.Session.GetString("khachle"));
                t.logfile = "-User tạo tour: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                _tourinfRepository.Create(t);
                // ------- Thêm tour note từ template vào trong tour node
                var tourtempNote = _tourTempNoteRepository.GetById(entity.codetemp);
                if (tourtempNote != null)
                {
                    Tournode tnode = new Tournode();
                    tnode.Sgtcode = t.sgtcode;
                    tnode.Headernode = tourtempNote.Header;
                    tnode.Footernode = tourtempNote.Footer;
                    _tournodeRepository.Create(tnode);
                }
                else
                {
                    Tournode tnode = new Tournode();
                    tnode.Sgtcode = t.sgtcode;
                    tnode.Headernode = "";
                    tnode.Footernode = "";
                    _tournodeRepository.Create(tnode);
                }

                // ----------Kiểm tra nếu có chương trình tour tạm thì thêm vào trong chương trình tour -----------------
                var tourprogtemp = _tourProgTempRepository.Find(x => x.Code == entity.codetemp).ToList();
                if (tourprogtemp != null)
                {
                    foreach (var item in tourprogtemp)
                    {
                        Tourprog tp = new Tourprog();
                        tp.sgtcode = t.sgtcode;
                        tp.stt = item.stt;
                        tp.date = item.date;
                        tp.time = item.time;
                        tp.pax = t.pax;
                        tp.childern = t.childern;
                        tp.srvtype = item.srvtype;
                        tp.supplierid = item.supplierid;
                        tp.srvcode = item.srvcode;
                        tp.tour_item = item.tour_item;
                        tp.srvnode = item.srvnode;
                        tp.pickuptime = item.pickuptime;
                        tp.currency = item.currency;
                        tp.arr = item.arr;
                        tp.dep = item.dep;
                        tp.carrier = item.carrier;
                        tp.airtype = item.airtype;
                        tp.unitpricea = item.unitpricea;
                        tp.unitpricec = item.unitpricec;
                        tp.foc = item.foc;
                        tp.carguide = item.carguide;
                        tp.amount = item.amount;
                        tp.debit = item.debit;
                        tp.vatin = item.vatin;
                        tp.vatout = item.vatout;
                        tp.status = "WL";
                        tp.logfile = " User tạo chương trình tour: " + username + " loại: " + item.srvtype + " vào lúc: " + System.DateTime.Now.ToString();
                        tp.chinhanh = chinhanh;
                        tp.dieuhanh = "";
                        _tourprogRepository.Create(tp);
                    }
                    // ----------Kiểm tra nếu có hotel tạm thì thêm vào trong hotel -----------------
                    var htltemp = _hotelTempRepository.Find(x => x.Code == entity.codetemp).ToList();
                    if (htltemp != null)
                    {
                        foreach (var h in htltemp)
                        {
                            Hotel htl = new Hotel();
                            htl.sgtcode = t.sgtcode;
                            htl.stt = h.stt;
                            htl.sgl = h.sgl;
                            htl.sglpax = 0;
                            htl.sglcost = h.sglcost;
                            htl.extsgl = 0;
                            htl.extsglcost = h.extsglcost;
                            htl.dbl = h.dbl;
                            htl.dblpax = 0;
                            htl.dblcost = h.dblcost;
                            htl.extdbl = 0;
                            htl.extdblcost = h.extdblcost;
                            htl.twn = h.twn;
                            htl.twnpax = 0;
                            htl.twncost = h.twncost;
                            htl.exttwn = h.exttwn;
                            htl.exttwncost = h.exttwncost;
                            htl.homestay = h.homestay;
                            htl.homestaypax = 0;
                            htl.homestaycost = h.homestaycost;
                            htl.homestaynote = h.homestaynote;
                           
                            htl.oth = h.oth;
                            htl.othcost = h.othcost;
                            htl.othpax = 0;
                            htl.othtype = h.othtype;
                            htl.currency = h.currency;
                            htl.note = h.note;
                            htl.logfile = "User tạo hotel: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                            _hotelRepository.Create(htl);
                        }
                    }

                    // ----------Kiểm tra nếu Sightseeing tạm thì thêm vào trong Sightseeing-----------------
                    var sstemp = _sightseeingTempRepository.Find(x => x.Code == entity.codetemp).ToList();
                    if (sstemp != null)
                    {
                        foreach (var s in sstemp)
                        {
                            Sightseeing ss = new Sightseeing();
                            ss.sgtcode = t.sgtcode;
                            ss.Stt = s.Stt;
                            ss.Codedtq = s.Codedtq;
                            ss.Serial = s.Serial;
                            ss.Debit = s.Debit;
                            ss.Pax = t.pax;
                            ss.PaxPrice = s.PaxPrice;
                            ss.Childern = t.childern;
                            ss.ChildernPrice = s.ChildernPrice;
                            ss.Amount = s.Amount;
                            ss.Vatin = s.Vatin;
                            ss.Vatout = s.Vatout;
                            ss.Chinhanh = s.chinhanh;
                            ss.Httt = s.httt;
                            // ss.del = false;
                            ss.logfile = "User tạo sightseeing: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                            _sightseeingRepository.Create(ss);
                        }
                    }

                }
                return RedirectToAction("Edit", new { id = t.sgtcode });
            }
            return View();
        }
        #endregion


        // GET: Tour/Edit/5
        #region Sửa tour
        public async Task<IActionResult> Edit(string id)
        {
            HttpContext.Session.SetString("urlEditTour", UriHelper.GetDisplayUrl(Request));
            if (id == null)
            {
                return NotFound();
            }
            var tourinf = await _tourinfRepository.GetByIdAsync(id);// _context.Tourinf.FindAsync(id);

            if (tourinf == null)
            {
                return NotFound();
            }
            if (tourinf.cancel.HasValue)
            {
                SetAlert("Tour này đã huỷ.", "error");
                return Redirect(HttpContext.Session.GetString("urlTour"));
            }
            listCompany(tourinf.companyId);
            listTourKind(tourinf.tourkindId.ToString());
            listTourTemp("");//khi edit khong cho doi tourtemplate
            listNgoaite(tourinf.currency);
            listPhongban(tourinf.departoperator);
            listPassType(tourinf.passtypeId);
            listTuyentq(tourinf.routing);
            listVisa(tourinf.visa);
            listChinhanh(tourinf.chinhanh);
            // listUser(tourinf.departoperator, tourinf.operators);
            // listUserByChinhanh(tourinf.chinhanh, tourinf.operators);
            listDieuhanh(tourinf.departoperator, Convert.ToBoolean(HttpContext.Session.GetString("khachle")), tourinf.chinhanh, tourinf.operators);
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.role = HttpContext.Session.GetString("roleId");
            ViewBag.tourcn = tourinf.chinhanh;
            ViewBag.cn = HttpContext.Session.GetString("chinhanh");
            ViewBag.operators = tourinf.operators;
            // ViewBag.trienkhai = tourinf.chinhanh;
            //var tourProg = _tourprogRepository.Find(x => x.sgtcode == tourinf.sgtcode).ToList();
            //ViewBag.tourProgcount = tourProg.Count();
            return View(tourinf);

        }

        // POST: Tour/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult Edit(Tourinf tourinf, string codetemp, string trienkhai)
        {
            temp = ""; log = "";
            //if (id != tourinf.sgtcode)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {

                string username = HttpContext.Session.GetString("username");
                Tourinf t = _tourinfRepository.GetById(tourinf.sgtcode);

                if (t.companyId != tourinf.companyId)
                {
                    temp += String.Format("- Hãng thay đổi: {0}->{1}", t.companyId, tourinf.companyId);
                }
                if (t.tourkindId != tourinf.tourkindId)
                {
                    temp += String.Format("- Loại tour thay đổi: {0}->{1}", t.tourkindId, tourinf.tourkindId);
                }
                if (t.arr != tourinf.arr)
                {
                    temp += String.Format("- Từ ngày thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.arr, tourinf.arr);
                }
                if (t.dep != tourinf.dep)
                {
                    temp += String.Format("- Đến ngày thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", t.dep, tourinf.dep);
                }
                if (t.pax != tourinf.pax)
                {
                    temp += String.Format("- Số khách thay đổi: {0}->{1}", t.pax, tourinf.pax);
                }
                if (t.childern != tourinf.childern)
                {
                    temp += String.Format("- Trẻ em thay đổi: {0}->{1}", t.childern, tourinf.childern);
                }
                if (t.departoperator != tourinf.departoperator)
                {
                    temp += String.Format("- Phòng điều hành thay đổi: {0}->{1}", t.departoperator, tourinf.departoperator);
                }
                if (!String.IsNullOrEmpty(t.operators) != !String.IsNullOrEmpty(tourinf.operators))
                {
                    temp += String.Format("- Nhân viên điều hành thay đổi: {0}->{1}", t.operators, tourinf.operators);
                }
                tourinf.routing = string.IsNullOrEmpty(tourinf.routing) ? "" : tourinf.routing;
                if (t.routing != tourinf.routing)
                {
                    temp += String.Format("- Tuyến tham quan thay đổi: {0}->{1}", t.routing, tourinf.routing);
                }
                if (t.revenue != tourinf.revenue)
                {
                    temp += String.Format("- Doanh thu thay đổi: {0:#,##0.0}->{1:#,##0.0}", t.revenue, tourinf.revenue);
                }
                if (t.currency != tourinf.currency)
                {
                    temp += String.Format("- Loại tiền thay đổi: {0}->{1}", t.currency, tourinf.currency);
                }
                if (t.rate != tourinf.rate)
                {
                    temp += String.Format("- Tỷ giá thay đổi:  {0:#,##0.0}->{1:#,##0.0}", t.rate, tourinf.rate);
                }
                if (t.rate != tourinf.rate)
                {
                    temp += String.Format("- Tỷ giá thay đổi:  {0:#,##0.0}->{1:#,##0.0}", t.rate, tourinf.rate);
                }
                tourinf.visa = tourinf.visa ?? "";
                if (t.visa != tourinf.visa)
                {
                    temp += String.Format("- Visa thay đổi:  {0}->{1}", t.visa, tourinf.visa);
                }
                if (t.reference != tourinf.reference)
                {
                    temp += String.Format("- Lộ trình  thay đổi:  {0}->{1}", t.reference, tourinf.reference);
                }


                t.companyId = tourinf.companyId;
                t.tourkindId = tourinf.tourkindId;
                t.arr = tourinf.arr;
                t.dep = tourinf.dep;
                t.pax = tourinf.pax;
                t.childern = tourinf.childern;
                t.departoperator = tourinf.departoperator;
                t.operators = tourinf.operators ?? "";
                t.routing = string.IsNullOrEmpty(tourinf.routing) ? "" : tourinf.routing;
                t.revenue = tourinf.revenue;
                t.currency = tourinf.currency;
                t.rate = tourinf.rate;
                t.visa = tourinf.visa;
                t.reference = tourinf.reference ?? "";
                t.passtypeId = tourinf.passtypeId;
                if (!string.IsNullOrEmpty(trienkhai))
                {
                    if (t.chinhanh != trienkhai)
                    {
                        temp += String.Format("- Chi nhánh triển khai  thay đổi:  {0}->{1}", t.chinhanh, trienkhai);
                    }
                    t.chinhanh = trienkhai;// tourinf.chinhanh;                    
                }

                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật tour: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                    t.logfile = t.logfile + log;
                }
                var resutl = _tourinfRepository.Update(t);

                #region Kiểm tra nếu chọn chương trình tour template thì thêm vào       
                // ----------Kiểm tra nếu chọn chương trình tour tạm thì thêm vào trong chương trình tour -----------------
                var tourprogtemp = _tourProgTempRepository.Find(x => x.Code == codetemp).ToList();
                //int maxdate = listTourProg.Where(x => x.date > 0).Select(x => x.date).Max() + 1;
                //int newstt = listTourProg.Select(x => x.stt).Max() + 1;

                if (tourprogtemp != null)
                {
                    foreach (var item in tourprogtemp)
                    {
                        Tourprog tp = new Tourprog();
                        int maxdate = _tourprogRepository.maxDate(tourinf.sgtcode) + 1;
                        int newstt = _tourprogRepository.newSttTourProg(tourinf.sgtcode);
                        tp.sgtcode = t.sgtcode;
                        tp.stt = newstt;// item.stt;
                        tp.date = item.date > 0 ? maxdate : item.date;// item.date;
                        tp.time = item.time;
                        tp.pax = t.pax;
                        tp.childern = t.childern;
                        tp.srvtype = item.srvtype;
                        tp.supplierid = item.supplierid;
                        tp.srvcode = item.srvcode;
                        tp.tour_item = item.tour_item;
                        tp.srvnode = item.srvnode;
                        tp.pickuptime = item.pickuptime;
                        tp.currency = item.currency;
                        tp.arr = item.arr;
                        tp.dep = item.dep;
                        tp.carrier = item.carrier;
                        tp.airtype = item.airtype;
                        tp.unitpricea = item.unitpricea;
                        tp.unitpricec = item.unitpricec;
                        tp.foc = item.foc;
                        tp.carguide = item.carguide;
                        tp.amount = item.amount;
                        tp.debit = item.debit;
                        tp.vatin = item.vatin;
                        tp.vatout = item.vatout;
                        tp.status = "WL";
                        tp.logfile = " User tạo chương trình tour: " + username + " loại: " + item.srvtype + " vào lúc: " + System.DateTime.Now.ToString();
                        tp.chinhanh = item.chinhanh;
                        tp.dieuhanh = t.operators;

                        _tourprogRepository.Create(tp);

                    }
                    // ----------Kiểm tra nếu có hotel tạm thì thêm vào trong hotel -----------------
                    var htltemp = _hotelTempRepository.Find(x => x.Code == codetemp).ToList();
                    if (htltemp != null)
                    {
                        foreach (var h in htltemp)
                        {
                            Hotel htl = new Hotel();
                            htl.sgtcode = t.sgtcode;
                            htl.stt = h.stt;
                            htl.sgl = h.sgl;
                            htl.sglpax = 0;
                            htl.sglcost = h.sglcost;
                            htl.extsgl = 0;
                            htl.extsglcost = h.extsglcost;
                            htl.dbl = h.dbl;
                            htl.dblpax = 0;
                            htl.dblcost = h.dblcost;
                            htl.extdbl = 0;
                            htl.extdblcost = h.extdblcost;
                            htl.twn = h.twn;
                            htl.twnpax = 0;
                            htl.twncost = h.twncost;
                            htl.exttwn = h.exttwn;
                            htl.exttwncost = h.exttwncost;
                            htl.homestay = h.homestay;
                            htl.homestaypax = 0;
                            htl.homestaycost = h.homestaycost;
                            htl.homestaynote = h.homestaynote;
                           
                            htl.oth = h.oth;
                            htl.othcost = h.othcost;
                            htl.othpax = 0;
                            htl.othtype = h.othtype;
                            htl.currency = h.currency;
                            htl.note = h.note;
                            htl.logfile = "User tạo hotel: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                            _hotelRepository.Create(htl);
                        }
                    }

                    // ----------Kiểm tra nếu Sightseeing tạm thì thêm vào trong Sightseeing-----------------
                    var sstemp = _sightseeingTempRepository.Find(x => x.Code == codetemp).ToList();
                    if (sstemp != null)
                    {
                        foreach (var s in sstemp)
                        {
                            Sightseeing ss = new Sightseeing();
                            ss.sgtcode = t.sgtcode;
                            ss.Stt = s.Stt;
                            ss.Codedtq = s.Codedtq;
                            ss.Serial = s.Serial;
                            ss.Debit = s.Debit;
                            ss.Pax = t.pax;
                            ss.PaxPrice = s.PaxPrice;
                            ss.Childern = t.childern;
                            ss.ChildernPrice = s.ChildernPrice;
                            ss.Amount = s.Amount;
                            ss.Vatin = s.Vatin;
                            ss.Vatout = s.Vatout;
                            ss.Chinhanh = s.chinhanh;
                            ss.Httt = s.httt;
                            // ss.del = false;
                            ss.logfile = "User tạo sightseeing: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                            _sightseeingRepository.Create(ss);
                        }
                    }
                }

                // Nếu như thay đổi điều hành trên tour thì cũng cập nhật lại điều hành trên tourProg
                var listTourProg = _tourprogRepository.ListTourProg(tourinf.sgtcode).Where(x => x.chinhanh == tourinf.chinhanh).ToList();
                foreach (var a in listTourProg)
                {
                    var tp = _tourprogRepository.GetById(a.Id);
                    tp.dieuhanh = tourinf.operators;
                    _tourprogRepository.Update(tp);
                }
                #endregion
                if (resutl != null)
                {
                    SetAlert("Cập nhật tour thành công", "success");
                }
                else
                {
                    SetAlert("Cập nhật tour không thành công", "error");
                }

            }
            return Redirect(HttpContext.Session.GetString("urlEditTour"));
        }

        #endregion

        #region Đóng / Huỷ tour
        [HttpPost]
        public ActionResult Dongtour(string id)
        {
            string username = HttpContext.Session.GetString("username");
            var t = _tourinfRepository.GetById(id);
            if (t.locktour.HasValue)
            {
                t.locktour = null;
                t.logfile += System.Environment.NewLine + "=============" + System.Environment.NewLine + "- User mở tour: " + username + " vào lúc: " + System.DateTime.Now;
            }
            else
            {
                t.locktour = System.DateTime.Now;
                t.logfile += System.Environment.NewLine + "=============" + System.Environment.NewLine + "- User lock tour: " + username + " vào lúc: " + System.DateTime.Now;
            }
            _tourinfRepository.Update(t);
            return Json(true);
        }
        public ActionResult Delete(string id)
        {
            var t = _tourinfRepository.GetById(id);
            if (t == null)
            {
                return NotFound();
            }
            if (t.cancel.HasValue)
            {
                ViewBag.title = "Phục hồi tour ";
                ViewBag.cancelnote = "Nhập lý do phục hồi tour";
            }
            else
            {
                ViewBag.title = "Huỷ tour ";
                ViewBag.cancelnote = "Nhập lý do huỷ tour";
            }
            t.cancelnote = "";
            return PartialView("Delete", t);
        }
        [HttpPost]
        public ActionResult Delete(Tourinf tourinf)
        {
            string username = HttpContext.Session.GetString("username");
            Tourinf t = _tourinfRepository.GetById(tourinf.sgtcode);
            if (t.cancel.HasValue)
            {
                t.cancel = null;
                t.cancelnote = tourinf.cancelnote;
                t.logfile += System.Environment.NewLine + "====================" + System.Environment.NewLine + " User: " + username + " phục hồi tour vào lúc: " + System.DateTime.Now.ToString() + ". Lý do phục hồi tour: " + t.cancelnote;

            }
            else
            {
                t.cancel = System.DateTime.Now;
                t.cancelnote = tourinf.cancelnote;
                t.logfile += System.Environment.NewLine + "====================" + System.Environment.NewLine + " User: " + username + " huỷ tour vào lúc: " + System.DateTime.Now.ToString() + ". Lý do huỷ tour: " + t.cancelnote;

            }

            var result = _tourinfRepository.Update(t);
            if (result != null)
            {
                if (t.cancel.HasValue)
                {
                    SetAlert("Huỷ tour thành công", "success");
                }
                else
                {
                    SetAlert("Phục hồi tour thành công", "success");
                }
            }
            else
            {
                if (t.cancel.HasValue)
                {
                    SetAlert("Huỷ tour không thành công", "error");
                }
                else
                {
                    SetAlert("Phục hồi không tour thành công", "error");
                }
            }
            return Redirect(HttpContext.Session.GetString("urlTour"));
        }
        #endregion

        #region "List box"

        public void listTourTemp(string select)
        {
            //List<TourTemplate> tourtemp = _tourTempRepository.GetAll().ToList();
            //ViewBag.tourtemp = new SelectList(tourtemp, "Code", "Tentour", select);

            var a = _tourTempRepository.GetAll().ToList();
            IEnumerable<SelectListItem> selectList = from s in a
                                                     where s.Chinhanh == HttpContext.Session.GetString("chinhanh")
                                                     select new SelectListItem
                                                     {
                                                         Value = s.Code,
                                                         Text = s.Tentour.ToString()
                                                     };
            var empty = new SelectListItem
            {
                Value = "",
                Text = "---- Chọn tour template ----"
            };
            selectList.ToList().Insert(0, empty);
            var b = selectList.ToList();
            b.Insert(0, empty);
            ViewBag.tourtemp = new SelectList(b, "Value", "Text", select);
        }
        public void listCompany(string select)
        {
            List<Company> company = new List<Company>();
            if (HttpContext.Session.GetString("chinhanh") == "STN" && HttpContext.Session.GetString("khachle").ToLower() == "false")
            {
                company = _companyRepository.ListCompanyKhachdoanND(HttpContext.Session.GetString("chinhanh")).ToList();
            }
            else
            {
                company = _companyRepository.GetAll().ToList();
            }

            ViewBag.company = new SelectList(company, "companyId", "name", select);
        }
        public void listUser(string maphong, string select)
        {
            List<UserInfo> nguoidhs = _userinfoRepository.GetLstUserInfoByPhong(maphong, Convert.ToBoolean(HttpContext.Session.GetString("khachle")), HttpContext.Session.GetString("chinhanh"));// _userinfoRepository.dsUser();
            var empty = new UserInfo
            {
                username = "",
                hoten = "-- Chọn nhân viên --"
            };
            nguoidhs.Insert(0, empty);
            ViewBag.operators = new SelectList(nguoidhs, "username", "hoten", select);
        }
        public void listUserByChinhanh(string chinhanh, string select)
        {
            List<Users> nguoidhs = _userRepository.GetAll().Where(x => x.chinhanh == chinhanh && x.Maphong == HttpContext.Session.GetString("phong") && x.khachle == Convert.ToBoolean(HttpContext.Session.GetString("khachle"))).ToList();// _userinfoRepository.GetLstUserInfoByPhong("").Where(x => x.macn == chinhanh).ToList();// _userinfoRepository.dsUser();
            var empty = new Users
            {
                Username = "",
                Hoten = "-- Chọn nhân viên --"
            };
            nguoidhs.Insert(0, empty);
            ViewBag.dieuhanh = new SelectList(nguoidhs, "Username", "Hoten", select);
        }
        public void listTourKind(string select)
        {
            List<Tourkind> tourkind = _tourkindRepository.GetAll().ToList();
            ViewBag.tourkind = new SelectList(tourkind, "Id", "TourkindInf", select);
        }
        public void listNgoaite(string selected)
        {
            List<Ngoaite> ngoaite = _ngoaiteRepository.GetAll().ToList();
            ViewBag.ngoaite = new SelectList(ngoaite, "MaNT", "MaNT", selected);
        }
        public void listPhongban(string select)
        {
            List<Phongban> phongban = _phongbanRepository.ListPhongban().ToList();
            ViewBag.phongban = new SelectList(phongban, "maphong", "tenphong", select);
        }
        public void listPassType(string select)
        {
            List<PassType> passType = _passtypeRepository.GetAll().ToList();
            ViewBag.passType = new SelectList(passType, "passtypeId", "passtypeId", select);
        }
        public void listTuyentq(string select)
        {
            List<Tuyentq> tuyentq = _tuyentqRepository.GetAll().ToList();
            var empty = new Tuyentq
            {
                Code = "",
                Tentuyen = "--- Chọn thông tin --- "
            };
            tuyentq.Insert(0, empty);
            ViewBag.tuyentq = new SelectList(tuyentq, "Code", "Tentuyen", select);
        }
        public void listDieuhanh(string phong, bool khachle, string chinhanh, string select)
        {
            List<UserInfo> nguoidh = _userinfoRepository.GetLstUserInfoByPhong(phong, khachle, chinhanh).ToList();
            ViewBag.nguoidh = new SelectList(nguoidh, "username", "hoten", select);
        }
        public void listDichvu(string selected)
        {
            List<Dichvu> dichvu = _dichvuRepository.Find(x => x.Trangthai == true).OrderBy(x=>x.Tendv).ToList();
            ViewBag.dichvu = new SelectList(dichvu, "Iddichvu", "Tendv", selected);
        }
        public void listChinhanh(string selected)
        {
            List<Dmchinhanh> chinhanh = _chinhanhRepository.ListChinhanh().ToList();
            ViewBag.chinhanh = new SelectList(chinhanh, "Macn", "Macn", selected);
        }
        public void listSupplier(string selected)
        {
            List<Supplier> supplier = _supplierRepository.ListSupplier().ToList();
            var empty = new Supplier
            {
                Code = "",
                Tengiaodich = "-- Chọn thông tin -- "
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
                Tengiaodich = "-- Chọn thông tin --"
            };
            srvcode.Insert(0, empty);
            ViewBag.srvcode = new SelectList(srvcode, "Code", "Tengiaodich", selected);
        }
        public void listQuocgia(string selected)
        {
            List<Quocgia> quocgia = _quocgiaRepository.ListQuocgia().ToList();
            ViewBag.quocgia = new SelectList(quocgia, "TenNuoc", "TenNuoc", selected);
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
        public void listVisa(string selected = "")
        {
            try
            {
                List<SelectListItem> visa = new List<SelectListItem>();
                visa.Add(new SelectListItem { Text = " ", Value = "  " });
                visa.Add(new SelectListItem { Text = "KHONG VISA", Value = "KHONG VISA" });
                visa.Add(new SelectListItem { Text = "NUOC NGOAI", Value = "NUOC NGOAI" });
                visa.Add(new SelectListItem { Text = "CUA KHAU", Value = "CUA KHAU" });
                ViewBag.visa = new SelectList(visa, "Text", "Value", selected);
            }
            catch { return; }
        }
        public void listStatus(string selected = "")
        {
            try
            {
                List<SelectListItem> status = new List<SelectListItem>();
                status.Add(new SelectListItem { Text = "WL", Value = "Waiting" });
                status.Add(new SelectListItem { Text = "PD", Value = "Pending" });
                status.Add(new SelectListItem { Text = "CL", Value = "Cancel" });
                status.Add(new SelectListItem { Text = "OK", Value = "OK" });

                ViewBag.status = new SelectList(status, "Text", "Value", selected);
            }
            catch { return; }
        }
        public void listDebit(string selected)
        {
            List<Supplier> congno = _supplierRepository.ListSupplier().ToList();
            var empty = new Supplier
            {
                Code = "",
                Tengiaodich = "-- Chọn thông tin --"
            };
            congno.Insert(0, empty);
            ViewBag.debit = new SelectList(congno, "Code", "Tengiaodich", selected);
        }
        public void listLoaixe(string selected)
        {
            List<DsLoaixe> loaixe = _loaixeRepository.ListLoaixe().ToList();
            ViewBag.loaixe = new SelectList(loaixe, "Loaixe", "Loaixe", selected);
        }
        public void listHttt(string selected = "")
        {
            List<SelectListItem> select = new List<SelectListItem>();

            select.Add(new SelectListItem { Text = "", Value = "" });
            select.Add(new SelectListItem { Text = "Tiền mặt", Value = "TIEN MAT" });
            select.Add(new SelectListItem { Text = "Chuyển khoản", Value = "CHUYEN KHOAN" });
            ViewBag.httt = new SelectList(select, "Value", "Text", selected);
        }
        public void listLoaiPhong(string selected)
        {
            List<DmLoaiphong> loaip = _loaiphongRepository.GetLstDmLoaiPhong().ToList();
            ViewBag.loaiphong = new SelectList(loaip, "Loaiphong", "Loaiphong", selected);
        }
        public void listPhai(string selected = "")
        {
            List<SelectListItem> select = new List<SelectListItem>();

            select.Add(new SelectListItem { Text = "", Value = "" });
            select.Add(new SelectListItem { Text = "Nam", Value = "true" });
            select.Add(new SelectListItem { Text = "Nữ", Value = "false" });
            ViewBag.phai = new SelectList(select, "Value", "Text", selected);
        }
        [HttpGet]
        public ActionResult getNguoiDHByMaPhong(string maphong)
        {
            List<UserInfo> nguoidhs = new List<UserInfo>();
            if (!string.IsNullOrWhiteSpace(maphong))
            {
                nguoidhs = _userinfoRepository.GetLstUserInfoByPhong(maphong, Convert.ToBoolean(HttpContext.Session.GetString("khachle")), HttpContext.Session.GetString("chinhanh")).ToList();

                //if (nguoidhs.Count() == 1)
                //{
                //    var empty = new UserInfo
                //    {
                //        username = "",
                //        hoten = "Không có thông tin"
                //    };
                //    nguoidhs.Insert(0, empty);
                //}
                //else
                //{
                //    var empty = new UserInfo
                //    {
                //        username = "",
                //        hoten = "-- Chọn nhân viên --"
                //    };
                //    nguoidhs.Insert(0, empty);
                //}
            }

            return Json(nguoidhs);

        }

        [HttpGet]
        public ActionResult getUserByChinhanh(string chinhanh)
        {
            List<Users> nguoidhs = new List<Users>();
            nguoidhs = _userRepository.GetAll().Where(x => x.chinhanh == chinhanh).ToList();
            var empty = new Users
            {
                Username = "",
                Hoten = "-- Chọn nhân viên --"
            };
            nguoidhs.Insert(0, empty);

            return Json(JsonConvert.SerializeObject(nguoidhs));

        }
        public void listMacode()
        {
            var macode = HttpContext.Session.GetString("macode");
            List<string> code = macode.Split(',').ToList<string>();
            List<SelectListItem> select = new List<SelectListItem>();
            foreach (var i in code)
            {
                select.Add(new SelectListItem { Text = i, Value = i });
            }
            ViewBag.code = new SelectList(select, "Value", "Text", "");
        }
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
        public void listDiemtqTinh(string selected)
        {
            var a = _diemtqRepository.GetLstDiemtqTinh();
            IEnumerable<vDmdiemtq> selectList = from s in a
                                                select new vDmdiemtq
                                                {
                                                    Code = s.Code,
                                                    Diemtq = s.Diemtq + ", " + s.Tinhtp.ToString()
                                                };
            var empty = new vDmdiemtq
            {
                Code = "",
                Diemtq = "-- Chọn thông tin --"
            };
            List<vDmdiemtq> lst = selectList.ToList();
            lst.Insert(0, empty);
            ViewBag.dtq = new SelectList(lst, "Code", "Diemtq", selected);
        }

        #endregion



        //-----------Tour Programe------------
        public IActionResult listTourProg(string id)
        {
            var progtemp = _tourprogRepository.ListTourProg(id);
            var t = _tourinfRepository.GetById(id);
            ViewBag.cn = HttpContext.Session.GetString("chinhanh");

            // ViewBag.dieuhanh = HttpContext.Session.GetString("username");
            //ViewBag.concernto = t.concernto;
            if (t != null)
            {
                ViewBag.operators = t.operators ?? "";
            }
            foreach (var item in progtemp)
            {
                // Mượn nguoihuydv để so sánh với điều hành=> cho phép kéo thả dịch vụ
                item.nguoihuydv = t.operators;
                listChinhanh(item.chinhanh);
                //if (!string.IsNullOrEmpty(item.dieuhanh))
                //{
                //    var user = _userRepository.GetById(item.dieuhanh);
                //    item.dieuhanh = user.Hoten;
                //}
                item.logfile = item.date > 0 ? t.arr.AddDays(item.date - 1).ToString("ddd").ToUpper() + "  " + t.arr.AddDays(item.date - 1).ToString("dd/MM") : "";
                switch (item.srvtype)
                {
                    case "LUN":
                    case "DIN":
                    case "BRK":
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
                    case "CAN":                  
                        if (item.supplierid != null)
                        {
                            var supplier = _supplierRepository.GetById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich;
                        }
                        else
                        {
                            item.tour_item = item.tour_item;
                        }
                        break;
                    case "ITI":
                        string tu="", den = "";
                        if (!string.IsNullOrEmpty(item.tour_item))
                        {
                             tu = _thanhphoRepository.ListTinh().Where(x => x.Matinh == item.tour_item).SingleOrDefault().Tentinh;
                        }
                        if (!string.IsNullOrEmpty(item.srvnode))
                        {
                             den = _thanhphoRepository.ListTinh().Where(x => x.Matinh == item.srvnode).SingleOrDefault().Tentinh ?? "";
                        }
                        item.tour_item = "Hành trình: "+ (string.IsNullOrEmpty(tu) ? "" : tu)  + (string.IsNullOrEmpty(den) ? "" : " - " + den);
                        item.srvnode = "";
                        break;
                    case "WPU":
                    case "MUS":
                    case "SHW":
                        if (item.supplierid != null)
                        {
                            var supplier = _supplierRepository.GetById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich + item.time ?? " " + item.carrier;
                        }
                        else
                        {
                            item.tour_item = item.tour_item + " " + item.time ?? " " + item.carrier; ;
                        }
                        break;
                       
                        
                    case "AIR":
                    case "TRA":
                        item.tour_item = item.tour_item + " chặng: " + item.dep + "-" + item.arr + " chuyến: " + item.carrier + " * " + String.Format("{0:#,##0.0}", item.unitpricea) + " " + item.currency;
                        break;
                    case "HTL":
                    case "CRU":
                        if (item.supplierid != null)
                        {
                            string gia = "";
                            var supplier = _supplierRepository.GetById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich + " - " + supplier.Diachi + " phone: " + supplier.Dienthoai;
                            var hotel = _hotelRepository.Find(x => x.sgtcode == item.sgtcode && x.stt == item.stt);//chi lay dich vu chua xoa
                            if (hotel != null)
                            {
                                foreach (var a in hotel)
                                {
                                    if (a.sgl > 0)
                                    {
                                        gia += a.sgl + "SGN" + "*" + string.Format("{0:#,##0.0}", a.sglcost) + a.currency;
                                    }
                                    if (a.extsgl > 0)
                                    {
                                        gia += "," + a.extsgl + "EXT-SGN" + "*" + string.Format("{0:#,##0.0}", a.extsglcost) + a.currency;
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
                                        gia += "," + a.homestay + "Home stay" + "*"+a.homestaypax+" pax*" + string.Format("{0:#,##0.0}", a.homestaycost)+"/1pax" + a.currency;
                                    }
                                   
                                   
                                    if (a.oth > 0)
                                    {
                                        gia += "," + a.oth + " OTH-" + a.othpax + "pax" + "*" + string.Format("{0:#,##0.0}", a.othcost) + a.currency + "-" + a.othtype;
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
                        var sse = _sightseeingRepository.GetByCodeAndStt(item.sgtcode, item.stt);
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
            return PartialView("listTourProg", progtemp);
        }
        #region Thay đổi đơn vị triển khai dịch vụ
        [HttpPost]
        public ActionResult changeChinhanh(decimal id, string chinhanh)
        {
            var tourProg = _tourprogRepository.GetById(id);
            tourProg.logfile = tourProg.logfile + Environment.NewLine + "=================== " + System.Environment.NewLine + " User " + HttpContext.Session.GetString("username") + " thay đổi đơn vị triển khai " + tourProg.chinhanh + " -> " + chinhanh + " ngày " + DateTime.Now.ToString();
            tourProg.chinhanh = chinhanh;
            tourProg.dieuhanh = "";

            _tourprogRepository.Update(tourProg);
            return Json(true);
        }

        #endregion
        #region Thao tác với Thêm/ xoá chương trình tour

        public ActionResult AddTourSrvProg(string id)
        {
            var a = new Tourprog();
            var tour = _tourinfRepository.GetById(id);
            a.sgtcode = id;           
            a.pax = tour.pax;
            a.childern = tour.childern;
            a.date = 0;
            a.vatin = 10;
            a.vatout = 10;
            listHanhtrinhTu("");
            listHanhtrinhDen("");
            listDichvu("TXT");
            listNgoaite("VND");
            //listChinhanh(HttpContext.Session.GetString("chinhanh"));
            //listUserByChinhanh(HttpContext.Session.GetString("chinhanh"), HttpContext.Session.GetString("username"));
            return PartialView("AddTourSrvProg", a);
        }
        [HttpPost]
        public ActionResult AddTourSrvProg([Bind("sgtcode,stt,date,tour_item,srvnode,srvtype,pax,childern,currency,vatin,vatout,status,dieuhanh,chinhanh")] Tourprog tourProg)
        {
            string username = HttpContext.Session.GetString("username");
            tourProg.stt = _tourprogRepository.newSttTourProg(tourProg.sgtcode);
            tourProg.logfile = tourProg.logfile + "-User tạo dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            tourProg.status = "WL";
            tourProg.currency = "VND";           
            tourProg.tour_item = tourProg.tour_item ?? "";
            tourProg.srvnode = tourProg.srvnode ?? "";
            tourProg.dieuhanh = username;
            tourProg.chinhanh = HttpContext.Session.GetString("chinhanh");
            var result = _tourprogRepository.Create(tourProg);
            if (result != null && (tourProg.srvtype == "HTL" || tourProg.srvtype=="CRU"))
            {
                Hotel h = new Hotel();
                h.sgtcode = tourProg.sgtcode;
                h.stt = tourProg.stt;
                h.currency = "VND";
                h.note = "";
                h.logfile = "Hotel được tạo tự động khi thêm dịch vụ HTL trong chương trình tour";
                AddHotel(h);
            }
            return Json(JsonConvert.SerializeObject(result));

        }
        [HttpGet]
        public ActionResult DelTourProg(decimal id)
        {
            var tourProg = _tourprogRepository.GetById(id);
            if (tourProg == null)
            {
                return NotFound();
            }
            if (tourProg.chinhanh != HttpContext.Session.GetString("chinhanh"))
            {
                TempData["thongbao"] = "Bạn không có quyền xoá dịch vụ này.";
                return RedirectToAction("index", "thongbao");
            }
            if (tourProg.status == "WL")
            {
                return PartialView("DelTourProgConfirm", tourProg);
            }
            return PartialView("DelTourProg", tourProg);
        }

        [HttpGet]
        public ActionResult DelTourProgConfirm(Tourprog tourprog)
        {
            return PartialView("DelTourProgConfirm", tourprog);
        }
        // Nếu trạng thái dịch vụ là OK thì phải nhập lý do huỷ dịch vụ
        [HttpPost]
        public ActionResult DelTourProg(Tourprog tourProg)
        {
            string username = HttpContext.Session.GetString("username");
            var tourProgTemp = _tourprogRepository.GetById(tourProg.Id);
            if (tourProg == null)
            {
                return NotFound();
            }
            tourProgTemp.Id = tourProg.Id;
            tourProgTemp.ngayhuydv = System.DateTime.Now;
            tourProgTemp.nguoihuydv = username;
            tourProgTemp.lydohuydv = tourProg.lydohuydv;
            //Cập nhật dịch vụ trước khi xoá, trigger sẽ tự động thêm vào trong TourProg_del
            _tourprogRepository.Update(tourProgTemp);
            var result = _tourprogRepository.Delete(tourProgTemp);
            var tourinf = _tourinfRepository.GetById(tourProgTemp.sgtcode);
            tourinf.logfile = tourinf.logfile + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User: " + username + " xoá dịch vụ: " + tourProgTemp.srvtype + " id=" + tourProgTemp.Id + " lý do: " + tourProg.lydohuydv + " vào lúc: " + System.DateTime.Now.ToString();
            _tourinfRepository.Update(tourinf);
            //xoa table dich vu lien quan      

            switch (tourProgTemp.srvtype)
            {
                case "HTL": //hotel
                    List<Hotel> ht = _hotelRepository.GetLstHTLByCodeAndOrder(tourProgTemp.sgtcode, tourProgTemp.stt);

                    foreach (Hotel htl in ht)
                    {
                        //htl.del = true;//xoa dong tuong ung                       
                        //htl.logfile = htl.logfile ?? "" + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User xóa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                        _hotelRepository.Delete(htl);
                        tourinf.logfile = tourinf.logfile + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User: " + username + " xoá dịch vụ hotel vào lúc: " + System.DateTime.Now.ToString();
                        _tourinfRepository.Update(tourinf);
                    }

                    break;
                case "SSE": //hotel
                    List<Sightseeing> see = _sightseeingRepository.GetByCodeAndStt(tourProgTemp.sgtcode, tourProgTemp.stt);

                    foreach (Sightseeing s in see)
                    {
                        //s.del = true;//xoa dong tuong ung
                        //s.logfile = s.logfile ?? "" + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User xóa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();

                        //_sightseeingRepository.Update(s);
                        _sightseeingRepository.Delete(s);

                        tourinf.logfile = tourinf.logfile + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User: " + username + " xoá dịch vụ sightseeing id= " + s.Id + " vào lúc: " + System.DateTime.Now.ToString();
                        _tourinfRepository.Update(tourinf);
                    }
                    break;
            }
            var listtourprog = _tourprogRepository.ListNoTracking(x => x.sgtcode == tourProgTemp.sgtcode).OrderBy(x => x.stt).ThenBy(x => x.Id);
            List<string> list = new List<string>();
            foreach (var i in listtourprog)
            {
                list.Add("row_" + i.Id);
            }
            CapNhatSTT(list.ToArray());
            return Redirect(HttpContext.Session.GetString("urlEditTour"));
        }
        // Nếu trạng thái dịch vụ là WL thì chỉ hỏi trước khi xoá, không cập nhật lý do xoá
        [HttpPost]
        public ActionResult DelTourSrvProg(Tourprog tourProg)
        {
            string username = HttpContext.Session.GetString("username");

            //bool status = true;
            var tourProgTemp = _tourprogRepository.GetById(tourProg.Id);
            if (tourProgTemp == null)
            {
                return NotFound();
            }
            tourProgTemp.ngayhuydv = System.DateTime.Now;
            tourProgTemp.nguoihuydv = username;
            _tourprogRepository.Update(tourProgTemp);
            var result = _tourprogRepository.Delete(tourProgTemp);
            var tourinf = _tourinfRepository.GetById(tourProgTemp.sgtcode);
            tourinf.logfile = tourinf.logfile + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User: " + username + " xoá dịch vụ: " + tourProgTemp.srvtype + " id=" + tourProg.Id + " vào lúc: " + System.DateTime.Now.ToString();
            _tourinfRepository.Update(tourinf);
            //xoa table dich vu lien quan      
            switch (tourProgTemp.srvtype)
            {
                case "HTL": //hotel
                    List<Hotel> ht = _hotelRepository.GetLstHTLByCodeAndOrder(tourProgTemp.sgtcode, tourProgTemp.stt);

                    foreach (Hotel htl in ht)
                    {
                        //htl.del = true;//xoa dong tuong ung                       
                        //htl.logfile = htl.logfile ?? "" + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User xóa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                        _hotelRepository.Delete(htl);
                        tourinf.logfile = tourinf.logfile + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User: " + username + " xoá dịch vụ hotel vào lúc: " + System.DateTime.Now.ToString();
                        _tourinfRepository.Update(tourinf);
                    }

                    break;
                case "SSE": //hotel
                    List<Sightseeing> see = _sightseeingRepository.GetByCodeAndStt(tourProgTemp.sgtcode, tourProgTemp.stt);

                    foreach (Sightseeing s in see)
                    {

                        _sightseeingRepository.Delete(s);
                        tourinf.logfile = tourinf.logfile + System.Environment.NewLine + "===================" + System.Environment.NewLine + "-User: " + username + " xoá dịch vụ sightseeing id= " + s.Id + " vào lúc: " + System.DateTime.Now.ToString();
                        _tourinfRepository.Update(tourinf);
                    }

                    break;
            }

            var listtourprog = _tourprogRepository.ListNoTracking(x => x.sgtcode == tourProgTemp.sgtcode).OrderBy(x => x.stt).ThenBy(x => x.Id);
            List<string> list = new List<string>();
            foreach (var i in listtourprog)
            {
                list.Add("row_" + i.Id);
            }
            CapNhatSTT(list.ToArray());
            return Redirect(HttpContext.Session.GetString("urlEditTour"));
        }
        #region Kiểm tra sự tồn tại ngay
        public IActionResult ngayExists(string sgtcode, int ngay)
        {
            bool result = false;
            var u = _tourprogRepository.Find(x => x.sgtcode == sgtcode && x.date == ngay).FirstOrDefault();
            if (ngay == 0 || u == null)
                result = true;

            return Json(result);
        }
        public int newDateTourProg(string sgtcode)
        {
            return _tourprogRepository.newDateTourProg(sgtcode);
        }
        #endregion
        #endregion
        #region Thao tác chèn copy / paste dịch vụ chương trình tour
        public ActionResult InsertTourSrvProg(decimal id)
        {
            var a = _tourprogRepository.GetByIdAsNoTracking(x => x.Id == id);
            ViewBag.stt = a.stt;
            //  ViewBag.date = a.date;
            var tour = _tourinfRepository.GetById(a.sgtcode);
            var newTourProg = new Tourprog();
            newTourProg.sgtcode = tour.sgtcode;
            newTourProg.pax = tour.pax;
            newTourProg.childern = tour.childern;
            newTourProg.date = 0;
            newTourProg.vatin = 0;
            newTourProg.vatout = 0;
            listDichvu("TXT");
            listNgoaite("VND");
            listChinhanh(HttpContext.Session.GetString("chinhanh"));
            listUserByChinhanh(HttpContext.Session.GetString("chinhanh"), HttpContext.Session.GetString("username"));

            return PartialView("InsertTourSrvProg", newTourProg);
        }

        [HttpPost]
        public ActionResult InsertTourSrvProg(Tourprog tourProg, int Oldstt)
        {
            var t = _tourinfRepository.GetById(tourProg.sgtcode);
            string username = HttpContext.Session.GetString("username");
            tourProg.stt = Oldstt - 1;
            if (tourProg.date > 0)
            {
                tourProg.ngaythang = t.arr.AddDays(tourProg.date - 1);
            }

            tourProg.logfile = tourProg.logfile + "-User tạo dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            tourProg.status = "WL";
            tourProg.dieuhanh = username;
            var result = _tourprogRepository.Create(tourProg);
            if (tourProg.srvtype == "HTL" && result != null)
            {
                Hotel h = new Hotel();
                h.sgtcode = tourProg.sgtcode;
                h.stt = tourProg.stt;
                h.currency = "VND";
                h.note = "";
                h.logfile = "Hotel được tạo tự động khi thêm dịch vụ HTL trong chương trình tour";
                AddHotel(h);
            }
            var listtourprog = _tourprogRepository.ListNoTracking(x => x.sgtcode == tourProg.sgtcode).OrderBy(x => x.stt).ThenBy(x => x.Id);
            List<string> list = new List<string>();
            foreach (var i in listtourprog)
            {
                list.Add("row_" + i.Id);
            }
            CapNhatSTT(list.ToArray());
            //return Redirect(HttpContext.Session.GetString("urlEditTour"));
            return Json(JsonConvert.SerializeObject(result));
        }
        [HttpPost]
        public ActionResult copyTourSrvProg(decimal id, int stt)
        {
            var t = _tourprogRepository.GetById(id);
            if (t == null)
            {
                return Json(new
                {
                    status = false,
                    mes = "Không tìm thấy dịch vụ cần tìm. Vui lòng chọn dịch vụ cần copy"
                });
            }
            else
            {
                #region Tạo chương trình tour từ copy
                var tourProg = new Tourprog();
                tourProg.sgtcode = t.sgtcode;
                tourProg.stt = stt;
                tourProg.date = t.date;
                tourProg.time = t.time;
                tourProg.pax = t.pax;
                tourProg.childern = t.childern;
                tourProg.srvtype = t.srvtype;
                tourProg.supplierid = t.supplierid;
                tourProg.srvcode = t.srvcode;
                tourProg.tour_item = t.tour_item;
                tourProg.srvnode = t.srvnode;
                tourProg.currency = t.currency;
                tourProg.arr = t.arr;
                tourProg.dep = t.dep;
                tourProg.carrier = t.carrier;
                tourProg.airtype = t.airtype;
                tourProg.pickuptime = t.pickuptime;
                tourProg.unitpricea = t.unitpricea;
                tourProg.unitpricec = t.unitpricec;
                tourProg.foc = t.foc;
                tourProg.carguide = t.carguide;
                tourProg.amount = t.amount;
                tourProg.debit = t.debit;
                tourProg.vatin = t.vatin;
                tourProg.vatout = t.vatout;
                tourProg.status = "WL";
                tourProg.dieuhanh = t.dieuhanh;
                tourProg.chinhanh = t.chinhanh;
                tourProg.logfile = "User tạo dịch vụ " + HttpContext.Session.GetString("username") + " ngày " + System.DateTime.Now;
                var result = _tourprogRepository.Create(tourProg);
                #endregion
                #region Tạo hotel từ copy
                // Nếu dịch vụ là Hotel thì copy và tạo 1 dòng mới trên tourProg
                if (tourProg.srvtype == "HTL" && result != null)
                {
                    var listhotel = _hotelRepository.GetAll().Where(x => x.sgtcode == tourProg.sgtcode && x.stt == t.stt).ToList(); // Lấy stt của tour đã copy
                    foreach (var i in listhotel)
                    {
                        Hotel h = new Hotel();
                        h.sgtcode = i.sgtcode;
                        h.stt = tourProg.stt;
                        h.sgl = i.sgl;
                        h.sglpax = i.sglpax;
                        h.sglcost = i.sglcost;
                        h.extsgl = i.extsgl;
                        h.extsglcost = i.extsglcost;
                        h.dbl = i.dbl;
                        h.dblpax = i.dblpax;
                        h.dblcost = i.dblcost;
                        h.extdbl = i.extdbl;
                        h.extdblcost = i.extdblcost;
                        h.twn = i.twn;
                        h.twnpax = i.twnpax;
                        h.twncost = i.twncost;
                        h.exttwn = i.exttwn;
                        h.exttwncost = i.exttwncost;
                        h.homestay = i.homestay;
                        h.homestaypax = i.homestaypax;
                        h.homestaycost = i.homestaycost;
                        h.homestaynote = i.homestaynote;
                        h.oth = i.oth;
                        h.othpax = i.othpax;
                        h.othcost = i.othcost;
                        h.othtype = i.othtype;
                        h.currency = i.currency;
                        h.note = i.note;
                        h.logfile = Environment.NewLine + "Hotel được tạo từ copy. User tạo: " + HttpContext.Session.GetString("username") + " Ngày " + System.DateTime.Now;
                        _hotelRepository.Create(h);
                    }
                }
                #endregion
                #region Cập nhật lại stt
                // Cập nhật lại stt
                var listtourprog = _tourprogRepository.ListNoTracking(x => x.sgtcode == tourProg.sgtcode).OrderBy(x => x.stt).ThenBy(x => x.Id);
                List<string> list = new List<string>();
                foreach (var i in listtourprog)
                {
                    list.Add("row_" + i.Id);
                }
                CapNhatSTT(list.ToArray());
                #endregion
                return Json(new
                {
                    status = true,
                    data = JsonConvert.SerializeObject(result)
                });
            }
        }

        #endregion

        #region Thao tác với hành trình Itinerary
        public ActionResult EditItineraryProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            var t = _tourinfRepository.GetById(tourprog.sgtcode);
            listStatus(tourprog.status);
            listHanhtrinhTu(tourprog.tour_item);
            listHanhtrinhDen(tourprog.srvnode);

            return PartialView("EditItineraryProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditItineraryProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (a.tour_item != tourProgTemp.tour_item)
            {
                temp += String.Format("- Hành trình từ thay đổi: {0}->{1} ", a.tour_item, tourProgTemp.tour_item);
            }
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += String.Format("- Hành trình đến thay đổi: {0}->{1} ", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Khả thi thay đổi: {0}->{1} ", a.status, tourProgTemp.status);
            }
           
            #endregion

            a.sgtcode = tourProgTemp.sgtcode;
            a.stt = tourProgTemp.stt;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.status = tourProgTemp.status;          
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User hành trình " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }

        #endregion

        #region Thao tác với dịch vụ OVR (oversea), WPU, TRS (vận chuyển khác), Các dịch vụ còn lại


        //---------- Cập nhật dịch vụ over sea trong chương trình tour-----------
        public ActionResult EditOverSeaProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            if (tourprog.srvtype == "OVR")
            {
                ViewBag.ghichu = "Ghi chú nối tour";
                ViewBag.cartitle = "CHI TIẾT VỀ NỐI TOUR QUỐC TẾ";
                ViewBag.legend = "Đơn vị bán tour";
                ViewBag.donvi = "Đơn vị";
            }
            //else if (tourprog.srvtype == "WPU")
            //{
            //    ViewBag.ghichu = "Ghi chú múa rối nước";
            //    ViewBag.cartitle = "CHI TIẾT VỀ MÚA RỐI NƯỚC";
            //    ViewBag.legend = "Thông tin về nhóm múa rối nước";
            //    ViewBag.donvi = "Nhóm";
            //}
            //else if (tourprog.srvtype == "TRS")
            //{
            //    ViewBag.ghichu = "Ghi chú dịch vụ";
            //    ViewBag.cartitle = "CHI TIẾT VỀ CÁC DỊCH VỤ VẬN CHUYỂN KHÁC";
            //    ViewBag.legend = "Thông tin nơi thuê xích lô, xe đạp...";
            //    ViewBag.donvi = "Diễn giải";
            //}
            else
            {
                var dichvu = _dichvuRepository.GetById(tourprog.srvtype);

                ViewBag.ghichu = "Ghi chú dịch vụ";
                ViewBag.cartitle = "CHI TIẾT VỀ  DỊCH VỤ " + dichvu.Tendv;
                ViewBag.legend = "Thông tin dịch vụ";
                ViewBag.donvi = "Đơn vị";
            }
            return PartialView("EditOverSeaProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditOverSeaProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");
            temp = ""; log = "";
            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;
            //a.Code = tourProgTemp.Code;
            //a.stt = tourProgTemp.stt;
            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            if (a.supplierid != tourProgTemp.supplierid)
            {
                temp += String.Format("- supplier thay đổi: {0}->{1}", a.supplierid, tourProgTemp.supplierid);
            }
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += String.Format("- Service code thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.currency != tourProgTemp.currency)
            {
                temp += String.Format("- Currencty thay đổi: {0}->{1} ", a.currency, tourProgTemp.currency);
            }
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1} ", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }
            if (a.amount != tourProgTemp.amount)
            {
                temp += String.Format("- Tổng chi phí thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.amount, tourProgTemp.amount);
            }
            if (a.vatin != tourProgTemp.vatin)
            {
                temp += String.Format("- VAT in: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += String.Format("- VAT out thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }
            if (a.debit != tourProgTemp.debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit.ToString(), tourProgTemp.debit.ToString());
            }
            if (a.pax != tourProgTemp.pax)
            {
                temp += String.Format("- Số khách thay đổi: {0}->{1}", a.pax, tourProgTemp.pax);
            }
            if (a.childern != tourProgTemp.childern)
            {
                temp += String.Format("- Số trẻ em thay đổi: {0}->{1}", a.childern, tourProgTemp.childern);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.chinhanh);
            }
            #endregion
            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.pax = tourProgTemp.pax;
            a.childern = tourProgTemp.childern;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode.ToUpper();
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.debit = tourProgTemp.debit;
            a.status = tourProgTemp.status;
            a.chinhanh = tourProgTemp.chinhanh;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            if (temp.Length > 0)
            {
                log = Environment.NewLine;
                log += "=============";
                log += Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }

            _tourprogRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ PAC
        //---------- Cập nhật dịch vụ tour trọn gói trong chương trình tour-----------
        public ActionResult EditPacProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            return PartialView("EditPacProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditPacProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);

            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            if (a.supplierid != tourProgTemp.supplierid)
            {
                temp += String.Format("- supplier thay đổi: {0}->{1}", a.supplierid, tourProgTemp.supplierid);
            }
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += String.Format("- Service code thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.currency != tourProgTemp.currency)
            {
                temp += String.Format("- Currencty thay đổi: {0}->{1} ", a.currency, tourProgTemp.currency);
            }
            if (a.pax != tourProgTemp.pax)
            {
                temp += String.Format("- Tổng khách thay đổi: {0}->{1} ", a.pax, tourProgTemp.pax);
            }
            if (a.childern != tourProgTemp.childern)
            {
                temp += String.Format("- Trẻ em thay đổi: {0}->{1} ", a.childern, tourProgTemp.childern);
            }
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1} ", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }
            if (a.amount != tourProgTemp.amount)
            {
                temp += String.Format("- Tổng chi phí thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.amount, tourProgTemp.amount);
            }
            if (a.vatin != tourProgTemp.vatin)
            {
                temp += String.Format("- VAT vào: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += String.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }
            if (a.debit != tourProgTemp.debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit.ToString(), tourProgTemp.debit.ToString());
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.chinhanh);
            }
            #endregion

            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.pax = tourProgTemp.pax;
            a.childern = tourProgTemp.childern;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode.ToUpper();
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.debit = tourProgTemp.debit;
            a.status = tourProgTemp.status;
            a.chinhanh = tourProgTemp.chinhanh;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            //a.logfile = a.logfile + "-User sửa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ CAR, CAG
        // Cập nhật dịch vụ text của chương trình tour
        public ActionResult EditCarProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
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
            return PartialView("EditCarProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditCarProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            string temp = ""; log = "";
            if (a.tour_item != tourProgTemp.tour_item)
            {
                temp += String.Format("- Yêu cầu thay đổi: {0}->{1}", a.tour_item, tourProgTemp.tour_item);
            }
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.chinhanh);
            }
            if (a.dieuhanh != tourProgTemp.dieuhanh)
            {
                temp += String.Format("- Điều hành thay đổi: {0}->{1}", a.dieuhanh, tourProgTemp.dieuhanh);
            }
            #endregion

            a.sgtcode = tourProgTemp.sgtcode;
            a.stt = tourProgTemp.stt;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.chinhanh = tourProgTemp.chinhanh;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            a.status = tourProgTemp.status;
            // a.logfile = a.logfile + "-User sửa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ AIR
        public ActionResult EditAirProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            var t = _tourinfRepository.GetById(tourprog.sgtcode);
            listAirType(tourprog.airtype);
            listNgoaite(tourprog.currency);
            listSrvcode(tourprog.srvcode);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            ViewBag.concernto = t.concernto;
            ViewBag.operators = t.operators;
            tourprog.pax = tourprog.pax + tourprog.childern;
            return PartialView("EditAirProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditAirProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            string temp = ""; log = "";
            if (a.currency != tourProgTemp.currency)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", a.currency, tourProgTemp.currency);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }
            if (a.dep != tourProgTemp.dep)
            {
                temp += String.Format("- Chặng bay đi thay đổi: {0}->{1}", a.dep, tourProgTemp.dep);
            }
            if (a.arr != tourProgTemp.arr)
            {
                temp += String.Format("- Chặng bay đến thay đổi: {0}->{1}", a.arr, tourProgTemp.arr);
            }

            if (a.debit != tourProgTemp.debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit, tourProgTemp.debit);
            }
            if (a.carrier != tourProgTemp.carrier)
            {
                temp += String.Format("- Chuyến thay đổi: {0}->{1}", a.carrier, tourProgTemp.carrier);
            }
            if (a.airtype != tourProgTemp.airtype)
            {
                temp += String.Format("- Loại hình thay đổi: {0}->{1}", a.airtype, tourProgTemp.airtype);
            }
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += String.Format("- Nơi book thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.vatin != tourProgTemp.vatin)
            {
                temp += String.Format("- VAT vào thay đổi: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += String.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }
            if (a.pax != tourProgTemp.pax - tourProgTemp.childern)
            {
                temp += String.Format("- Tổng số khách thay đổi: {0}->{1}", a.pax, tourProgTemp.pax);
            }
            if (a.childern != tourProgTemp.childern)
            {
                temp += String.Format("- Số trẻ em thay đổi: {0}->{1}", a.childern, tourProgTemp.childern);
            }
            if (a.time != tourProgTemp.time)
            {
                temp += String.Format("- Giờ thay đổi: {0}->{1}", a.time, tourProgTemp.time);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.chinhanh);
            }
            if (a.dieuhanh != tourProgTemp.dieuhanh)
            {
                temp += String.Format("- Điều hành thay đổi thay đổi: {0}->{1}", a.dieuhanh, tourProgTemp.dieuhanh);
            }
            #endregion

            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.currency = tourProgTemp.currency;
            a.pax = tourProgTemp.pax - tourProgTemp.childern;
            a.childern = tourProgTemp.childern;
            a.time = tourProgTemp.time;
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.arr = string.IsNullOrEmpty(tourProgTemp.arr) ? "" : tourProgTemp.arr.ToUpper();
            a.dep = string.IsNullOrEmpty(tourProgTemp.dep) ? "" : tourProgTemp.dep.ToUpper();
            a.debit = tourProgTemp.debit;
            a.carrier = tourProgTemp.carrier;
            a.airtype = tourProgTemp.airtype;
            a.srvcode = tourProgTemp.srvcode;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.status = tourProgTemp.status;
            a.chinhanh = tourProgTemp.chinhanh;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            //a.logfile = a.logfile + "-User sửa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ Train / Boat ticket
        public ActionResult EditTrainProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            var t = _tourinfRepository.GetById(tourprog.sgtcode);
            listAirType(tourprog.airtype);
            listNgoaite(tourprog.currency);
            listSrvcode(tourprog.srvcode);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            ViewBag.concernto = t.concernto;
            ViewBag.operators = t.operators;
            tourprog.pax = tourprog.pax + tourprog.childern;
            return PartialView("EditTrainProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditTrainProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            string temp = ""; log = "";
            if (a.currency != tourProgTemp.currency)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", a.currency, tourProgTemp.currency);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }
            if (a.dep != tourProgTemp.dep)
            {
                temp += String.Format("-Ga / Cảng  đi thay đổi: {0}->{1}", a.dep, tourProgTemp.dep);
            }
            if (a.arr != tourProgTemp.arr)
            {
                temp += String.Format("- Ga / Cảng đến thay đổi: {0}->{1}", a.arr, tourProgTemp.arr);
            }
            if (a.time != tourProgTemp.time)
            {
                temp += String.Format("- Giờ khởi hành / đến đến thay đổi: {0}->{1}", a.arr, tourProgTemp.arr);
            }
            if (a.debit != tourProgTemp.debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit, tourProgTemp.debit);
            }
            if (a.carrier != tourProgTemp.carrier)
            {
                temp += String.Format("- Chuyến thay đổi: {0}->{1}", a.carrier, tourProgTemp.carrier);
            }
            if (a.airtype != tourProgTemp.airtype)
            {
                temp += String.Format("- Loại hình thay đổi: {0}->{1}", a.airtype, tourProgTemp.airtype);
            }
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += String.Format("- Nơi book thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.vatin != tourProgTemp.vatin)
            {
                temp += String.Format("- VAT vào thay đổi: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += String.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }
            if (a.pax != tourProgTemp.pax - tourProgTemp.childern)
            {
                temp += String.Format("- Tổng số khách thay đổi: {0}->{1}", a.pax, tourProgTemp.pax);
            }
            if (a.childern != tourProgTemp.childern)
            {
                temp += String.Format("- Số trẻ em thay đổi: {0}->{1}", a.childern, tourProgTemp.childern);
            }
            if (a.time != tourProgTemp.time)
            {
                temp += String.Format("- Giờ thay đổi: {0}->{1}", a.time, tourProgTemp.time);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.chinhanh);
            }
            if (a.dieuhanh != tourProgTemp.dieuhanh)
            {
                temp += String.Format("- Điều hành thay đổi thay đổi: {0}->{1}", a.dieuhanh, tourProgTemp.dieuhanh);
            }
            #endregion

            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.currency = tourProgTemp.currency;
            a.pax = tourProgTemp.pax - tourProgTemp.childern;
            a.childern = tourProgTemp.childern;
            a.time = tourProgTemp.time;
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.arr = tourProgTemp.arr;
            a.dep = tourProgTemp.dep;
            a.debit = tourProgTemp.debit;
            a.carrier = tourProgTemp.carrier;
            a.airtype = tourProgTemp.airtype;
            a.srvcode = tourProgTemp.srvcode;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.status = tourProgTemp.status;
            a.chinhanh = tourProgTemp.chinhanh;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            //a.logfile = a.logfile + "-User sửa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }

        #endregion
        #region Thao tác với dịch vụ SHP (shopping)
        public ActionResult EditShopProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            var t = _tourinfRepository.GetById(tourprog.sgtcode);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            ViewBag.concernto = t.concernto;
            ViewBag.operators = t.operators;
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            return PartialView("EditShopProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditShopProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            string temp = ""; log = "";
            if (a.supplierid != tourProgTemp.supplierid)
            {
                temp += String.Format("- supplier thay đổi: {0}->{1}", a.supplierid, tourProgTemp.supplierid);
            }
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += String.Format("- Service code thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.currency != tourProgTemp.currency)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", a.currency, tourProgTemp.currency);
            }
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }
            if (a.amount != tourProgTemp.amount)
            {
                temp += String.Format("- Tổng chi phí thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.amount, tourProgTemp.amount);
            }
            if (a.vatin != tourProgTemp.vatin)
            {
                temp += String.Format("- VAT vào thay đổi: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += String.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }
            if (a.debit != tourProgTemp.debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit, tourProgTemp.debit);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.chinhanh);
            }
            #endregion

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
            a.debit = tourProgTemp.debit;
            a.status = tourProgTemp.status;
            a.chinhanh = tourProgTemp.chinhanh;
            // a.logfile = a.logfile + "-User sửa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }
        #endregion

        #region Thao tác với dịch vụ Shows / Music / Water Puple / AO Show
        public ActionResult EditShowProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            var t = _tourinfRepository.GetById(tourprog.sgtcode);
            listAirType(tourprog.airtype);
            listNgoaite(tourprog.currency);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            ViewBag.concernto = t.concernto;
            ViewBag.operators = t.operators;
            tourprog.pax = tourprog.pax + tourprog.childern;
            return PartialView("EditShowProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditShowProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            string temp = ""; log = "";
            if (a.currency != tourProgTemp.currency)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", a.currency, tourProgTemp.currency);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }
            if (a.amount != tourProgTemp.amount)
            {
                temp += String.Format("- Giá theo nhóm thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.amount, tourProgTemp.amount);
            }
            if (a.debit != tourProgTemp.debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit, tourProgTemp.debit);
            }
            if (a.carrier != tourProgTemp.carrier)
            {
                temp += String.Format("- Chuyến thay đổi: {0}->{1}", a.carrier, tourProgTemp.carrier);
            }
            //if (a.airtype != tourProgTemp.airtype)
            //{
            //    temp += String.Format("- Loại hình thay đổi: {0}->{1}", a.airtype, tourProgTemp.airtype);
            //}
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += String.Format("- Nơi book thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.vatin != tourProgTemp.vatin)
            {
                temp += String.Format("- VAT vào thay đổi: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += String.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }
            if (a.pax != tourProgTemp.pax - tourProgTemp.childern)
            {
                temp += String.Format("- Tổng số khách thay đổi: {0}->{1}", a.pax, tourProgTemp.pax);
            }
            if (a.childern != tourProgTemp.childern)
            {
                temp += String.Format("- Số trẻ em thay đổi: {0}->{1}", a.childern, tourProgTemp.childern);
            }
            if (a.time != tourProgTemp.time)
            {
                temp += String.Format("- Giờ thay đổi: {0}->{1}", a.time, tourProgTemp.time);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.chinhanh);
            }
            if (a.dieuhanh != tourProgTemp.dieuhanh)
            {
                temp += String.Format("- Điều hành thay đổi thay đổi: {0}->{1}", a.dieuhanh, tourProgTemp.dieuhanh);
            }
            #endregion

            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.currency = tourProgTemp.currency;
            a.pax = tourProgTemp.pax - tourProgTemp.childern;
            a.childern = tourProgTemp.childern;
            a.time = tourProgTemp.time??"";
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;           
            a.debit = tourProgTemp.debit;
            a.carrier = tourProgTemp.carrier??"";
            a.srvcode = tourProgTemp.srvcode;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.status = tourProgTemp.status;
            a.chinhanh = tourProgTemp.chinhanh;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            //a.logfile = a.logfile + "-User sửa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }
            #endregion

        #region Thao tác với dịch vụ SSE
            public ActionResult EditSightseeing(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            // listDiemtq("");
            listNgoaite(tourprog.currency);
            listDiemtqTinh(tourprog.tour_item);
            listSrvcode(tourprog.srvcode == null ? "" : tourprog.srvcode);
            listHttt("");
            listDebit("");
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            List<Sightseeing> lst = _sightseeingRepository.GetByCodeAndStt(tourprog.sgtcode, (int)tourprog.stt);
            var model = new List<SightseeingViewModel>();
            string sDiemtq = "", sTengiaodich = "";
            foreach (Sightseeing s in lst)
            {
                SightseeingViewModel m = new SightseeingViewModel();
                m.Sightseeing = s;

                Dmdiemtq dtq = _diemtqRepository.GetById(s.Codedtq);
                if (dtq != null) sDiemtq = dtq.Diemtq;
                m.Diemtq = sDiemtq;

                Supplier sup = _supplierRepository.GetById(s.Debit);
                if (sup != null) sTengiaodich = sup.Tengiaodich;
                m.Tengiaodich = sTengiaodich;

                model.Add(m);
            }

            ViewBag.listsee = model;

            return PartialView("EditSightseeing", tourprog);
        }

        /// <summary>
        /// Code:    stt
        /// </summary>
        /// <param name="tourProgTemp"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSightseeingProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            string temp = ""; log = "";
            string odtq = "", ndtq = "";
            if (a.tour_item != tourProgTemp.tour_item)
            {
                var olddtq = _diemtqRepository.GetById(a.tour_item);
                if (olddtq != null)
                {
                    odtq = olddtq.Diemtq;
                }
                else
                {
                    odtq = a.tour_item;
                }
                var newdtq = _diemtqRepository.GetById(tourProgTemp.tour_item);
                if (newdtq != null)
                {
                    ndtq = newdtq.Diemtq;
                }
                else
                {
                    ndtq = tourProgTemp.tour_item;
                }

                temp += string.Format("- Tham quan thay đổi: {0}->{1}", odtq, ndtq);
            }
            if (a.supplierid != tourProgTemp.supplierid)
            {
                temp += string.Format("- supplier thay đổi: {0}->{1}", a.supplierid, tourProgTemp.supplierid);
            }
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += string.Format("- Service code thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.currency != tourProgTemp.currency)
            {
                temp += string.Format("- Loại tiền thay đổi: {0}->{1}", a.currency, tourProgTemp.currency);
            }
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += string.Format("- Ghi chú thay đổi: {0}->{1}", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += string.Format("- Trạng thái thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += string.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += string.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }
            if (a.amount != tourProgTemp.amount)
            {
                temp += string.Format("- Tổng chi phí thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.amount, tourProgTemp.amount);
            }
            if (a.vatin != tourProgTemp.vatin)
            {
                temp += string.Format("- VAT vào thay đổi: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += string.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }
            if (a.debit != tourProgTemp.debit)
            {
                temp += string.Format("- Công nợ thay đổi: {0}->{1}", a.debit, tourProgTemp.debit);
            }

            #endregion

            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.debit = tourProgTemp.debit;
            a.status = tourProgTemp.status;
            a.chinhanh = tourProgTemp.chinhanh;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            //a.logfile = a.logfile + "-User sửa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);

            return Json(true);
        }

        //Cap nhat THam quan Temp
        [HttpPost]
        public ActionResult AddSee(Sightseeing entity)
        {
            string username = HttpContext.Session.GetString("username");


            var see = new Sightseeing();

            see.sgtcode = entity.sgtcode;
            see.Stt = entity.Stt;
            see.Codedtq = entity.Codedtq;
            see.Serial = entity.Serial;
            see.Debit = entity.Debit;
            see.PaxPrice = string.IsNullOrEmpty(entity.PaxPrice.ToString()) ? 0 : entity.PaxPrice;
            see.Pax = string.IsNullOrEmpty(entity.Pax.ToString()) ? 0 : entity.Pax;
            see.ChildernPrice = string.IsNullOrEmpty(entity.ChildernPrice.ToString()) ? 0 : entity.ChildernPrice;
            see.Childern = string.IsNullOrEmpty(entity.Childern.ToString()) ? 0 : entity.Childern;
            see.Amount = (see.PaxPrice * see.Pax) + (see.Childern * see.ChildernPrice);
            see.Vatin = string.IsNullOrEmpty(entity.Vatin.ToString()) ? 10 : entity.Vatin;
            see.Vatout = string.IsNullOrEmpty(entity.Vatout.ToString()) ? 10 : entity.Vatout;
            see.Httt = entity.Httt;
            see.Chinhanh = HttpContext.Session.GetString("chinhanh");
            see.logfile = see.logfile + "-User tạo sightseeing: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            //see.del = false;//trang thai chua xoa

            _sightseeingRepository.Create(see);
            return Json(see);
        }

        [HttpPost]
        public ActionResult EditSee(Sightseeing entity)
        {
            string username = HttpContext.Session.GetString("username");

            var see = _sightseeingRepository.GetById(entity.Id);
            see.sgtcode = entity.sgtcode;
            see.Stt = entity.Stt;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            string temp = ""; log = "";
            if (see.Codedtq != entity.Codedtq)
            {
                temp += String.Format("- Điểm tham quan thay đổi: {0}->{1}", see.Codedtq, entity.Codedtq);
            }
            if (see.Serial != entity.Serial)
            {
                temp += String.Format("- Serial thay đổi: {0}->{1}", see.Serial, entity.Serial);
            }
            if (see.Debit != entity.Debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", see.Debit, entity.Debit);
            }

            if (see.PaxPrice != entity.PaxPrice)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", see.PaxPrice, entity.PaxPrice);
            }
            if (see.ChildernPrice != entity.ChildernPrice)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", see.ChildernPrice, entity.ChildernPrice);
            }
            if (see.Pax != entity.Pax)
            {
                temp += String.Format("- Số người lớn thay đổi: {0}->{1}", see.Pax, entity.Pax);
            }
            if (see.Childern != entity.Childern)
            {
                temp += String.Format("- Số trẻ em thay đổi{0}->{1}", see.Childern, entity.Childern);
            }
            if (see.Vatin != entity.Vatin)
            {
                temp += String.Format("- VAT vào thay đổi: {0}->{1}", see.Vatin, entity.Vatin);
            }
            if (see.Vatout != entity.Vatout)
            {
                temp += String.Format("- VAT ra thay đổi: {0}->{1}", see.Vatout, entity.Vatout);
            }
            if (see.Httt != entity.Httt)
            {
                temp += String.Format("- HTTT thay đổi: {0}->{1}", see.Httt, entity.Httt);
            }

            if (see.Chinhanh != entity.Chinhanh)
            {
                temp += String.Format("- Chi nhánh thay đổi: {0}->{1}", see.Chinhanh, entity.Chinhanh);
            }

            #endregion

            see.Codedtq = entity.Codedtq;
            see.Serial = entity.Serial;
            see.Debit = entity.Debit;
            see.PaxPrice = string.IsNullOrEmpty(entity.PaxPrice.ToString()) ? 0 : entity.PaxPrice;
            see.Pax = string.IsNullOrEmpty(entity.Pax.ToString()) ? 0 : entity.Pax;
            see.ChildernPrice = string.IsNullOrEmpty(entity.ChildernPrice.ToString()) ? 0 : entity.ChildernPrice;
            see.Childern = string.IsNullOrEmpty(entity.Childern.ToString()) ? 0 : entity.Childern;
            see.Amount = (see.PaxPrice * see.Pax) + (see.ChildernPrice * see.Childern);
            see.Vatin = string.IsNullOrEmpty(entity.Vatin.ToString()) ? 0 : entity.Vatin;
            see.Vatout = string.IsNullOrEmpty(entity.Vatout.ToString()) ? 0 : entity.Vatout;
            see.Httt = entity.Httt;
            see.Chinhanh = HttpContext.Session.GetString("chinhanh");
            // see.logfile = see.logfile + "-User sửa sightseeing: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            // see.del = false;//trang thai chua xoa
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa sightseeing : " + username + " vào lúc: " + System.DateTime.Now.ToString();
                see.logfile = see.logfile + log;
            }
            _sightseeingRepository.Update(see);
            return new EmptyResult();
        }

        //Xoa hotel temp
        [HttpPost]
        public ActionResult DelSee(decimal id)
        {
            string username = HttpContext.Session.GetString("username");

            var seetemp = _sightseeingRepository.GetById(id);
            if (seetemp == null)
            {
                return NotFound();
            }
            // seetemp.del = true;
            //  seetemp.logfile = seetemp.logfile + "-User xóa sightseeing: " + username + " vào lúc: " + System.DateTime.Now.ToString();

            //log = System.Environment.NewLine;
            //log += "=============";
            //log += System.Environment.NewLine;
            //log += " -User xóa sightseeing : " + username + " vào lúc: " + System.DateTime.Now.ToString();
            //seetemp.logfile = seetemp.logfile + log;

            var result = _sightseeingRepository.Delete(seetemp);
            var tourprog = _tourprogRepository.Find(x => x.sgtcode == seetemp.sgtcode && x.stt == seetemp.Stt).FirstOrDefault();
            tourprog.logfile = tourprog.logfile + System.Environment.NewLine + "=============" + System.Environment.NewLine + "User " + username + " xoá dịch vụ sightseeing id= " + seetemp.Id + " vào lúc: " + System.DateTime.Now.ToString();
            _tourprogRepository.Update(tourprog);
            return Json(true);
        }

        #endregion

        #region Thao tác với dịch vụ LUN, DIN, BRK
        //---------- Cập nhật dịch vụ ăn trưa trong chương trình tour-----------
        public ActionResult EditLunchProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);

            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            switch (tourprog.srvtype)
            {
                case "BRK":
                    ViewBag.tieude="Ăn sáng tại nhà hàng";
                    break;
                case "LUN":
                    ViewBag.tieude = "Ăn trưa tại nhà hàng";
                    break;
                case "DIN":
                    ViewBag.tieude = "Ăn tối tại nhà hàng";
                    break;
                default:
                    ViewBag.tieude="Ăn tại nhà hàng";
                    break;
            }
            return PartialView("EditLunchProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditLunchProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (a.supplierid != tourProgTemp.supplierid)
            {
                temp += String.Format("- Nhà hàng thay đổi: {0}->{1}", a.supplierid, tourProgTemp.supplierid);
            }
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += String.Format("- Nơi book thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.currency != tourProgTemp.currency)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", a.currency, tourProgTemp.currency);
            }
            if (a.pax != tourProgTemp.pax)
            {
                temp += String.Format("- Tổng khách thay đổi: {0}->{1} ", a.pax, tourProgTemp.pax);
            }
            if (a.childern != tourProgTemp.childern)
            {
                temp += String.Format("- Số trẻ em thay đổi: {0}->{1} ", a.childern, tourProgTemp.childern);
            }
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1} ", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }

            if (a.vatin != tourProgTemp.vatin)
            {
                temp += String.Format("- VAT vào thay đổi: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += String.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }
            if (a.amount != tourProgTemp.amount)
            {
                temp += String.Format("- Tổng chi phí thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.amount, tourProgTemp.amount);
            }
            if (a.debit != tourProgTemp.debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit, tourProgTemp.debit);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            #endregion

            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.amount = tourProgTemp.amount;
            a.debit = tourProgTemp.debit;
            a.pax = tourProgTemp.pax;
            a.childern = tourProgTemp.childern;
            a.status = tourProgTemp.status;
            a.chinhanh = tourProgTemp.chinhanh;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }
        #endregion
        #region Thao tác với dịch vụ TXT


        // Cập nhật dịch vụ text của chương trình tour
        public ActionResult EditTextProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            var t = _tourinfRepository.GetById(tourprog.sgtcode);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            ViewBag.concernto = t.concernto;
            ViewBag.operators = t.operators;
           
            return PartialView("EditTextProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditTextProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1} ", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Khả thi thay đổi: {0}->{1} ", a.status, tourProgTemp.status);
            }
            if (a.dieuhanh != tourProgTemp.dieuhanh)
            {
                temp += String.Format("- Điều hành thay đổi: {0}->{1} ", a.dieuhanh, tourProgTemp.dieuhanh);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1} ", a.chinhanh, tourProgTemp.chinhanh);
            }
            #endregion

            a.sgtcode = tourProgTemp.sgtcode;
            a.stt = tourProgTemp.stt;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.status = tourProgTemp.status;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            a.chinhanh = tourProgTemp.chinhanh;
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }
        #endregion

        #region Thao tác với dịch vụ OTH


        //---------- Cập nhật dịch vụ tour trọn gói trong chương trình tour-----------
        public ActionResult EditOtherProg(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            return PartialView("EditOtherProg", tourprog);
        }
        [HttpPost]
        public ActionResult EditOtherProg(Tourprog tourProgTemp)
        {
            string username = HttpContext.Session.GetString("username");

            Tourprog a = _tourprogRepository.GetById(tourProgTemp.Id);
            a.Id = tourProgTemp.Id;

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (a.supplierid != tourProgTemp.supplierid)
            {
                temp += String.Format("- Nhà hàng thay đổi: {0}->{1}", a.supplierid, tourProgTemp.supplierid);
            }
            if (a.srvcode != tourProgTemp.srvcode)
            {
                temp += String.Format("- Nơi book thay đổi: {0}->{1}", a.srvcode, tourProgTemp.srvcode);
            }
            if (a.currency != tourProgTemp.currency)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", a.currency, tourProgTemp.currency);
            }
            if (a.srvnode != tourProgTemp.srvnode)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1} ", a.srvnode, tourProgTemp.srvnode);
            }
            if (a.unitpricea != tourProgTemp.unitpricea)
            {
                temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.unitpricea);
            }
            if (a.unitpricec != tourProgTemp.unitpricec)
            {
                temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.unitpricec);
            }
            if (a.amount != tourProgTemp.amount)
            {
                temp += String.Format("- Tổng chi phí thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.amount, tourProgTemp.amount);
            }
            if (a.vatin != tourProgTemp.vatin)
            {
                temp += String.Format("- VAT vào thay đổi: {0}->{1}", a.vatin, tourProgTemp.vatin);
            }
            if (a.vatout != tourProgTemp.vatout)
            {
                temp += String.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.vatout);
            }

            if (a.debit != tourProgTemp.debit)
            {
                temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit, tourProgTemp.debit);
            }
            if (a.status != tourProgTemp.status)
            {
                temp += String.Format("- Trạng thái khả thi thay đổi: {0}->{1}", a.status, tourProgTemp.status);
            }
            if (a.chinhanh != tourProgTemp.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.chinhanh);
            }
            if (a.dieuhanh != tourProgTemp.dieuhanh)
            {
                temp += String.Format("- Điều hành thay đổi: {0}->{1}", a.dieuhanh, tourProgTemp.dieuhanh);
            }
            #endregion

            a.supplierid = tourProgTemp.supplierid;
            a.srvcode = tourProgTemp.srvcode;
            a.currency = tourProgTemp.currency;
            a.tour_item = string.IsNullOrEmpty(tourProgTemp.tour_item) ? "" : tourProgTemp.tour_item;
            a.srvnode = string.IsNullOrEmpty(tourProgTemp.srvnode) ? "" : tourProgTemp.srvnode;
            a.unitpricea = tourProgTemp.unitpricea;
            a.unitpricec = tourProgTemp.unitpricec;
            a.amount = tourProgTemp.amount;
            a.vatin = tourProgTemp.vatin;
            a.vatout = tourProgTemp.vatout;
            a.debit = tourProgTemp.debit;
            a.status = tourProgTemp.status;
            a.dieuhanh = tourProgTemp.dieuhanh ?? "";
            a.chinhanh = tourProgTemp.chinhanh;
            //a.logfile = a.logfile + "-User sửa dịch vụ: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = Environment.NewLine;
                log += "=============";
                log += Environment.NewLine;
                log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                a.logfile = a.logfile + log;
            }
            _tourprogRepository.Update(a);
            return Json(true);
        }
        #endregion

        #region Thao tác với dịch vụ HTL trong chương trình tour


        //---------- Cập nhật dịch vụ hotel trong chương trình tour-----------
        public ActionResult EditHotelProg(decimal id)
        {
            TourprogHotelViewModel t = new TourprogHotelViewModel();

            var tourprog = _tourprogRepository.GetById(id);
            t.Tourprog = tourprog;
            listSupplier(tourprog.supplierid);
            listSrvcode(tourprog.srvcode);
            listNgoaite(tourprog.currency);
            listStatus(tourprog.status);
            listChinhanh(tourprog.chinhanh);
            listUserByChinhanh(tourprog.chinhanh, tourprog.dieuhanh);
            ViewBag.sgtcode = tourprog.sgtcode;
            ViewBag.stt = tourprog.stt;
            //chi lay dich vu hotel chua bi xoa
            Hotel hotel = _hotelRepository.Find(x => x.sgtcode == tourprog.sgtcode && x.stt == tourprog.stt).FirstOrDefault();
            t.Hotel = hotel;
            listNgoaite(t.Hotel.currency);
            return PartialView("EditHotelProg", t);
        }
        [HttpPost]
        public ActionResult EditHotelProg(TourprogHotelViewModel tourProgTemp)
        {
            if (ModelState.IsValid)
            {
                string username = HttpContext.Session.GetString("username");

                Tourprog a = _tourprogRepository.GetById(tourProgTemp.Tourprog.Id);
                a.Id = tourProgTemp.Tourprog.Id;

                #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
                temp = ""; log = "";
                if (a.supplierid != tourProgTemp.Tourprog.supplierid)
                {
                    temp += String.Format("- Khách sạn thay đổi: {0}->{1}", a.supplierid, tourProgTemp.Tourprog.supplierid);
                }
                if (a.pax != tourProgTemp.Tourprog.pax)
                {
                    temp += String.Format("- Khách người lớn thay đổi: {0}->{1}", a.pax, tourProgTemp.Tourprog.pax);
                }
                if (a.childern != tourProgTemp.Tourprog.childern)
                {
                    temp += String.Format("- Khách trẽ em thay đổi: {0}->{1}", a.childern, tourProgTemp.Tourprog.childern);
                }
                if (a.srvcode != tourProgTemp.Tourprog.srvcode)
                {
                    temp += String.Format("- Nơi book thay đổi: {0}->{1}", a.srvcode, tourProgTemp.Tourprog.srvcode);
                }
                if (!string.IsNullOrEmpty(a.currency) != !string.IsNullOrEmpty(tourProgTemp.Tourprog.currency))
                {
                    temp += String.Format("- Loại tiền thay đổi: {0}->{1}", a.currency, tourProgTemp.Tourprog.currency);
                }
                if (a.srvnode != tourProgTemp.Tourprog.srvnode)
                {
                    temp += String.Format("- Ghi chú thay đổi: {0}->{1} ", a.srvnode, tourProgTemp.Tourprog.srvnode);
                }
                if (a.status != tourProgTemp.Tourprog.status)
                {
                    temp += String.Format("- Khả thi thay đổi: {0}->{1} ", a.status, tourProgTemp.Tourprog.status);
                }
                if (a.unitpricea != tourProgTemp.Tourprog.unitpricea)
                {
                    temp += String.Format("- Giá người lớn thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricea, tourProgTemp.Tourprog.unitpricea);
                }
                if (a.unitpricec != tourProgTemp.Tourprog.unitpricec)
                {
                    temp += String.Format("- Giá trẻ em thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.unitpricec, tourProgTemp.Tourprog.unitpricec);
                }
                if (a.amount != tourProgTemp.Tourprog.amount)
                {
                    temp += String.Format("- Tổng chi phí thay đổi: {0:#,###0.0}->{1:#,##0.0}", a.amount, tourProgTemp.Tourprog.amount);
                }
                if (a.vatin != tourProgTemp.Tourprog.vatin)
                {
                    temp += String.Format("- VAT vào thay đổi: {0}->{1}", a.vatin, tourProgTemp.Tourprog.vatin);
                }
                if (a.vatout != tourProgTemp.Tourprog.vatout)
                {
                    temp += String.Format("- VAT ra thay đổi: {0}->{1}", a.vatout, tourProgTemp.Tourprog.vatout);
                }
                if (a.debit != tourProgTemp.Tourprog.debit)
                {
                    temp += String.Format("- Công nợ thay đổi: {0}->{1}", a.debit, tourProgTemp.Tourprog.debit);
                }
                if (a.chinhanh != tourProgTemp.Tourprog.chinhanh)
                {
                    temp += String.Format("- Chi nhánh triển khai thay đổi: {0}->{1}", a.chinhanh, tourProgTemp.Tourprog.chinhanh);
                }
                if (a.dieuhanh != tourProgTemp.Tourprog.dieuhanh)
                {
                    temp += String.Format("- Điều hành thay đổi: {0}->{1}", a.dieuhanh, tourProgTemp.Tourprog.dieuhanh);
                }
                if (a.foc != tourProgTemp.Tourprog.foc)
                {
                    temp += String.Format("- FOC thay đổi: {0}->{1}", a.foc, tourProgTemp.Tourprog.foc);
                }
                #endregion

                a.supplierid = tourProgTemp.Tourprog.supplierid;
                a.srvcode = tourProgTemp.Tourprog.srvcode;
                a.currency = tourProgTemp.Tourprog.currency;
                a.tour_item = string.IsNullOrEmpty(tourProgTemp.Tourprog.tour_item) ? "" : tourProgTemp.Tourprog.tour_item;
                a.srvnode = string.IsNullOrEmpty(tourProgTemp.Tourprog.srvnode) ? "" : tourProgTemp.Tourprog.srvnode.ToUpper();
                a.unitpricea = tourProgTemp.Tourprog.unitpricea;
                a.unitpricec = tourProgTemp.Tourprog.unitpricec;
                a.amount = tourProgTemp.Tourprog.amount;
                a.vatin = tourProgTemp.Tourprog.vatin;
                a.vatout = tourProgTemp.Tourprog.vatout;
                a.debit = tourProgTemp.Tourprog.debit;
                a.status = tourProgTemp.Tourprog.status;
                a.pax = tourProgTemp.Tourprog.pax;
                a.childern = tourProgTemp.Tourprog.childern;
                a.chinhanh = tourProgTemp.Tourprog.chinhanh;
                a.dieuhanh = tourProgTemp.Tourprog.dieuhanh ?? "";
                a.foc = tourProgTemp.Tourprog.foc;
                // a.logfile = a.logfile + "-User sửa dịch vụ hotel: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User sửa dịch vụ " + a.srvtype + ": " + username + " vào lúc: " + System.DateTime.Now.ToString();
                    a.logfile = a.logfile + log;
                }

                _tourprogRepository.Update(a);
                EditHotel(tourProgTemp.Hotel);
                return Json(true);
            }
            return View(tourProgTemp);
        }

        //Cap nhat Hotel Temp
        [HttpPost]
        //public ActionResult EditHotel(Hotel entity)
        public Hotel EditHotel(Hotel entity)
        {
            string username = HttpContext.Session.GetString("username");

            var hotel = _hotelRepository.GetById(entity.Id);

            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (hotel.currency != entity.currency)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", hotel.currency, entity.currency);
            }
            if (hotel.note != entity.note)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", hotel.note, entity.note);
            }
            if (hotel.sgl != entity.sgl)
            {
                temp += String.Format("- Phòng single thay đổi: {0}->{1}", hotel.sgl, entity.sgl);
            }
            if (hotel.sglpax != entity.sglpax)
            {
                temp += String.Format("- Số khách sgl thay đổi: {0}->{1}", hotel.sglpax, entity.sglpax);
            }
            if (hotel.sglcost != entity.sglcost)
            {
                temp += String.Format("- Giá sgl thay đổi: {0:#,##0.0}->{1:#,##0.0}", hotel.sglcost, entity.sglcost);
            }
            if (hotel.extsgl != entity.extsgl)
            {
                temp += String.Format("- Phòng ext thay đổi: {0}->{1}", hotel.extsgl, entity.extsgl);
            }

            if (hotel.extsglcost != entity.extsglcost)
            {
                temp += String.Format("- Giá ext thay đổi: {0:#,##0.0}->{1:#,##0.0}", hotel.extsglcost, entity.extsglcost);
            }
            if (hotel.dbl != entity.dbl)
            {
                temp += String.Format("- Phòng dbl thay đổi: {0}->{1}", hotel.dbl, entity.dbl);
            }
            if (hotel.dblpax != entity.dblpax)
            {
                temp += String.Format("- Số khách dbl thay đổi: {0}->{1}", hotel.dblpax, entity.dblpax);
            }
            if (hotel.dblcost != entity.dblcost)
            {
                temp += String.Format("- Giá dbl thay đổi: {0:#,##0.0}->{1:#,##0.0}", hotel.dblcost, entity.dblcost);
            }

            if (hotel.extdbl != entity.extdbl)
            {
                temp += String.Format("- Phòng extdbl thay đổi: {0}->{1}", hotel.extdbl, entity.extdbl);
            }

            if (hotel.extdblcost != entity.extdblcost)
            {
                temp += String.Format("- Giá extdbl thay đổi: {0:#,##0.0}->{1:#,##0.0}", hotel.extdblcost, entity.extdblcost);
            }

            if (hotel.twn != entity.twn)
            {
                temp += String.Format("- Phòng twn thay đổi: {0}->{1}", hotel.twn, entity.twn);
            }
            if (hotel.twnpax != entity.twnpax)
            {
                temp += String.Format("- Số khách twn thay đổi: {0}->{1}", hotel.twnpax, entity.twnpax);
            }
            if (hotel.twncost != entity.twncost)
            {
                temp += String.Format("- Giá twn thay đổi: {0:#,##0.0}->{1:#,##0.0}", hotel.twncost, entity.twncost);
            }

            if (hotel.exttwn != entity.exttwn)
            {
                temp += String.Format("- Phòng exttwn thay đổi: {0}->{1}", hotel.exttwn, entity.exttwn);
            }
            if (hotel.exttwncost != entity.exttwncost)
            {
                temp += String.Format("- Giá exttwn thay đổi: {0:#,##0.0}->{1:#,##0.0}", hotel.exttwncost, entity.exttwncost);
            }

            if (hotel.homestay != entity.homestay)
            {
                temp += String.Format("- Home stay thay đổi: {0}->{1}", hotel.homestay, entity.homestay);
            }
            if (hotel.homestaypax != entity.homestaypax)
            {
                temp += String.Format("- Số khách home stay thay đổi: {0}->{1}", hotel.homestaypax, entity.homestaypax); ;
            }
            if (hotel.homestaycost != entity.homestaycost)
            {
                temp += String.Format("- Giá home stay / khách thay đổi: {0:#,##0.0}->{1:#,##0.0}", hotel.homestaycost, entity.homestaycost);
            }

           
            if (hotel.oth != entity.oth)
            {
                temp += String.Format("- Phòng oth thay đổi: {0}->{1}", hotel.oth, entity.oth);
            }
            if (hotel.othpax != entity.othpax)
            {
                temp += String.Format("- Số khách oth thay đổi: {0}->{1}", hotel.othpax, entity.othpax);
            }
            if (hotel.othcost != entity.othcost)
            {
                temp += String.Format("- Giá oth thay đổi: {0:#,##0.0}->{1:#,##0.0}", hotel.othcost, entity.othcost);
            }
            if (hotel.othtype != entity.othtype)
            {
                temp += String.Format("- Loại oth thay đổi: {0}->{1}", hotel.othtype, entity.othtype);
            }
            #endregion
            hotel.currency = string.IsNullOrEmpty(entity.currency) ? "VND" : entity.currency;
            hotel.note = string.IsNullOrEmpty(entity.note) ? "" : entity.note;
            hotel.sgl = string.IsNullOrEmpty(entity.sgl.ToString()) ? 0 : entity.sgl;
            hotel.sglpax = string.IsNullOrEmpty(entity.sgl.ToString()) ? 0 : entity.sgl;
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
            hotel.othtype = string.IsNullOrEmpty(entity.othtype) ? "" : entity.othtype;
            //hotel.logfile = hotel.logfile + "-User sửa dịch vụ hotel: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User sửa dịch vụ  khách sạn: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                hotel.logfile = hotel.logfile + log;
            }
            _hotelRepository.Update(hotel);
            //return new EmptyResult();
            return hotel;
        }

        //Cap nhat Hotel Temp
        [HttpPost]
        //public ActionResult AddHotel(Hotel entity)
        public Hotel AddHotel(Hotel entity)
        {
            string username = HttpContext.Session.GetString("username");

            var hotel = new Hotel();
            hotel.sgtcode = entity.sgtcode;
            hotel.stt = entity.stt;
            hotel.currency = string.IsNullOrEmpty(entity.currency) ? "VND" : entity.currency.ToUpper();
            hotel.note = entity.note;
            hotel.sgl = string.IsNullOrEmpty(entity.sgl.ToString()) ? 0 : entity.sgl;
            hotel.sglpax = hotel.sgl;// string.IsNullOrEmpty(entity.sglpax.ToString()) ? 0 : entity.sglpax;
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

            hotel.logfile = "-User thêm dịch vụ hotel: " + username + " vào lúc: " + System.DateTime.Now.ToString();

            _hotelRepository.Create(hotel);
            //return Json(hotel);
            return hotel;
        }
        //Xoa hotel temp
        [HttpPost]
        public ActionResult DelHotel(decimal id)
        {
            string username = HttpContext.Session.GetString("username");

            var hoteltemp = _hotelRepository.GetById(id);
            if (hoteltemp == null)
            {
                return NotFound();
            }
            //hoteltemp.del = true;
            hoteltemp.logfile = hoteltemp.logfile + "-User xóa hotel: " + username + " vào lúc: " + System.DateTime.Now.ToString();

            var result = _hotelRepository.Delete(hoteltemp);
            var tourprog = _tourprogRepository.Find(x => x.sgtcode == hoteltemp.sgtcode && x.stt == hoteltemp.stt).SingleOrDefault();
            tourprog.logfile = tourprog.logfile + "-User xóa hotel: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            _tourprogRepository.Update(tourprog);
            return Json(true);
        }

        #endregion

        #region Thao tac ghi chu dau tour, cuoi tour 


        public ActionResult AddEditTourNote(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var t = _tourinfRepository.GetById(id);
            var tournote = _tournodeRepository.GetById(id);
            ViewBag.Sgtcode = id;
            ViewBag.concernto = t.concernto;
            ViewBag.operators = t.operators;
            return PartialView("AddEditTourNote", tournote);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddEditTourNote(Tournode tourTempNote)
        {
            var temp = _tournodeRepository.GetSingleNoTracking(x => x.Sgtcode == tourTempNote.Sgtcode);
            var result = new object();
            if (temp == null)
            {
                result = _tournodeRepository.Create(tourTempNote);
            }
            else
            {
                result = _tournodeRepository.Update(tourTempNote);
            }
            return Json(result);
        }

        #endregion

        #region Thao tac keo thả trên danh sách tour program 

        [HttpPost]

        public JsonResult CapNhatSTT([FromBody] string[] sortedIDs)
        {
            string lst = "";
            string username = HttpContext.Session.GetString("username");
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
                        Tourprog d = _tourprogRepository.GetById(id);
                        iIdOld = d.stt;
                        d.stt = i + 1;

                        string temp = "Thứ tự cũ: " + iIdOld + " đã đổi sang thứ tự mới: " + d.stt;
                        string log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
                        log += temp + " -Thứ tự thay đổi bởi: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                        d.logfile = d.logfile + log;


                        _tourprogRepository.Update(d);//update thutu


                        //cap nhat stt vao table dich vu tuong ung
                        switch (d.srvtype)
                        {
                            case "HTL": //hotel
                                List<Hotel> ht = _hotelRepository.GetLstHTLByCodeAndOrder(d.sgtcode, iIdOld);

                                foreach (Hotel htl in ht)
                                {
                                    htl.stt = d.stt;//cap nhat stt moi

                                    temp = "Thứ tự cũ: " + iIdOld + " đã đổi sang thứ tự mới: " + htl.stt;
                                    log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
                                    log += temp + " -Thứ tự thay đổi bởi: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                                    htl.logfile = htl.logfile + log;

                                    _hotelRepository.Update(htl);
                                }
                                break;
                            case "SSE": //hotel
                                List<Sightseeing> see = _sightseeingRepository.GetByCodeAndStt(d.sgtcode, iIdOld);

                                foreach (Sightseeing s in see)
                                {
                                    s.Stt = d.stt;//cap nhat stt moi

                                    temp = "Thứ tự cũ: " + iIdOld + " đã đổi sang thứ tự mới: " + s.Stt;
                                    log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
                                    log += temp + " -Thứ tự thay đổi bởi: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                                    s.logfile = s.logfile + log;

                                    _sightseeingRepository.Update(s);
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

        #region View log

        public ActionResult ViewLogTour(string code)
        {
            var tour = _tourinfRepository.GetById(code);
            ViewBag.log = tour.logfile;
            return PartialView("ViewLogTour", tour);
            //return tour.logfile.ToString();
        }

        public ActionResult ViewLog(decimal id)
        {
            var tourprog = _tourprogRepository.GetById(id);
            ViewBag.sgtcode = tourprog.sgtcode;

            switch (tourprog.srvtype)
            {
                case "SSE":
                    List<Sightseeing> sse = _sightseeingRepository.GetByCodeAndStt(tourprog.sgtcode, tourprog.stt);
                    ViewBag.logservice = sse;
                    break;

                case "HTL":
                    List<Hotel> htl = _hotelRepository.GetLstHTLByCodeAndOrder(tourprog.sgtcode, tourprog.stt);
                    ViewBag.logservice = htl;
                    break;
            }

            return PartialView("ViewLog", tourprog);
        }

        #endregion
        ////--------Thêm xoá sửa yêu cầu xe---------------------------
        #region thao tác thêm sửa yêu cầu xe
        public IActionResult ListYeucauxe(string id)
        {
            var xe = _dieuxeRepository.ListXe(id);
            ViewBag.cn = HttpContext.Session.GetString("chinhanh");
            foreach (var x in xe)
            {
                if (!String.IsNullOrEmpty(x.SupplierId))
                {
                    x.SupplierId = _supplierRepository.GetById(x.SupplierId).Tengiaodich;
                }
            }
            ViewBag.sgtcode = id;
            return PartialView("ListYeucauxe", xe);
        }
        public ActionResult AddXe(string code)
        {
            var t = _tourinfRepository.GetById(code);
            var dx = new Dieuxe();
            dx.Ngaydon = t.arr;
            dx.Denngay = t.dep;
            dx.Sgtcode = code;
            dx.Sokhach = t.pax;
            listSupplier("");
            listLoaixe("");
            listChinhanh(HttpContext.Session.GetString("chinhanh"));
            return PartialView("AddXe", dx);

        }

        [HttpPost]
        public ActionResult AddXe(Dieuxe model)
        {
            string username = HttpContext.Session.GetString("username");

            if (ModelState.IsValid)
            {
                model.Sttxe = _dieuxeRepository.newSttxe(model.Sgtcode);
                model.chinhanh = HttpContext.Session.GetString("chinhanh");
                model.Logfile = model.Logfile + "-User tạo điều xe: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                _dieuxeRepository.Create(model);
            }

            return Json(true);
        }
        public ActionResult EditXe(decimal id)
        {
            var dx = _dieuxeRepository.GetById(id);
            ViewBag.cartitle = "Cập nhật xe - Điều hành";
            listSupplier(dx.SupplierId);
            listLoaixe(dx.Loaixe);
            listChinhanh(dx.chinhanh);
            return PartialView("EditXe", dx);
        }
        [HttpPost]
        public ActionResult EditXe(Dieuxe entity)
        {
            string username = HttpContext.Session.GetString("username");
            var dx = _dieuxeRepository.GetById(entity.Idxe);
            //chi gan lai cac gia tri tren giao dien edit


            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (dx.Ngaydon != entity.Ngaydon)
            {
                temp += String.Format("- Ngày đón thay đổi:  {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", dx.Ngaydon, entity.Ngaydon);
            }
            if (dx.Giodon != entity.Giodon)
            {
                temp += String.Format("- Giờ đón thay đổi: {0}->{1}", dx.Giodon, entity.Giodon);
            }
            if (dx.Diemdon != entity.Diemdon)
            {
                temp += String.Format("- Điểm đón thay đổi: {0}->{1}", dx.Diemdon, entity.Diemdon);
            }
            if (dx.Denngay != entity.Denngay)
            {
                temp += String.Format("- Đến ngày thay đổi:  {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", dx.Denngay, entity.Denngay);
            }
            if (dx.Sokhach != entity.Sokhach)
            {
                temp += String.Format("- Số khách thay đổi: {0:#,###0}->{1:#,##0}", dx.Sokhach, entity.Sokhach);
            }
            if (dx.Loaixe != entity.Loaixe)
            {
                temp += String.Format("- Loại xe thay đổi: {0}->{1}", dx.Loaixe, entity.Loaixe);
            }
            if (dx.Soxe != entity.Soxe)
            {
                temp += String.Format("- Số xe thay đổi: {0}->{1}", dx.Soxe, entity.Soxe);
            }
            if (dx.Laixe != entity.Laixe)
            {
                temp += String.Format("- Lái xe thay đổi: {0}->{1}", dx.Laixe, entity.Laixe);
            }
            if (dx.Dienthoai != entity.Dienthoai)
            {
                temp += String.Format("- Điện thoại thay đổi: {0}->{1}", dx.Dienthoai, entity.Dienthoai);
            }
            if (dx.Ghichu != entity.Ghichu)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", dx.Ghichu, entity.Ghichu);
            }

            if (dx.Km != entity.Km)
            {
                temp += String.Format("- Số KM thay đổi: {0:#,###0}->{1:#,##0}", dx.Km, entity.Km);
            }
            if (dx.Dongiakm != entity.Dongiakm)
            {
                temp += String.Format("- Đơn giá/KM(VND) thay đổi: {0:#,###0.0}->{1:#,##0.0}", dx.Dongiakm, entity.Dongiakm);
            }
            if (dx.Kmnl != entity.Kmnl)
            {
                temp += String.Format("- KM nhiên liệu thay đổi: {0:#,###0}->{1:#,##0}", dx.Kmnl, entity.Kmnl);
            }
            if (dx.SupplierId != entity.SupplierId)
            {
                temp += String.Format("- Chủ xe thay đổi: {0}->{1}", dx.SupplierId, entity.SupplierId);
            }
            if (dx.Lotrinh != entity.Lotrinh)
            {
                temp += String.Format("- Lộ trình thay đổi: {0}->{1}", dx.Lotrinh, entity.Lotrinh);
            }
            if (dx.Chiphi != entity.Chiphi)
            {
                temp += String.Format("- Chi phí thuê ngoài thay đổi: {0:#,###0.0}->{1:#,##0.0}", dx.Chiphi, entity.Chiphi);
            }
            if (dx.chinhanh != entity.chinhanh)
            {
                temp += String.Format("- Triển khai thay đổi: {0}->{1}", dx.chinhanh, entity.chinhanh);
            }
            #endregion

            dx.Ngaydon = entity.Ngaydon;
            dx.Giodon = entity.Giodon;
            dx.Diemdon = entity.Diemdon;
            dx.Denngay = entity.Denngay;
            dx.Sokhach = entity.Sokhach;
            dx.Loaixe = entity.Loaixe;
            dx.Ghichu = entity.Ghichu;
            dx.Lotrinh = entity.Lotrinh;
            dx.chinhanh = entity.chinhanh;
            dx.Laixe = entity.Laixe;
            //cac dong comment do ben doi xe edit
            //dx.Soxe = entity.Soxe;
            //dx.Laixe = entity.Laixe;
            //dx.Dienthoai = entity.Dienthoai;
            //dx.Km = entity.Km;
            //dx.Dongiakm = entity.Dongiakm;
            //dx.Kmnl = entity.Kmnl;
            //dx.SupplierId = entity.SupplierId;
            //dx.Chiphi = entity.Chiphi;


            if (temp.Length > 0)
            {
                log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
                log += temp + " -User sửa điều xe : " + username + " vào lúc: " + System.DateTime.Now.ToString();
                dx.Logfile = dx.Logfile + log;
            }
            _dieuxeRepository.Update(dx);

            return Json(true);
        }

        public ActionResult ViewLogDieuXe(decimal id)
        {
            var dx = _dieuxeRepository.GetById(id);
            ViewBag.sgtcode = dx.Sgtcode;
            return PartialView("ViewLogDieuXe", dx);
        }

        [HttpPost]
        public ActionResult DelXe(decimal id)
        {
            string username = HttpContext.Session.GetString("username");

            var dx = _dieuxeRepository.GetById(id);
            if (dx == null)
            {
                return NotFound();
            }
            dx.del = true;

            log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
            log += " -User xóa điều xe : " + username + " vào lúc: " + System.DateTime.Now.ToString();
            dx.Logfile = dx.Logfile + log;

            var result = _dieuxeRepository.Update(dx);
            return Json(true);
        }
        #endregion
        ////--------Thêm xoá sửa yêu cầu hướng dẫn---------------------------
        #region thao tác thêm sửa yêu cầu hướng dẫn
        public IActionResult ListHuongdan(string id)
        {
            var hd = _huongdanRepository.ListHuongdan(id);
            ViewBag.cn = HttpContext.Session.GetString("chinhanh");
            ViewBag.sgtcode = id;
            return PartialView("ListHuongdan", hd);
        }

        public ActionResult AddHuongdan(string id)//id = sgtcode
        {
            var tourinf = _tourinfRepository.GetById(id);
            var hd = new Huongdan();
            hd.Sgtcode = id;
            hd.Batdau = tourinf.arr;
            hd.Ketthuc = tourinf.dep;
            listNgoaite("VND");
            listChinhanh(HttpContext.Session.GetString("chinhanh"));
            listSupplier("");
            return PartialView("AddHuongdan", hd);
        }

        [HttpPost]
        public ActionResult AddHuongdan(Huongdan model)
        {
            string username = HttpContext.Session.GetString("username");

            if (ModelState.IsValid)
            {
                model.Stt = _huongdanRepository.newStthd(model.Sgtcode);
                model.Ngoaingu = string.IsNullOrEmpty(model.Ngoaingu) ? "" : model.Ngoaingu.ToUpper();
                model.chinhanh = model.chinhanh;
                model.Logfile = model.Logfile + "-User tạo hướng dẫn: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                model.Ngayyeucau = DateTime.Now;
                _huongdanRepository.Create(model);
            }

            return Json(true);
        }
        public ActionResult EditHuongdan(decimal id)
        {
            var hd = _huongdanRepository.GetById(id);
            listNgoaite(hd.Loaitien);
            listChinhanh(hd.chinhanh);
            listSupplier(hd.hopdongcty);
            return PartialView("EditHuongdan", hd);
        }
        [HttpPost]
        public ActionResult EditHuongdan(Huongdan entity)
        {
            string username = HttpContext.Session.GetString("username");
            var dx = _huongdanRepository.GetById(entity.IdHuongdan);
            //chi gan lai cac gia tri tren giao dien edit


            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (dx.Tenhd != entity.Tenhd)
            {
                temp += String.Format("- Tên hướng dẫn thay đổi: {0}->{1}", dx.Tenhd, entity.Tenhd);
            }
            if (dx.Dienthoai != entity.Dienthoai)
            {
                temp += String.Format("- Điện thoại thay đổi: {0}->{1}", dx.Dienthoai, entity.Dienthoai);
            }
            if (dx.Ngoaingu != entity.Ngoaingu)
            {
                temp += String.Format("- Ngoại ngữ thay đổi: {0}->{1}", dx.Ngoaingu, entity.Ngoaingu);
            }
            if (dx.hopdongcty != entity.hopdongcty)
            {
                temp += String.Format("- Hướng dẫn thay đổi: {0}->{1}", dx.hopdongcty, entity.hopdongcty);
            }
            if (dx.Batdau != entity.Batdau)
            {
                temp += String.Format("- Đi tour từ thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", dx.Batdau, entity.Batdau);
            }
            if (dx.Batdautai != entity.Batdautai)
            {
                temp += String.Format("- Đi tour tại thay đổi: {0}->{1}", dx.Batdautai, entity.Batdautai);
            }
            if (dx.Ketthuc != entity.Ketthuc)
            {
                temp += String.Format("- Kết thúc tour thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", dx.Ketthuc, entity.Ketthuc);
            }
            if (dx.Ketthuctai != entity.Ketthuctai)
            {
                temp += String.Format("- Kết thúc tại thay đổi: {0}->{1}", dx.Ketthuctai, entity.Ketthuctai);
            }
            if (dx.Suottuyen != entity.Suottuyen)
            {
                temp += String.Format("- Suốt tuyến thay đổi: {0}->{1}", dx.Suottuyen, entity.Suottuyen);
            }
            if (dx.Ghichu != entity.Ghichu)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", dx.Ghichu, entity.Ghichu);
            }

            if (dx.Ndcongviec != entity.Ndcongviec)
            {
                temp += String.Format("- Nội dung đi tour thay đổi: {0}->{1}", dx.Ndcongviec, entity.Ndcongviec);
            }
            if (dx.Loaitien != entity.Loaitien)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", dx.Loaitien, entity.Loaitien);
            }
            if (dx.Phidontien != entity.Phidontien)
            {
                temp += String.Format("- Phí đón tiển thay đổi: {0:#,###0.0}->{1:#,##0.0}", dx.Phidontien, entity.Phidontien);
            }
            if (dx.Phididoan != entity.Phididoan)
            {
                temp += String.Format("- Phí đi đoàn thay đổi: {0:#,###0.0}->{1:#,##0.0}", dx.Phididoan, entity.Phididoan);
            }
            if (dx.Traphi != entity.Traphi)
            {
                temp += String.Format("- Trả phí thay đổi: {0:#,###0.0}->{1:#,##0.0}", dx.Traphi, entity.Traphi);
            }
            if (dx.chinhanh != entity.chinhanh)
            {
                temp += String.Format("- Chi nhánh thay đổi: {0}->{1}", dx.chinhanh, entity.chinhanh);
            }
            #endregion

            dx.Ngoaingu = entity.Ngoaingu;
            dx.Batdautai = entity.Batdautai;
            dx.Batdau = entity.Batdau;
            dx.Ketthuc = entity.Ketthuc;
            dx.Ketthuctai = entity.Ketthuctai;
            dx.Suottuyen = entity.Suottuyen;
            dx.Ghichu = entity.Ghichu;
            dx.Ndcongviec = entity.Ndcongviec;
            dx.chinhanh = entity.chinhanh;
            dx.Tenhd = entity.Tenhd;
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
                log += temp + " -User sửa thông tin hướng dẫn : " + username + " vào lúc: " + System.DateTime.Now.ToString();
                dx.Logfile = dx.Logfile + log;
            }
            _huongdanRepository.Update(dx);

            return Json(true);
        }

        public ActionResult ViewLogHuongDan(decimal id)
        {
            var dx = _huongdanRepository.GetById(id);
            ViewBag.sgtcode = dx.Sgtcode;
            return PartialView("ViewLogHuongDan", dx);
        }

        [HttpPost]
        public ActionResult DelHuongdan(decimal id)
        {
            string username = HttpContext.Session.GetString("username");

            var dx = _huongdanRepository.GetById(id);
            if (dx == null)
            {
                return NotFound();
            }
            dx.del = true;

            log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
            log += " -User xóa hướng dẫn : " + username + " vào lúc: " + System.DateTime.Now.ToString();
            dx.Logfile = dx.Logfile + log;

            var result = _huongdanRepository.Update(dx);
            return Json(true);
        }
        #endregion

        ////--------thao tác thêm sửa danh sách khách---------------------------
        #region thao tác thêm sửa danh sách khách

        public IActionResult ListDsKhach(string id)
        {
            var hd = _khachtourRepository.ListKhachTour(id).ToList();
            listQuocgia("VIETNAM");
            ViewBag.cn = HttpContext.Session.GetString("chinhanh");
            ViewBag.sgtcode = id;
            return PartialView("ListDsKhach", hd);
        }

        public ActionResult ImportDsKhach(string id)
        {
            var hd = _khachtourRepository.ListKhachTour(id);
            FileUploadViewModel model = new FileUploadViewModel
            {
                lstkhach = hd,
                sgtcode = id
            };

            return PartialView("ImportDsKhach", model);
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        public ActionResult AddKhach(string code)
        {
            listLoaiPhong("");
            listPhai("");
            listQuocgia("VIETNAM");
            ViewBag.sgtcode = code;
            KhachTour model = new KhachTour();
            model.sgtcode = code;
            model.stt = _khachtourRepository.newStt(code);
            return PartialView("AddKhach", model);

        }

        [HttpPost]
        public ActionResult AddKhach(KhachTour model)
        {
            string username = HttpContext.Session.GetString("username");

            if (ModelState.IsValid)
            {
                model.stt = _khachtourRepository.newStt(model.sgtcode);
                model.hoten = model.hoten ?? "";
                model.makh = model.makh ?? "";
                model.diachi = model.diachi ?? "";
                model.dienthoai = model.dienthoai ?? "";
                model.loaiphong = model.loaiphong ?? "";
                model.hochieu = model.hochieu ?? "";
                model.cmnd = model.cmnd ?? "";
                model.quoctich = model.quoctich ?? "VIETNAM";

                model.del = false;
                model.Logfile = model.Logfile + "-User tạo khách: " + username + " vào lúc: " + System.DateTime.Now.ToString();
                try
                {
                    _khachtourRepository.Create(model);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }

            }
            return Json(true);
        }

        public ActionResult EditKhach(decimal id)
        {
            var hd = _khachtourRepository.GetById(id);
            listLoaiPhong(hd.loaiphong);
            listPhai(hd.phai.ToString());
            listQuocgia(hd.quoctich);
            return PartialView("EditKhach", hd);
        }

        [HttpPost]
        public ActionResult EditKhach(KhachTour entity)
        {
            string username = HttpContext.Session.GetString("username");
            var dx = _khachtourRepository.GetById(entity.IdKhach);
            //chi gan lai cac gia tri tren giao dien edit
            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (dx.stt != entity.stt)
            {
                temp += String.Format("- STT thay đổi: {0}->{1}", dx.stt, entity.stt);
            }
            if (dx.makh != entity.makh)
            {
                temp += String.Format("- Mã KH thay đổi: {0}->{1}", dx.makh, entity.makh);
            }
            if (dx.hoten != entity.hoten)
            {
                temp += String.Format("- Họ tên khách hàng thay đổi: {0}->{1}", dx.hoten, entity.hoten);
            }
            if (dx.ngaysinh != entity.ngaysinh)
            {
                temp += String.Format("- Ngày sinh thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", dx.ngaysinh, entity.ngaysinh);
            }
            if (dx.phai != entity.phai)
            {
                temp += String.Format("- Phái thay đổi: {0}->{1}", dx.phai, entity.phai);
            }

            if (!string.IsNullOrEmpty(dx.hochieu) != !string.IsNullOrEmpty(entity.hochieu))
            {
                temp += String.Format("- Hộ chiếu thay đổi: {0}->{1}", dx.hochieu, entity.hochieu);
            }
            if (!string.IsNullOrEmpty(dx.cmnd) != !string.IsNullOrEmpty(entity.cmnd))
            {
                temp += String.Format("- CMND thay đổi: {0}->{1}", dx.cmnd, entity.cmnd);
            }
            if (!string.IsNullOrEmpty(dx.diachi) != !string.IsNullOrEmpty(entity.diachi))
            {
                temp += String.Format("- Địa chỉ thay đổi: {0}->{1}", dx.diachi, entity.diachi);
            }
            if (!string.IsNullOrEmpty(dx.dienthoai) != !string.IsNullOrEmpty(entity.dienthoai))
            {
                temp += String.Format("- Điện thoại thay đổi: {0}->{1}", dx.dienthoai, entity.dienthoai);
            }
            if (!string.IsNullOrEmpty(dx.quoctich) != !string.IsNullOrEmpty(entity.quoctich))
            {
                temp += String.Format("- Quốc tịch thay đổi: {0}->{1}", dx.quoctich, entity.quoctich);
            }
            if (dx.loaiphong != entity.loaiphong)
            {
                temp += String.Format("- Loại phòng thay đổi: {0}->{1}", dx.loaiphong, entity.loaiphong);
            }
            if (!string.IsNullOrEmpty(dx.prn) != !string.IsNullOrEmpty(entity.prn))
            {
                temp += String.Format("- PRN thay đổi: {0}->{1}", dx.prn, entity.prn);
            }
            if (!string.IsNullOrEmpty(dx.ghichu) != !string.IsNullOrEmpty(entity.ghichu))
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", dx.ghichu, entity.ghichu);
            }
            if (dx.vmb != entity.vmb)
            {
                temp += String.Format("- Vé máy bay thay đổi: {0}->{1}", dx.vmb, entity.vmb);
            }
            #endregion

            dx.stt = entity.stt;
            dx.sgtcode = entity.sgtcode;
            dx.makh = entity.makh ?? "";
            dx.hoten = entity.hoten ?? "";
            dx.ngaysinh = entity.ngaysinh;
            dx.phai = entity.phai;
            dx.diachi = entity.diachi ?? "";
            dx.dienthoai = entity.dienthoai ?? "";
            dx.loaiphong = entity.loaiphong ?? "";
            dx.hochieu = entity.hochieu ?? "";
            dx.cmnd = entity.cmnd ?? "";
            dx.ngaycaphc = entity.ngaycaphc;
            dx.hieuluchc = entity.hieuluchc;
            dx.prn = entity.prn;
            dx.ghichu = entity.ghichu;
            dx.quoctich = entity.quoctich ?? "VIETNAM";

            if (temp.Length > 0)
            {
                log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
                log += temp + " -User sửa thông tin khách tour : " + username + " vào lúc: " + System.DateTime.Now.ToString();
                dx.Logfile = dx.Logfile + log;
            }
            _khachtourRepository.Update(dx);

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> ImportDSK([FromForm] FileUploadViewModel model, CancellationToken cancellationToken)
        {
            string username = HttpContext.Session.GetString("username");
            var list = new List<KhachTour>();

            foreach (IFormFile formFile in model.files)
            {
                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase) && !Path.GetExtension(formFile.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    return Json("Không phải file dạng Excel!");
                }
                else
                {
                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream, cancellationToken);

                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;
                            string sNgaySinh = "";
                            for (int row = 2; row <= rowCount; row++)
                            {
                                sNgaySinh = worksheet.Cells[row, 3].Value.ToString() ?? "";
                                list.Add(new KhachTour
                                {
                                    stt = int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                                    sgtcode = model.sgtcode,
                                    makh = "",
                                    diachi = "",
                                    dienthoai = "",
                                    hoten = string.IsNullOrEmpty(worksheet.Cells[row, 2].Value.ToString().Trim()) ? "" : worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    ngaysinh = sNgaySinh == "" ? DateTime.MinValue : DateTime.Parse(sNgaySinh),
                                    phai = worksheet.Cells[row, 4].Value.ToString() == "1" ? true : false,
                                    loaiphong = worksheet.Cells[row, 5].Value.ToString().Trim(),
                                    hochieu = string.IsNullOrEmpty(worksheet.Cells[row, 6].Value.ToString().Trim()) ? "" : worksheet.Cells[row, 6].Value.ToString().Trim(),
                                    cmnd = string.IsNullOrEmpty(worksheet.Cells[row, 7].Value.ToString().Trim()) ? "" : worksheet.Cells[row, 7].Value.ToString().Trim(),
                                    quoctich = string.IsNullOrEmpty(worksheet.Cells[row, 8].Value.ToString().Trim()) ? "" : worksheet.Cells[row, 8].Value.ToString().Trim(),
                                    del = false,
                                    Logfile = "-User import từ excel khách tour: " + username + " vào lúc: " + System.DateTime.Now.ToString()

                                });

                            }
                        }
                    }

                    try
                    {
                        foreach (KhachTour k in list)
                        {
                            _khachtourRepository.Create(k);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(ex.Message);
                    }
                }//dung file excel               
            }

            return Json("OK");
        }

        public ActionResult ViewLogKhach(decimal id)
        {
            var dx = _khachtourRepository.GetById(id);
            ViewBag.sgtcode = dx.sgtcode;
            return PartialView("ViewLogKhach", dx);
        }

        [HttpPost]
        public ActionResult DelKhach(decimal id)
        {
            string username = HttpContext.Session.GetString("username");

            var dx = _khachtourRepository.GetById(id);
            if (dx == null)
            {
                return NotFound();
            }
            dx.del = true;

            log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
            log += " -User xóa khách : " + username + " vào lúc: " + System.DateTime.Now.ToString();
            dx.Logfile = dx.Logfile + log;

            var result = _khachtourRepository.Update(dx);
            return Json(true);
        }
        [HttpGet]
        public ActionResult searchKhach(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return PartialView("searchKhach");
            }
            else
            {
                search = search.ToLower();
                ViewBag.search = search;
                var k = _khachtourRepository.GetAll().Where(x => x.hoten.ToLower().Contains(search) || x.hochieu.Contains(search) || x.cmnd.Contains(search) || x.sgtcode.Contains(search)).ToList();
                ViewBag.k = k.Count();
                return PartialView("searchKhach", k);
            }

        }
        [HttpPost]
        public ActionResult SaveVemaybay(string list)
        {
            var idList = JsonConvert.DeserializeObject<List<KhachTour>>(list);
            foreach (var i in idList)
            {
                temp = ""; log = "";
                var k = _khachtourRepository.GetById(i.IdKhach);
                if (k.vmb != i.vmb)
                {
                    temp += String.Format("- Vé máy bay thay đổi: {0}->{1}", k.vmb, i.vmb);
                }
                k.vmb = i.vmb;
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
                    log += temp + " -User sửa thông tin vé máy bay : " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString();
                    k.Logfile = k.Logfile + log;
                }
                _khachtourRepository.Update(k);
            }

            return Json(true);
        }
        #endregion

        ////--------thao tác thêm sửa chi phí khác---------------------------
        #region "Chi phí khác"
        public IActionResult ListCPKhac(string id)//id = sgtcode
        {
            List<Chiphikhac> cp = _chiphiRepository.GetlstCP(id);
            ViewBag.cn = HttpContext.Session.GetString("chinhanh");
            ViewBag.sgtcode = id;
            List<ChiphikhacViewModel> lst = new List<ChiphikhacViewModel>();
            foreach (Chiphikhac c in cp)
            {
                ChiphikhacViewModel v = new ChiphikhacViewModel();
                v.Chiphikhac = c;
                try
                {
                    v.Tendv = _dichvuRepository.GetById(c.srvtype).Tendv;
                }
                catch { }
                try
                {
                    v.Tengiaodich = _supplierRepository.GetById(c.srvcode).Tengiaodich;
                }
                catch { }
                lst.Add(v);
            }

            return PartialView("ListCPKhac", lst);
        }

        public ActionResult AddCP(string code)
        {
            listSrvcode("");
            listDichvu("TXT");
            listNgoaite("VND");
            listChinhanh(HttpContext.Session.GetString("chinhanh"));
            ViewBag.sgtcode = code;
            Chiphikhac model = new Chiphikhac();
            model.sgtcode = code;
            return PartialView("AddCP", model);

        }

        [HttpPost]
        public ActionResult AddCP(Chiphikhac model)
        {
            string username = HttpContext.Session.GetString("username");
            string sSrvtype = "", sSrvcode = "", sSrvnode = "";
            sSrvtype = model.srvtype ?? " ";
            sSrvcode = model.srvcode ?? " ";
            sSrvnode = model.srvnode ?? " ";
            try
            {
                sSrvtype = _dichvuRepository.GetById(sSrvtype).Tendv;
            }
            catch { }
            try
            {
                sSrvcode = _supplierRepository.GetById(sSrvcode).Tengiaodich;
            }
            catch { }

            model.tour_item = sSrvtype + " " + sSrvcode + "" + sSrvnode;
            model.logfile = model.logfile + "-User tạo chi phí khác: " + username + " vào lúc: " + System.DateTime.Now.ToString();
            _chiphiRepository.Create(model);

            return Json(true);
        }

        public ActionResult EditCP(decimal id)
        {
            var hd = _chiphiRepository.GetById(id);
            listSrvcode(hd.srvcode);
            listDichvu(hd.srvtype);
            listNgoaite(hd.currency);
            listChinhanh(hd.chinhanh);
            return PartialView("EditCP", hd);
        }

        [HttpPost]
        public ActionResult EditCP(Chiphikhac entity)
        {
            string username = HttpContext.Session.GetString("username");
            var dx = _chiphiRepository.GetById(entity.idorthercost);
            //chi gan lai cac gia tri tren giao dien edit
            #region Kiểm tra nếu có thay đổi thì cập nhật, không thì thôi
            temp = ""; log = "";
            if (dx.fromdate != entity.fromdate)
            {
                temp += String.Format("- Từ ngày thay đổi: {0}->{1}", dx.fromdate, entity.fromdate);
            }
            if (dx.todate != entity.todate)
            {
                temp += String.Format("- Đến ngày thay đổi: {0}->{1}", dx.todate, entity.todate);
            }
            if (dx.srvtype != entity.srvtype)
            {
                temp += String.Format("- Code dịch vụ thay đổi: {0}->{1}", dx.srvtype, entity.srvtype);
            }
            if (dx.srvcode != entity.srvcode)
            {
                temp += String.Format("- Tên đơn vị cung ứng thay đổi: {0}->{1}", dx.srvcode, entity.srvcode);
            }

            if (dx.quantity != entity.quantity)
            {
                temp += String.Format("- Số lượng thay đổi: {0}->{1}", dx.quantity, entity.quantity);
            }
            if (dx.unitprice != entity.unitprice)
            {
                temp += String.Format("- Đơn giá thay đổi: {0}->{1}", dx.unitprice, entity.unitprice);
            }
            if (dx.km != entity.km)
            {
                temp += String.Format("- Km thay đổi: {0}->{1}", dx.km, entity.km);
            }

            if (dx.guidedays != entity.guidedays)
            {
                temp += String.Format("- Km thay đổi: {0}->{1}", dx.guidedays, entity.guidedays);
            }
            if (dx.amount != entity.amount)
            {
                temp += String.Format("- Tổng chi phí thay đổi: {0}->{1}", dx.amount, entity.amount);
            }
            if (dx.debit != entity.debit)
            {
                temp += String.Format("- Phải trả thay đổi: {0}->{1}", dx.debit, entity.debit);
            }
            if (dx.credit != entity.credit)
            {
                temp += String.Format("- Phải thu thay đổi: {0}->{1}", dx.credit, entity.credit);
            }
            if (dx.currency != entity.currency)
            {
                temp += String.Format("- Loại tiền thay đổi: {0}->{1}", dx.currency, entity.currency);
            }
            if (dx.vatin != entity.vatin)
            {
                temp += String.Format("- Vat vào thay đổi: {0}->{1}", dx.vatin, entity.vatin);
            }

            if (dx.vatout != entity.vatout)
            {
                temp += String.Format("- Vat ra thay đổi: {0}->{1}", dx.vatout, entity.vatout);
            }

            if (dx.srvprofit != entity.srvprofit)
            {
                temp += String.Format("- DV phí thay đổi: {0}->{1}", dx.srvprofit, entity.srvprofit);
            }

            if (dx.srvnode != entity.srvnode)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", dx.srvnode, entity.srvnode);
            }
            if (dx.chinhanh != entity.chinhanh)
            {
                temp += String.Format("- Chi nhánh thay đổi: {0}->{1}", dx.chinhanh, entity.chinhanh);
            }
            #endregion

            dx.fromdate = entity.fromdate;
            dx.todate = entity.todate;
            dx.srvtype = entity.srvtype;
            dx.srvcode = entity.srvcode;
            string sSrvtype = "", sSrvcode = "", sSrvnode = "";
            sSrvtype = entity.srvtype ?? " ";
            sSrvcode = entity.srvcode ?? " ";
            sSrvnode = entity.srvnode ?? " ";

            try
            {
                sSrvtype = _dichvuRepository.GetById(sSrvtype).Tendv;
            }
            catch { }
            try
            {
                sSrvcode = _supplierRepository.GetById(sSrvcode).Tengiaodich;
            }
            catch { }

            dx.tour_item = sSrvtype + " - " + sSrvcode + " - " + sSrvnode;

            dx.quantity = entity.quantity;
            dx.unitprice = entity.unitprice;
            dx.guidedays = entity.guidedays;
            dx.km = entity.km;
            dx.amount = entity.amount;
            dx.debit = entity.debit;
            dx.credit = entity.credit;
            dx.vatin = entity.vatin;
            dx.vatout = entity.vatout;
            dx.srvprofit = entity.srvprofit;
            dx.srvnode = entity.srvnode;
            dx.chinhanh = entity.chinhanh;
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
                log += temp + " -User sửa thông tin chi phí khác : " + username + " vào lúc: " + System.DateTime.Now.ToString();
                dx.logfile = dx.logfile + log;
            }
            _chiphiRepository.Update(dx);

            return Json(true);
        }

        [HttpPost]
        public ActionResult DelCP(decimal id)
        {
            string username = HttpContext.Session.GetString("username");

            var dx = _chiphiRepository.GetById(id);
            if (dx == null)
            {
                return NotFound();
            }
            dx.del = true;

            log = System.Environment.NewLine + "=============" + System.Environment.NewLine;
            log += " -User xóa chi phí khác : " + username + " vào lúc: " + System.DateTime.Now.ToString();
            dx.logfile = dx.logfile + log;

            var result = _chiphiRepository.Update(dx);
            return Json(true);
        }

        public ActionResult ViewLogCP(decimal id)
        {
            var dx = _chiphiRepository.GetById(id);
            ViewBag.sgtcode = dx.sgtcode;
            return PartialView("ViewLogCP", dx);
        }
        #endregion

        ////--------thao tác booking---------------------------
        #region Thao tác booking
        [HttpGet]
        public ActionResult Booking(string sgtcode)
        {
            HttpContext.Session.SetString("urlBooking", UriHelper.GetDisplayUrl(Request));
            var tour = _tourinfRepository.GetById(sgtcode);
            ViewBag.sgtcode = sgtcode;
            ViewBag.tour = sgtcode + " => " + tour.arr.ToString("dd/MM/yyyy") + " - " + tour.dep.ToString("dd/MM/yyyy");
            ViewBag.date = System.DateTime.Now.ToString("dd/MM/yyyy");

            Booked b = new Booked();
            b.Date = System.DateTime.Now;

            List<SupplierByCode> supplierByCode = _bookedRepository.listSupplierByCode(sgtcode, HttpContext.Session.GetString("chinhanh")).Distinct().ToList();


            ViewBag.listSupplier = supplierByCode;
            ViewBag.count = supplierByCode.Count();
            if (supplierByCode.Count == 0)
            {
                return View("Booking");
            }
            else
            {
                return View("Booking", b);
            }
        }
        [HttpPost]
        public ActionResult Booking(Booked entity)
        {
            //if (string.IsNullOrEmpty(entity.Supplierid))
            //{
            //    SetAlert("Vui lòng chọn nhà cung cấp cần confirm", "error");
            //    return Redirect(HttpContext.Session.GetString("urlBooking"));
            //}
            // string note = bookstring(entity.Sgtcode, entity.Supplierid);
            //#region Tạo profile booking
            //var dichvu = _tourprogRepository.Find(x => x.sgtcode == entity.Sgtcode && x.supplierid == entity.Supplierid).ToList();
            //string note =  "Ngày " + System.DateTime.Now.ToString("dd/MM/yyyy") + System.Environment.NewLine;

            //if (entity.Times > 0)
            //{
            //    note += " Thay đổi lần thứ " + entity.Times;
            //}

            //note += System.Environment.NewLine + "Gửi cho:                Nội dung như sau:" + System.Environment.NewLine;
            //foreach (var item in dichvu)
            //{

            //    var supplier = _supplierRepository.GetById(item.supplierid);
            //    switch (item.srvtype)
            //    {
            //        case "HTL":

            //            note += System.Environment.NewLine;
            //            note += " - " + item.ngaythang.Value.ToString("dd/MM/yyyy") + "-" + item.ngaythang.Value.AddDays(1).ToString("dd/MM/yyyyy") + " ";
            //            item.tour_item = item.tour_item + " " + supplier.Tengiaodich + " - " + supplier.Diachi + " phone: " + supplier.Dienthoai;
            //            var hotel = _hotelRepository.Find(x => x.sgtcode == item.sgtcode && x.stt == item.stt);//chi lay dich vu chua xoa
            //            foreach (var h in hotel)
            //            {
            //                if (h.sgl > 0)
            //                {
            //                    note += h.sgl + "SGN" + "*" + string.Format("{0:#,##0.0}", h.sglcost) + h.currency;
            //                }
            //                if (h.extsgl > 0)
            //                {
            //                    note += " " + h.extsgl + "EXT-SGN" + "*" + string.Format("{0:#,##0.0}", h.extsglcost) + h.currency;
            //                }
            //                //if (a.sglfoc > 0)
            //                //{
            //                //    gia += "," + gia + " " + a.sglfoc + "SGN FOC";
            //                //}
            //                if (h.dbl > 0)
            //                {
            //                    note += " " + h.dbl + "DBL" + "*" + string.Format("{0:#,##0.0}", h.dblcost) + h.currency;
            //                }
            //                if (h.extdbl > 0)
            //                {
            //                    note += " " + h.extdbl + "EXT-DBL" + "*" + string.Format("{0:#,##0.0}", h.extdblcost) + h.currency;
            //                }
            //                //if (a.dblfoc > 0)
            //                //{
            //                //    gia += "," + gia + " " + a.dblfoc + "DBL FOC";
            //                //}
            //                if (h.twn > 0)
            //                {
            //                    note += " " + h.twn + "TWN" + "*" + string.Format("{0:#,##0.0}", h.twncost) + h.currency;
            //                }
            //                if (h.exttwn > 0)
            //                {
            //                    note += " " + h.exttwn + "EXT-TWN" + "*" + string.Format("{0:#,##0.0}", h.exttwncost) + h.currency;
            //                }
            //                //if (a.twnfoc > 0)
            //                //{
            //                //    gia += "," + gia + " " + a.twnfoc + "TWN FOC";
            //                //}
            //                if (h.tpl > 0)
            //                {
            //                    note += " " + h.tpl + "TPL" + "*" + string.Format("{0:#,##0.0}", h.tplcost) + h.currency;
            //                }
            //                if (h.exttpl > 0)
            //                {
            //                    note += " " + h.exttpl + "EXT-TPL" + "*" + string.Format("{0:#,##0.0}", h.exttplcost) + h.currency;
            //                }
            //                //if (a.tplfoc > 0)
            //                //{
            //                //    gia += "," + gia + " " + a.tplfoc + "TPL FOC";
            //                //}
            //                if (h.oth > 0)
            //                {
            //                    note += " " + h.oth + " OTH-" + h.othpax + "pax" + "*" + string.Format("{0:#,##0.0}", h.othcost) + h.currency + "-" + h.othtype;
            //                }
            //                note += "( phòng loại " + h.note + " ) " + item.srvnode;
            //            }
            //            break;
            //        //case "DIN":
            //        //case "BRK":
            //        //case "LUN":
            //        //    note += System.Environment.NewLine;
            //        //    note += item.srvtype + " " + item.ngaythang.Value.ToString("dd/MM/yyyy") + " " + (item.pax + item.childern) + "P "+item.tour_item+" " + supplier.Tengiaodich + " gồm có " + item.pax + " người lớn *"+item.unitpricea+item.currency+" " + (item.childern>0? item.childern+" trẻ em * "+item.unitpricec+ item.currency:"")+" "+item.srvnode;
            //        //    break;
            //        default:
            //            note += System.Environment.NewLine;
            //            note += " - " + item.srvtype + ": " + item.ngaythang.Value.ToString("dd/MM/yyyy") + " " + (item.pax + item.childern) + "P " + item.tour_item + " " + supplier.Tengiaodich + (item.amount > 0 ? " Tổng chi phí  " + string.Format("{0:#,##0.0}", item.amount) : " gồm có " + item.pax + " người lớn *" + string.Format("{0:#,##0.0}", item.unitpricea) + item.currency + " " + (item.childern > 0 ? item.childern + " trẻ em * " + string.Format("{0:#,##0.0}", item.unitpricec) + item.currency : "")) + " " + item.srvnode;
            //            break;
            //    }
            //}
            //#endregion

            entity.Times = _bookedRepository.getNextBookingTime(entity.Sgtcode, entity.Supplierid);
            entity.Booking = _bookedRepository.nextBooking();
            entity.Date = System.DateTime.Now;
            entity.Name = entity.Name ?? "";
            entity.Profile = entity.Profile ?? "";
            entity.Profile += entity.Logfile;// TempData["booking"].ToString();
            entity.Note = entity.Note ?? "";
            entity.Logfile = entity.Logfile ?? "";
            entity.Logfile += "User booking lần thứ " + entity.Booking + "  " + HttpContext.Session.GetString("usename") + " vào lúc: " + DateTime.Now.ToString();

            _bookedRepository.Create(entity);


            return Redirect(HttpContext.Session.GetString("urlBooking"));
            // 
        }
        [HttpGet]
        public ActionResult getBookingByCode(string sgtcode, string supplierid)
        {
            string note = "", ghichu = "";
            var dichvu = _tourprogRepository.Find(x => x.sgtcode == sgtcode && x.supplierid == supplierid).ToList();
            BookedViewModel b = _bookedRepository.getBookingBySupplier(sgtcode, supplierid);
            if (b == null)
            {
                b = new BookedViewModel();
            }
            if (b.Times > 0)
            {
                note = "Thay đổi booking lần thứ " + b.Times + ", ngày " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                note = "Booking lần thứ nhất, ngày " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            note += bookstring(sgtcode, supplierid);
            if (b == null)
            {
                b = new BookedViewModel();
            }

            b.Logfile = note; // dùng cho hàm post  public ActionResult Booking(Booked entity)
            ghichu = Environment.NewLine + "=================================================" + Environment.NewLine;
            ghichu += "      Đề nghị quý đối tác xác nhận dịch vụ cho chúng tôi trong vòng 12 giờ";
            ghichu += Environment.NewLine + " Thông tin hoá đơn VAT ";
            ghichu += Environment.NewLine + "        * Hoá đơn phải ghi rõ code đoàn,(gửi kèm booking này)";
            ghichu += Environment.NewLine + "        * Nội dung xuất hoá đơn:";
            b.Note = ghichu;
            var result = JsonConvert.SerializeObject(b);
            return Json(result);
        }
        #endregion
        #region Export chương trình tour to word
        public IEnumerable<Tourprog> TourProgBySGTCode(string id)
        {
            var progtemp = _tourprogRepository.ListTourProg(id);
            var t = _tourinfRepository.GetById(id);
            ViewBag.cn = HttpContext.Session.GetString("chinhanh");
            foreach (var item in progtemp)
            {
                item.logfile = item.date > 0 ? t.arr.AddDays(item.date - 1).ToString("ddd, dd/MM/yyyy").ToUpper() : "";
                switch (item.srvtype)
                {
                    case "LUN":
                    case "DIN":
                    case "BRK":
                    case "SHP":
                        if (item.supplierid != null)
                        {
                            var supplier = _supplierRepository.getSupplierById(item.supplierid);
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
                            var supplier = _supplierRepository.getSupplierById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich;
                        }
                        else
                        {
                            item.tour_item = item.tour_item;
                        }
                        break;
                    case "SHW":
                    case "MUS":
                    case "WPU":
                        if (item.supplierid != null)
                        {
                            var supplier = _supplierRepository.GetById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich + item.time ?? " " + item.carrier;
                        }
                        else
                        {
                            item.tour_item = item.tour_item + " " + item.time ?? " " + item.carrier; ;
                        }
                        break;
                    case "AIR":
                    case "TRA":
                        if (item.airtype == "DON")
                        {
                            //item.tour_item = item.tour_item + " " + item.dep + "-" + item.arr + " chuyến: " + item.carrier + " đáp lúc " + item.time + " * " + String.Format("{0:#,##0.0}", item.unitpricea) + " " + item.currency;
                            item.tour_item = item.tour_item + " " + item.dep + "-" + item.arr + " chuyến: " + item.carrier + " đáp lúc " + item.time + " * " + string.Format("{0:#,##0.0}", item.unitpricea) + " " + item.currency;
                        }
                        else if (item.airtype == "TIEN")
                        {
                            //item.tour_item = item.tour_item + " tiển khách chặng: " + item.dep + "-" + item.arr + " chuyến: " + item.carrier + " * " + String.Format("{0:#,##0.0}", item.unitpricea) + " " + item.currency;
                            item.tour_item = item.tour_item + " " + item.dep + "-" + item.arr + " chuyến: " + item.carrier + " *" + string.Format("{0:#,##0.0}", item.unitpricea) + " " + item.currency;
                        }
                        else
                        {
                            //item.tour_item = item.tour_item + " bay cùng khách chặng: " + item.dep + "-" + item.arr + " chuyến: " + item.carrier + " * " + String.Format("{0:#,##0.0}", item.unitpricea) + " " + item.currency;
                            item.tour_item = item.tour_item + " " + item.dep + "-" + item.arr + " chuyến: " + item.carrier + " * " + string.Format("{0:#,##0.0}", item.unitpricea) + " " + item.currency;
                        }

                        break;
                    case "HTL":
                    case "CRU":
                        if (item.supplierid != null)
                        {
                            string gia = "";
                            var supplier = _supplierRepository.getSupplierById(item.supplierid);
                            item.tour_item = item.tour_item + " " + supplier.Tengiaodich + " - " + supplier.Diachi + " phone: " + supplier.Dienthoai;
                            var hotel = _hotelRepository.Find(x => x.sgtcode == item.sgtcode);//chi lay dich vu chua xoa
                            if (hotel != null)
                            {
                                foreach (var a in hotel)
                                {

                                    if (a.sgl > 0)
                                    {
                                        gia += a.sgl + "SGN" + "*" + string.Format("{0:#,##0.0}", a.sglcost) + a.currency;
                                    }
                                    if (a.extsgl > 0)
                                    {
                                        gia += "," + a.extsgl + "EXT-SGN" + "*" + string.Format("{0:#,##0.0}", a.extsglcost) + a.currency;
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
                                        gia += "," + a.homestay + " Home stay" + "*"+a.homestaypax+" pax " + string.Format("{0:#,##0.0}", a.homestaycost)+"/1pax " + a.currency+" - "+a.homestaynote;
                                    }                                                                    
                                    if (a.oth > 0)
                                    {
                                        gia += "," + a.oth + " OTH-" + a.othpax + "pax" + "*" + string.Format("{0:#,##0.0}", a.othcost) + a.currency + "-" + a.othtype;
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
                        var sse = _sightseeingRepository.GetByCodeAndStt(item.sgtcode, item.stt);
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
                                    tendtq += "," + _diemtqRepository.GetById(d.Codedtq).Diemtq;
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
            return progtemp;
        }
        public BookKS bookKS(string Sgtcode, string Supplierid)
        {
            var dichvu = _tourprogRepository.Find(x => x.sgtcode == Sgtcode && x.supplierid == Supplierid).ToList();
            BookKS bookKS = new BookKS();
            bookKS.loaiphong = "";
            bookKS.foc = "";
            foreach (var i in dichvu)
            {
                var supplier = _supplierRepository.getSupplierById(i.supplierid);
                int soluong = 0;
                switch (i.srvtype)
                {
                    case "HTL":
                    case "CRU":
                        bookKS.sgtcode = Sgtcode;
                        bookKS.tenkhachsan = supplier.Tenthuongmai;
                        bookKS.ngaythang += "      * " + i.ngaythang.Value.ToString("dd/MM/yyyy") + Environment.NewLine;
                        var hotel = _hotelRepository.Find(x => x.sgtcode == i.sgtcode && x.stt == i.stt);
                        int freefoc = _tourprogRepository.GetById(i.Id).foc;
                        foreach (var h in hotel)
                        {
                            bookKS.loaiphong += "      * " + i.ngaythang.Value.ToString("dd/MM/yyyy") + " : ";
                            if (freefoc > 0)
                            {
                                bookKS.foc += Environment.NewLine + "            * " + i.ngaythang.Value.ToString("dd/MM/yyyy") + " : " + freefoc.ToString();
                            }
                            if (h.sgl > 0)
                            {
                                soluong += h.sgl;
                                bookKS.loaiphong += h.sgl + "SGN" + "*" + string.Format("{0:#,##0.0}", h.sglcost) + h.currency;
                            }
                        
                            if (h.dbl > 0)
                            {
                                soluong += h.dbl;
                                bookKS.loaiphong += " " + h.dbl + "DBL" + "*" + string.Format("{0:#,##0.0}", h.dblcost) + h.currency;
                            }
                            if (h.extdbl > 0)
                            {
                                bookKS.loaiphong += " " + h.extdbl + "EXT-DBL" + "*" + string.Format("{0:#,##0.0}", h.extdblcost) + h.currency;
                            }
                          
                            if (h.twn > 0)
                            {
                                soluong += h.twn;
                                bookKS.loaiphong += " " + h.twn + "TWN" + "*" + string.Format("{0:#,##0.0}", h.twncost) + h.currency;

                            }
                            if (h.exttwn > 0)
                            {
                                bookKS.loaiphong += " " + h.exttwn + "EXT-TWN" + "*" + string.Format("{0:#,##0.0}", h.exttwncost) + h.currency;

                            }
                          
                            if (h.homestay > 0)
                            {
                                bookKS.loaiphong += " " + h.homestay + " Homestay" + "*" + h.homestaypax + " pax * " + string.Format("{0:#,##0.0}", h.homestaycost)+"/1pax" + h.currency;

                            }
                           
                            if (h.oth > 0)
                            {
                                soluong += h.oth;
                                bookKS.loaiphong += " " + h.oth + " OTH-" + h.othpax + "pax" + "*" + string.Format("{0:#,##0.0}", h.othcost) + h.currency + "-" + h.othtype;
                            }
                            bookKS.loaiphong += "(phòng loại " + h.note + ")" + Environment.NewLine;
                        }

                        bookKS.thanhtoan = i.debit ? " CHUYỂN KHOẢN" : "TIỀN MẶT";
                        bookKS.soluong = soluong.ToString();
                        //}
                        break;
                    default:
                        break;
                }

            }
            return bookKS;
        }
        public BookLUN bookLUN(string Sgtcode, string Supplierid)
        {
            var dichvu = _tourprogRepository.Find(x => x.sgtcode == Sgtcode && x.supplierid == Supplierid).ToList();
            BookLUN bookDV = new BookLUN();

            foreach (var i in dichvu)
            {
                var supplier = _supplierRepository.getSupplierById(i.supplierid);
                switch (i.srvtype)
                {
                    case "LUN":
                    case "DIN":
                    case "BRK":
                        bookDV.sgtcode = Sgtcode;
                        if (i.supplierid != null)
                        {
                            bookDV.tenkhachsan = supplier.Tenthuongmai;
                        }
                        else
                        {
                            bookDV.tenkhachsan = "";
                        }
                        // bookDV.ngaythang += "      * " + i.ngaythang.Value.ToString("dd/MM/yyyy") + Environment.NewLine;
                        if (i.srvtype == "LUN")
                        {
                            bookDV.dichvu = " Buổi trưa:" + " - Ngày " + i.ngaythang.Value.ToString("dd/MM/yyyy") + Environment.NewLine;
                        }
                        else
                        {
                            bookDV.dichvu = " Ăn tối" + " - Ngày " + i.ngaythang.Value.ToString("dd/MM/yyyy") + Environment.NewLine;
                        }
                        string gia = "";
                        gia = "Tổng số khách " + (i.pax+i.childern);
                        if (i.amount > 0)
                        {
                            if (i.childern > 0)
                            {
                                gia += " trong đó có: " + i.childern + " trẻ em, giá tổng cộng: " + string.Format("{0:#,##0}", i.amount) + " VND";
                            }
                            else
                            {
                                gia += " giá tổng cộng: " + string.Format("{0:#,##0}", i.amount) + " VND";
                            }
                        }
                        else
                        {
                            if (i.childern > 0)
                            {
                                gia += " bao gồm " + (i.pax) + " x " + string.Format("{0:#,##0}", i.unitpricea) + "VND trong đó có: " + i.childern + " trẻ em x " + string.Format("{0:#,##0}", i.unitpricec) + " VND";
                            }
                            else
                            {
                                gia += " x " + string.Format("{0:#,##0}", i.unitpricea) + " VND";
                            }
                        }
                        bookDV.soluong = gia + Environment.NewLine;
                        bookDV.thongtinkhac = i.srvnode;
                        bookDV.thanhtoan = i.debit ? " CHUYỂN KHOẢN" : "TIỀN MẶT";
                        bookDV.thucdon = i.srvnode ?? ""+Environment.NewLine;
                        break;
                    default:
                        break;
                }
            }
            return bookDV;
        }
        #region Lấy thông tin hướng dẫn, khách import vô booking form
        public string bookstring(string Sgtcode, string Supplierid)
        {
            string note = "";
            BookKS bookingKS = bookKS(Sgtcode, Supplierid);
            BookLUN bookingDV = bookLUN(Sgtcode, Supplierid);

            if (!string.IsNullOrEmpty(bookingKS.tenkhachsan))
            {
                note += Environment.NewLine + "      BOOKING DỊCH VỤ - KHÁCH SẠN" + Environment.NewLine;
                note += "1. Đối tác vui lòng giử dịch vụ tại: " + bookingKS.tenkhachsan + " theo thông tin như sau:" + Environment.NewLine;
                note += "2. CODE ĐOÀN: " + bookingKS.sgtcode + Environment.NewLine;
                note += "3. Đêm check in: " + Environment.NewLine + bookingKS.ngaythang;
                note += "4. Số lượng:" + bookingKS.soluong + Environment.NewLine;
                note += "5. Loại phòng - Giá tiền: " + Environment.NewLine + bookingKS.loaiphong;
                note += "     Giá phòng bao gồm ăn sáng cho 2 khách(phòng đôi)-10%VAT thuế và 5% phí phục vụ." + Environment.NewLine;
                note += "     Các dịch vụ khác như nước uống phát sinh, điện thoại, giặt ủi, mini bar.... Khách tự thanh toán trực tiếp với khách sạn khi trả phòng" + Environment.NewLine;
                note += "     Dịch vụ phòng (free/ nếu có) cho nội bộ:" + bookingKS.foc + Environment.NewLine;
                note += "6. Thông tin hướng dẫn viên:" + Environment.NewLine + "        * " + thongtinhuongdan(Sgtcode) + Environment.NewLine;
                note += "7. Thông tin Khách (Tên cụ thể hoặc file đính kèm):" + Environment.NewLine + thongtinkhach(Sgtcode);
                note += "8. Thanh toán tiền mặt hay chuyển khoản: " + bookingKS.thanhtoan;
            }
            if (!string.IsNullOrEmpty(bookingDV.tenkhachsan) && !string.IsNullOrEmpty(bookingKS.tenkhachsan))
            {
                note += Environment.NewLine + "=======================================";
            }
            if (!string.IsNullOrEmpty(bookingDV.tenkhachsan))
            {
                note += Environment.NewLine + "      BOOKING DỊCH VỤ - ĂN UỐNG" + Environment.NewLine;
                note += "1. Đối tác vui lòng giử dịch vụ tại: " + bookingDV.tenkhachsan + " theo thông tin như sau:" + Environment.NewLine;
                note += "2. CODE ĐOÀN: " + bookingDV.sgtcode + Environment.NewLine;
                note += "3. Buổi ăn và ngày ăn cụ thể: " + Environment.NewLine + "        " + bookingDV.dichvu;
                note += "4. Số lượng khách và giá tiền" + Environment.NewLine + "        " + bookingDV.soluong;
                note += "5. Số lượng free yêu cầu cho nội bộ (nếu có):  " + Environment.NewLine;
                note += "6. Yêu cầu khác " + Environment.NewLine;
                note += "7. Thông tin hướng dẫn viên:" + Environment.NewLine + "        * " + thongtinhuongdan(Sgtcode) + Environment.NewLine;
                note += "8. Thực đơn đoàn" + Environment.NewLine + bookingDV.thucdon;
                note += "9. Thanh toán tiền mặt hay chuyển khoản: " + bookingDV.thanhtoan;
            }
            return note;
        }
        public string thongtinhuongdan(string Sgtcode)
        {
            var huongdan = _huongdanRepository.ListHuongdan(Sgtcode).ToList();
            string tenhd = "", dienthoai = "", thongtin = "";

            if (huongdan != null)
            {
                foreach (var h in huongdan)
                {
                    tenhd = h.Tenhd ?? "";
                    dienthoai = " - " + h.Dienthoai ?? "";
                    thongtin += " " + tenhd + dienthoai;
                }
            }
            return thongtin;
        }
        public string thongtinkhach(string Sgtcode)
        {
            string listkhach = "";
            var khachtour = _khachtourRepository.ListKhachTour(Sgtcode);
            int dong = 1;
            if (khachtour != null)
            {
                foreach (var k in khachtour)
                {
                    listkhach += "         " + dong.ToString() + ". " + k.hoten + Environment.NewLine;
                    dong++;
                }
            }
            return listkhach + Environment.NewLine;
        }
        #endregion

        [HttpGet]
        public IActionResult ExportBookingToWord(string Sgtcode, string Supplierid, string Note)
        //public IActionResult ExportBookingToWord(string Note)
        {
            DocX doc = null;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string fileName = webRootPath + @"\bookingform.docx";
            doc = DocX.Load(fileName);
            string a = bookstring(Sgtcode, Supplierid);
            // string a =  TempData["booking"].ToString();
            Note = Note ?? "";
            a += Note;
            doc.InsertParagraph(a);
            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "BookingForm" + "_" + DateTime.Now + ".docx");

        }
        public IActionResult ExportWord(string sgtcode)
        {
            var listTourPro = TourProgBySGTCode(sgtcode).OrderBy(x => x.ngaythang);
            var tourInfo = _tourinfRepository.GetById(sgtcode);
            var tourNote = _tournodeRepository.GetById(sgtcode);
            var userInfo = _userinfoRepository.GetUserByUsername(tourInfo.operators);
            var khachTours = _khachtourRepository.ListKhachTour(sgtcode);
            DocX doc = null;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string fileName = webRootPath + @"\WordTemplateForTour.docx";
            doc = DocX.Load(fileName);

            doc.AddCustomProperty(new CustomProperty("sgtcode", "CHƯƠNG TRÌNH TOUR: " + sgtcode));
            doc.AddCustomProperty(new CustomProperty("tuyen", "Tuyến: " + (string.IsNullOrEmpty(tourInfo.reference) ? "" : tourInfo.reference)));// _tuyentqRepository.GetById(tourInfo.routing).Tentuyen));
            doc.AddCustomProperty(new CustomProperty("batDau", "Bắt đầu: " + tourInfo.arr.ToString("dd/MM/yyyy")));
            doc.AddCustomProperty(new CustomProperty("ketThuc", "Kết thúc: " + tourInfo.dep.ToString("dd/MM/yyyy")));
            string dh = "", dt = "";
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.hoten))
            {
                dh = userInfo.hoten;
                dt = userInfo.dienthoai;
            }
            else
            {
                userInfo = new UserInfo();
            }
            doc.AddCustomProperty(new CustomProperty("dieuHanh", "Điều hành: " + dh + " - " + dt));
            doc.AddCustomProperty(new CustomProperty("sk", "Sk: " + tourInfo.pax));
            if (tourNote != null && !string.IsNullOrEmpty(tourNote.Headernode))
            {
                doc.InsertParagraph(tourNote.Headernode);
            }

            var tourProgramTbl = doc.AddTable(1, 3);

            tourProgramTbl.Rows[0].Cells[0].Paragraphs[0].Append("Giờ").Bold().Font("Times New Roman").FontSize(11).Alignment = Alignment.center;
            tourProgramTbl.Rows[0].Cells[1].Paragraphs[0].Append("SK").Bold().Font("Times New Roman").FontSize(11).Alignment = Alignment.center;
            tourProgramTbl.Rows[0].Cells[2].Paragraphs[0].Append("Chương trình").Bold().Font("Times New Roman").FontSize(11).Alignment = Alignment.center;

            int dong = 0;
            int tongkhach = 0;
            string sokhach = "";
            foreach (var i in listTourPro)
            {
                var row = tourProgramTbl.InsertRow();
                tongkhach = i.pax + i.childern;
                if (tongkhach > 0)
                    sokhach = tongkhach.ToString();
                else sokhach = "";
                if (i.date > 0)
                {
                    string ngaythang = i.ngaythang == null ? "" : i.ngaythang.Value.ToString("ddd, dd/MM/yyyy").ToUpper();
                    string tu = "        ";
                    if (i.srvtype == "ITI")
                    {                   
                        if (!string.IsNullOrEmpty(i.tour_item))
                        {
                            tu +=  _thanhphoRepository.ListTinh().Where(x => x.Matinh == i.tour_item).SingleOrDefault().Tentinh;
                        }
                        if (!string.IsNullOrEmpty(i.srvnode))
                        {
                            tu += " - " + _thanhphoRepository.ListTinh().Where(x => x.Matinh == i.srvnode).SingleOrDefault().Tentinh;
                        }
                    }
                    string thu = ngaythang.Substring(0, 3);
                    switch (thu)
                    {
                        case "MON": thu = "Thứ hai " + i.ngaythang.Value.ToString("dd/MM/yyyy")+ tu; break;
                        case "TUE": thu = "Thứ ba " + i.ngaythang.Value.ToString("dd/MM/yyyy") + tu; break;
                        case "WED": thu = "Thứ tư " + i.ngaythang.Value.ToString("dd/MM/yyyy") + tu; break;
                        case "THU": thu = "Thứ năm " + i.ngaythang.Value.ToString("dd/MM/yyyy") + tu; break;
                        case "FRI": thu = "Thứ sáu " + i.ngaythang.Value.ToString("dd/MM/yyyy") + tu; break;
                        case "SAT": thu = "Thứ bảy " + i.ngaythang.Value.ToString("dd/MM/yyyy") + tu; break;
                        case "SUN": thu = "Chủ nhật " + i.ngaythang.Value.ToString("dd/MM/yyyy") + tu; break;
                    }

               //     row.Cells[0].Paragraphs[0].Append("\n" + i.time).Alignment = Alignment.center;
                    //row.Cells[1].Paragraphs[0].Append("\n"+i.pax.ToString()=="0"?"":i.pax.ToString()).Alignment = Alignment.center;

                  //  row.Cells[1].Paragraphs[0].Append("\n" + sokhach).Alignment = Alignment.center;
                    row.Cells[2].Paragraphs[0].Append("               " + thu).Bold();
                    //row.Cells[2].Paragraphs[0].Append("\n- " + i.tour_item);
                }
                else
                {
                    row.Cells[0].Paragraphs[0].Append(i.time).Alignment = Alignment.center;
                    row.Cells[1].Paragraphs[0].Append(sokhach).Alignment = Alignment.center;
                    //row.Cells[1].Paragraphs[0].Append(i.pax.ToString() =="0"?"":i.pax.ToString()).Alignment = Alignment.center;
                    row.Cells[2].Paragraphs[0].Append("- " + i.tour_item);
                }
                dong++;
                if (dong > 1)
                {
                    row.Cells[0].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_dotDotDash, BorderSize.one, 0, Color.White));
                    row.Cells[1].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_dotDotDash, BorderSize.one, 0, Color.White));
                    row.Cells[2].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_dotDotDash, BorderSize.one, 0, Color.White));
                }
            }
            tourProgramTbl.AutoFit = AutoFit.Window;

            tourProgramTbl.SetWidthsPercentage(new[] { 5f, 5f, 90f }, 500);

            doc.InsertTable(tourProgramTbl);

            if (tourNote != null && !string.IsNullOrEmpty(tourNote.Footernode))
            {
                doc.InsertParagraph();
                doc.InsertParagraph(tourNote.Footernode);
            }
            doc.InsertParagraph();
            string roomingList = "Danh sách khách (Rooming List): ";

            //Formatting Title  
            Novacode.Formatting titleFormat = new Novacode.Formatting();

            titleFormat.Bold = true;
            doc.InsertParagraph(roomingList, false, titleFormat);

            var roomingListTbl = doc.AddTable(1, 4);
            roomingListTbl.Rows[0].Cells[0].Paragraphs[0].Append("Tên khách").Bold().Font("Times New Roman").FontSize(11).Alignment = Alignment.center;
            roomingListTbl.Rows[0].Cells[1].Paragraphs[0].Append("Phái").Bold().Font("Times New Roman").FontSize(11).Alignment = Alignment.center;
            roomingListTbl.Rows[0].Cells[2].Paragraphs[0].Append("Hộ chiếu").Bold().Font("Times New Roman").FontSize(11).Alignment = Alignment.center;
            roomingListTbl.Rows[0].Cells[3].Paragraphs[0].Append("Loại phòng").Bold().Font("Times New Roman").FontSize(11).Alignment = Alignment.center;
            int dong_ = 0;
            foreach (var k in khachTours)
            {
                var row = roomingListTbl.InsertRow();
                row.Cells[0].Paragraphs[0].Append(k.hoten);
                row.Cells[1].Paragraphs[0].Append(!k.phai ? "Nữ" : "Nam");
                row.Cells[2].Paragraphs[0].Append(k.hochieu);
                row.Cells[3].Paragraphs[0].Append(k.hochieu);
                dong_++;
                if (dong_ > 1)
                {
                    row.Cells[0].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_inset, BorderSize.one, 0, Color.Silver));
                    row.Cells[1].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_inset, BorderSize.one, 0, Color.Silver));
                    row.Cells[2].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_inset, BorderSize.one, 0, Color.Silver));
                    row.Cells[3].SetBorder(TableCellBorderType.Top, new Border(BorderStyle.Tcbs_inset, BorderSize.one, 0, Color.Silver));
                }
            }

            roomingListTbl.AutoFit = AutoFit.Window;
            roomingListTbl.SetWidthsPercentage(new[] { 35f, 5f, 20f, 20f }, 500);

            doc.InsertTable(roomingListTbl);

            MemoryStream stream = new MemoryStream();

            // Saves the Word document to MemoryStream
            doc.SaveAs(stream);
            stream.Position = 0;
            // Download Word document in the browser
            return File(stream, "application/msword", "Chuongtrinhtour_" + sgtcode + "_" + DateTime.Now + ".docx");
        }
        #endregion
    }

}
