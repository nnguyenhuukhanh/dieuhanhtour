using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace dieuhanhtour.Controllers
{
    public class BaocaoController : BaseController
    {
        private ITourinfRepository _tourinfRepository;
        private IBaocaoRepository _baocaoRepository;
        private IPhongbanRepository _phongbanRepository;
        private IChinhanhRepository _chinhanhRepository;
        private IDichvuRepository _dichvuRepository;

        public BaocaoController(ITourinfRepository tourinfRepository, IBaocaoRepository baocaoRepository,
                                IPhongbanRepository phongbanRepository, IChinhanhRepository chinhanhRepository, IDichvuRepository dichvuRepository)
        {
            _tourinfRepository = tourinfRepository;
            _baocaoRepository = baocaoRepository;
            _phongbanRepository = phongbanRepository;
            _chinhanhRepository = chinhanhRepository;
            _dichvuRepository = dichvuRepository;
        }
        #region Thong ke doan bat dau
        [HttpGet]
        public ActionResult Doanbatdau(string tungay, string denngay, string thitruong, string chinhanh)
        {
            thitruong = thitruong ?? "";
            chinhanh = string.IsNullOrEmpty(chinhanh) ? HttpContext.Session.GetString("chinhanh") : chinhanh;
            listDsPhongban(thitruong);
            listChinhanh(chinhanh);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanbatdau");
            }

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            ViewBag.chinhanh = chinhanh;
            var d = _baocaoRepository.listDoanbatdau(tungay, denngay, thitruong, chinhanh);
            ViewBag.doanhthu= d.Sum(x => x.doanhthu);
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doanbatdau");
            }
            else
            {
                return View("Doanbatdau", d);
            }

        }

        public void TrSetCellBorder(ExcelWorksheet xlSheet, int iRowIndex, int colIndex, ExcelBorderStyle excelBorderStyle, ExcelHorizontalAlignment excelHorizontalAlignment, Color borderColor, string fontName, int fontSize, FontStyle fontStyle)
        {
            xlSheet.Cells[iRowIndex, colIndex].Style.HorizontalAlignment = excelHorizontalAlignment;
            // Set Border
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Left.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Top.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Right.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Bottom.Style = excelBorderStyle;
            // Set màu ch Border
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Left.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Top.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Right.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Bottom.Color.SetColor(borderColor);

            // Set Font cho text  trong Range hiện tại                    
            xlSheet.Cells[iRowIndex, colIndex].Style.Font.SetFromFont(new Font(fontName, fontSize, fontStyle));
        }

        [HttpPost]
        public ActionResult DoanbatdautoExcel(string tungay, string denngay, string thitruong,string chinhanh)
        {
            thitruong = thitruong ?? "";
            chinhanh=string.IsNullOrEmpty(chinhanh)?HttpContext.Session.GetString("chinhanh"):chinhanh;
            var d = _baocaoRepository.listDoanbatdau(tungay, denngay, thitruong, chinhanh);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 8].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CÁC ĐOÀN BẮT ĐẦU THỰC HIỆN TOUR ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 8].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "ngày " + tungay;
            }
            else
            {
                fromTo = "từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = "Ngày bắt đầu tour " + fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 9].Merge = true;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Khối / Phòng";
            xlSheet.Cells[5, 3].Value = "SGT code ";
            xlSheet.Cells[5, 4].Value = "SK ";
            xlSheet.Cells[5, 5].Value = "Bắt đầu";
            xlSheet.Cells[5, 6].Value = "Kết thúc";
            xlSheet.Cells[5, 7].Value = "Lộ trình";
            xlSheet.Cells[5, 8].Value = "Doanh thu";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Borde
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoanbatdauViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenphong;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.batdau.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.ketthuc.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 7].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.doanhthu;
                xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }

            //dong tong
            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "=SUM(D6:D" + (5 + d.Count()) + ")";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Formula = "=SUM(H6:H" + (5 + d.Count()) + ")";
            xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;


            //end dong tong

            xlSheet.Cells.AutoFitColumns();


            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_cac_doan_batdau_thuchientour_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }


        #endregion

        #region Thong ke doan ket thuc
        [HttpGet]
        public ActionResult Doanketthuc(string tungay, string denngay, string thitruong)
        {
            thitruong = thitruong ?? "";
            listDsPhongban(thitruong);

            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanketthuc");
            }
            thitruong = thitruong ?? "";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var d = _baocaoRepository.listDoanketthuc(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doanketthuc");
            }
            else
            {
                return View("Doanketthuc", d);
            }

        }

        [HttpPost]
        public ActionResult DoanketthuctoExcel(string tungay, string denngay, string thitruong)
        {
            thitruong = thitruong ?? "";

            var d = _baocaoRepository.listDoanketthuc(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"));

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 9].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CÁC ĐOÀN KẾT THÚC TOUR ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 9].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "ngày " + tungay;
            }
            else
            {
                fromTo = "từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = "Ngày bắt đầu tour " + fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 9].Merge = true;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Khối / Phòng";
            xlSheet.Cells[5, 3].Value = "SGT code ";
            xlSheet.Cells[5, 4].Value = "SK ";
            xlSheet.Cells[5, 5].Value = "Bắt đầu";
            xlSheet.Cells[5, 6].Value = "Kết thúc";
            xlSheet.Cells[5, 7].Value = "Lộ trình";
            xlSheet.Cells[5, 8].Value = "Doanh thu";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoanbatdauViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenphong;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.batdau.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.ketthuc.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
             
                xlSheet.Cells[iRowIndex, 7].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.doanhthu;
                xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }

            //dong tong
            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "=SUM(D6:D" + (5 + d.Count()) + ")";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Formula = "=SUM(H6:H" + (5 + d.Count()) + ")";
            xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;


            //end dong tong

            xlSheet.Cells.AutoFitColumns();
            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_cac_doan_ketthuc_tour_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }
        #endregion

        #region Thong ke doan đã lock
        [HttpGet]
        public ActionResult Doanlocktour(string tungay, string denngay, string thitruong)
        {
            thitruong = thitruong ?? "";
            listDsPhongban(thitruong);

            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanlocktour");
            }

            //thitruong = thitruong ?? "";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listDoanlocktour(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doanlocktour");
            }
            else
            {
                return View("Doanlocktour", d);
            }

        }

        [HttpPost]
        public ActionResult DoanlocktourtoExcel(string tungay, string denngay, string thitruong)
        {
            thitruong = thitruong ?? "";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listDoanlocktour(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"));

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 9].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CÁC ĐOÀN LOCK TOUR ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 9].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "ngày " + tungay;
            }
            else
            {
                fromTo = "từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = "Ngày bắt đầu tour " + fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Khối / Phòng";
            xlSheet.Cells[5, 3].Value = "SGT code ";
            xlSheet.Cells[5, 4].Value = "SK ";
            xlSheet.Cells[5, 5].Value = "Bắt đầu";
            xlSheet.Cells[5, 6].Value = "Kết thúc";
            xlSheet.Cells[5, 7].Value = "Lộ trình";
            xlSheet.Cells[5, 8].Value = "Doanh thu";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoanbatdauViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenphong;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.batdau.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.ketthuc.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
              
                xlSheet.Cells[iRowIndex,7].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.doanhthu;
                xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }

            //dong tong
            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "=SUM(D6:D" + (5 + d.Count()) + ")";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Formula = "=SUM(H6:H" + (5 + d.Count()) + ")";
            xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;


            //end dong tong

            xlSheet.Cells.AutoFitColumns();
            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_cac_doan_lock_tour_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }

        #endregion

        #region Chuyen bay den
        [HttpGet]
        public ActionResult Chuyenbayden(string tungay, string denngay, string thitruong, string sanbay, string chinhanh)
        {
            thitruong = thitruong ?? "";
            sanbay = sanbay ?? "";
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Chuyenbayden");
            }

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            ViewBag.sanbay = sanbay;
            var d = _baocaoRepository.listChuyenbayden(tungay, denngay, thitruong, sanbay, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Chuyenbayden");
            }
            else
            {
                return View("Chuyenbayden", d);
            }
        }
        [HttpPost]
        public ActionResult ChuyenbaydentoExcel(string tungay, string denngay, string thitruong, string sanbay)
        {
            thitruong = thitruong ?? "";
            sanbay = sanbay ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listChuyenbayden(tungay, denngay, thitruong, sanbay, HttpContext.Session.GetString("chinhanh"));

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CÁC CHUYẾN BAY ĐẾN " + sanbay.ToUpper();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 5].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;
            xlSheet.Column(2).Width = 15;
            xlSheet.Column(3).Width = 20;
            xlSheet.Column(4).Width = 10;
            xlSheet.Column(5).Width = 50;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Ngày";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "SK ";
            xlSheet.Cells[5, 5].Value = "Thông tin chuyến bay";


            xlSheet.Cells[5, 1, 5, 5].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 5].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (ChuyenbayViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.ngayden.ToString("dd/MM/yyy");
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_cac_chuyen_bay_den_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }
        #endregion
        #region Chuyen bay di
        [HttpGet]
        public ActionResult Chuyenbaydi(string tungay, string denngay, string thitruong, string sanbay)
        {
            thitruong = thitruong ?? "";
            sanbay = sanbay ?? "";
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Chuyenbaydi");
            }

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            ViewBag.sanbay = sanbay;
            var d = _baocaoRepository.listChuyenbaydi(tungay, denngay, thitruong, sanbay, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Chuyenbaydi");
            }
            else
            {
                return View("Chuyenbaydi", d);
            }
        }
        [HttpPost]
        public ActionResult ChuyenbayditoExcel(string tungay, string denngay, string thitruong, string sanbay)
        {
            thitruong = thitruong ?? "";
            sanbay = sanbay ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listChuyenbaydi(tungay, denngay, thitruong, sanbay, HttpContext.Session.GetString("chinhanh"));

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CÁC CHUYẾN BAY ĐI  " + sanbay.ToUpper();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 5].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;
            xlSheet.Column(2).Width = 15;
            xlSheet.Column(3).Width = 20;
            xlSheet.Column(4).Width = 10;
            xlSheet.Column(5).Width = 50;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Ngày";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "SK ";
            xlSheet.Cells[5, 5].Value = "Thông tin chuyến bay";


            xlSheet.Cells[5, 1, 5, 5].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 5].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (ChuyenbayViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.ngayden.ToString("dd/MM/yyy");
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_cac_chuyen_bay_di_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }
        #endregion
        #region Chuyen bay den va di
        [HttpGet]
        public ActionResult Chuyenbaydenvadi(string tungay, string denngay, string thitruong, string sanbay, string chinhanh)
        {
            thitruong = thitruong ?? "";
            sanbay = sanbay ?? "";
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Chuyenbaydenvadi");
            }

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            ViewBag.sanbay = sanbay;
            var d = _baocaoRepository.listChuyenbaydenvadi(tungay, denngay, thitruong, sanbay, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Chuyenbaydenvadi");
            }
            else
            {
                return View("Chuyenbaydenvadi", d);
            }
        }
        [HttpPost]
        public ActionResult ChuyenbaydenvadiToExcel(string tungay, string denngay, string thitruong, string sanbay)
        {
            thitruong = thitruong ?? "";
            sanbay = sanbay ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listChuyenbaydenvadi(tungay, denngay, thitruong, sanbay, HttpContext.Session.GetString("chinhanh"));

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CÁC CHUYẾN BAY ĐẾN VÀ ĐI  " + sanbay.ToUpper();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 5].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;
            xlSheet.Column(2).Width = 15;
            xlSheet.Column(3).Width = 20;
            xlSheet.Column(4).Width = 10;
            xlSheet.Column(5).Width = 50;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Ngày";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "SK ";
            xlSheet.Cells[5, 5].Value = "Thông tin chuyến bay";


            xlSheet.Cells[5, 1, 5, 5].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 5].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (ChuyenbayViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.ngayden.ToString("dd/MM/yyy");
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_cac_chuyen_bay_den_va_di" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }

        #endregion

        #region Chang bay cua doan
        [HttpGet]
        public ActionResult Changbaydoan(string tungay, string denngay, string thitruong)
        {
            thitruong = thitruong ?? "";
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Changbaydoan");
            }

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var d = _baocaoRepository.listChuyenbaydoan(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"));
            foreach (var item in d)
            {
                item.lotrinh = item.lotrinh.Replace(";", "<br/>");
                item.ghichu = item.ghichu.Replace(";", "<br/>");
            }
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Changbaydoan");
            }
            else
            {
                return View("Changbaydoan", d);
            }
        }
        [HttpPost]
        public ActionResult ChangbaydoanToExcel(string tungay, string denngay, string thitruong)
        {
            thitruong = thitruong ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listChuyenbaydoan(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"));

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 7].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CÁC CHUYẾN BAY CỦA ĐOÀN ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 7].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 7].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;
            xlSheet.Column(2).Width = 20;
            xlSheet.Column(3).Width = 12;
            xlSheet.Column(4).Width = 12;
            xlSheet.Column(5).Width = 10;
            xlSheet.Column(6).Width = 50;
            xlSheet.Column(7).Width = 50;
            //xlSheet.Column(6).BestFit = true;
            //xlSheet.Column(7).BestFit = true;


            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Đoàn";
            xlSheet.Cells[5, 3].Value = "Ngày đến ";
            xlSheet.Cells[5, 4].Value = "Ngày đi ";
            xlSheet.Cells[5, 5].Value = "SK";
            xlSheet.Cells[5, 6].Value = "Chặng bay";
            xlSheet.Cells[5, 7].Value = "Ghi chú";

            xlSheet.Cells[5, 1, 5, 7].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 7].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (ChuyenbayViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.sgtcode;// vm.ngayden.ToString("dd/MM/yyy");
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.ngayden.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.ngaydi.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.lotrinh.Replace(";", System.Environment.NewLine);
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ghichu.Replace(";", System.Environment.NewLine);
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 7].Style.WrapText = true;


                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_cac_chang_bay_doan_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }
        #endregion

        #region Thống kê loại phòng
        [HttpGet]
        public ActionResult Roomnite(string tungay, string denngay)
        {
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Roomnite");
            }

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.listRoomNite(tungay, denngay, HttpContext.Session.GetString("chinhanh"));

            ViewBag.rowtotal = d.Where(x => x.thanhpho != null && x.tenkhachsan == null).OrderBy(x => x.thanhpho).AsEnumerable().ToList();

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Roomnite");
            }
            else
            {
                return View("Roomnite", d);
            }
        }
        [HttpPost]
        public ActionResult RoomniteToExcel(string tungay, string denngay)
        {
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.listRoomNite(tungay, denngay, HttpContext.Session.GetString("chinhanh"));



            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 22].Merge = true;

            xlSheet.Cells[2, 1].Value = "THỐNG KÊ ROOMNITE ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 22].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "ngày " + tungay;
            }
            else
            {
                fromTo = "từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = "Ngày bắt đầu tour " + fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 22].Merge = true;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Tên khách sạn";
            xlSheet.Cells[5, 3].Value = "SGL ";
            xlSheet.Cells[5, 4].Value = "SK ";
            xlSheet.Cells[5, 5].Value = "Phí VNĐ";
            xlSheet.Cells[5, 6].Value = "Phí USD";
            xlSheet.Cells[5, 7].Value = "DBL ";
            xlSheet.Cells[5, 8].Value = "SK ";
            xlSheet.Cells[5, 9].Value = "Phí VNĐ";
            xlSheet.Cells[5, 10].Value = "Phí USD";
            xlSheet.Cells[5, 11].Value = "TWN ";
            xlSheet.Cells[5, 12].Value = "SK ";
            xlSheet.Cells[5, 13].Value = "Phí VNĐ";
            xlSheet.Cells[5, 14].Value = "Phí USD";
            xlSheet.Cells[5, 15].Value = "TPL ";
            xlSheet.Cells[5, 16].Value = "SK ";
            xlSheet.Cells[5, 17].Value = "Phí VNĐ";
            xlSheet.Cells[5, 18].Value = "Phí USD";
            xlSheet.Cells[5, 19].Value = "RTN ";
            xlSheet.Cells[5, 20].Value = "SK ";
            xlSheet.Cells[5, 21].Value = "Phí VNĐ";
            xlSheet.Cells[5, 22].Value = "Phí USD";

            xlSheet.Cells[5, 1, 5, 22].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 22].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 22].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 22].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 22].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 22].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 22].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 22].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 22].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 22].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 22].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;

            List<RoomniteViewModel> lstTotal = d.Where(x => x.thanhpho != null && x.tenkhachsan == null).OrderBy(x => x.thanhpho).AsEnumerable().ToList();
            //  var lstTotal = Model;
            int iSttks = 0;
            int iTongphong = 0, iTongsk = 0;
            decimal dTongvnd = 0, dTongusd = 0;

            foreach (RoomniteViewModel item in lstTotal)
            {

                //row by thanhpho
                List<RoomniteViewModel> lstTp = d.Where(x => x.thanhpho == item.thanhpho && x.tenkhachsan != null).AsEnumerable().ToList();

                decimal[] adTotalSk = new decimal[4];

                foreach (RoomniteViewModel itemtp in lstTp)
                {
                    iSttks = iSttks + 1;
                    iTongphong = iTongphong + itemtp.sgl + itemtp.dbl + itemtp.twn + itemtp.tpl;
                    iTongsk = iTongsk + itemtp.sksgl + itemtp.skdbl + itemtp.sktwn + itemtp.sktpl;
                    dTongvnd = dTongvnd + itemtp.phivndsgl + itemtp.phivnddbl + itemtp.phivndtwn + itemtp.phivndtpl;
                    dTongusd = dTongusd + itemtp.phiusdsgl + itemtp.phiusddbl + itemtp.phiusdtwn + itemtp.phiusdtpl;

                    adTotalSk[0] = adTotalSk[0] + iTongphong;
                    adTotalSk[1] = adTotalSk[1] + iTongsk;
                    adTotalSk[2] = adTotalSk[2] + dTongvnd;
                    adTotalSk[3] = adTotalSk[3] + dTongusd;

                    xlSheet.Cells[iRowIndex, 1].Value = iSttks;
                    TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 2].Value = itemtp.tenkhachsan;
                    TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.sgl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 3].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 3].Value = itemtp.sgl;
                    }

                    TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.sksgl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 4].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 4].Value = itemtp.sksgl;
                    }
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.phivndsgl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 5].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 5].Value = itemtp.phivndsgl;
                    }
                    xlSheet.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.phiusdsgl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 6].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 6].Value = itemtp.phiusdsgl;
                    }
                    xlSheet.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.dbl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 7].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 7].Value = itemtp.dbl;
                    }
                    TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.skdbl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 8].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 8].Value = itemtp.skdbl;
                    }
                    TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.phivnddbl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 9].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 9].Value = itemtp.phivnddbl;
                    }

                    xlSheet.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.phiusddbl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 10].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 10].Value = itemtp.phiusddbl;
                    }

                    xlSheet.Cells[iRowIndex, 10].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.twn == 0)
                    {
                        xlSheet.Cells[iRowIndex, 11].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 11].Value = itemtp.twn;
                    }
                    TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.sktwn == 0)
                    {
                        xlSheet.Cells[iRowIndex, 12].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 12].Value = itemtp.sktwn;
                    }
                    TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.phivndtwn == 0)
                    {
                        xlSheet.Cells[iRowIndex, 13].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 13].Value = itemtp.phivndtwn;
                    }
                    xlSheet.Cells[iRowIndex, 13].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.phiusdtwn == 0)
                    {
                        xlSheet.Cells[iRowIndex, 14].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 14].Value = itemtp.phiusdtwn;
                    }
                    xlSheet.Cells[iRowIndex, 14].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 14, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.tpl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 15].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 15].Value = itemtp.tpl;
                    }
                    TrSetCellBorder(xlSheet, iRowIndex, 15, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 15].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.sktpl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 16].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 16].Value = itemtp.sktpl;
                    }

                    TrSetCellBorder(xlSheet, iRowIndex, 16, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 16].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.phivndtpl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 17].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 17].Value = itemtp.phivndtpl;
                    }
                    xlSheet.Cells[iRowIndex, 17].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 17, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 17].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (itemtp.phiusdtpl == 0)
                    {
                        xlSheet.Cells[iRowIndex, 18].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 18].Value = itemtp.phiusdtpl;
                    }

                    xlSheet.Cells[iRowIndex, 18].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 18, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 18].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    //RTN
                    if (iTongphong == 0)
                    {
                        xlSheet.Cells[iRowIndex, 19].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 19].Value = iTongphong;
                    }

                    TrSetCellBorder(xlSheet, iRowIndex, 19, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 19].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (iTongsk == 0)
                    {
                        xlSheet.Cells[iRowIndex, 20].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 20].Value = iTongsk;
                    }

                    TrSetCellBorder(xlSheet, iRowIndex, 20, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 20].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (dTongvnd == 0)
                    {
                        xlSheet.Cells[iRowIndex, 21].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 21].Value = dTongvnd;
                    }

                    xlSheet.Cells[iRowIndex, 21].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 21, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 21].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (dTongusd == 0)
                    {
                        xlSheet.Cells[iRowIndex, 22].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 22].Value = dTongusd;
                    }

                    xlSheet.Cells[iRowIndex, 22].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 22, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 22].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    //reset dong tiep theo
                    iTongphong = 0;
                    iTongsk = 0;
                    dTongusd = 0;
                    dTongvnd = 0;

                    iRowIndex += 1;
                }

                //dong tong
                xlSheet.Cells[iRowIndex, 1].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = item.thanhpho;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.sgl == 0)
                {
                    xlSheet.Cells[iRowIndex, 3].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 3].Value = item.sgl;
                }

                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.sksgl == 0)
                {
                    xlSheet.Cells[iRowIndex, 4].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 4].Value = item.sksgl;
                }
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.phivndsgl == 0)
                {
                    xlSheet.Cells[iRowIndex, 5].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 5].Value = item.phivndsgl;
                }
                xlSheet.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.phiusdsgl == 0)
                {
                    xlSheet.Cells[iRowIndex, 6].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 6].Value = item.phiusdsgl;
                }
                xlSheet.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.dbl == 0)
                {
                    xlSheet.Cells[iRowIndex, 7].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 7].Value = item.dbl;
                }
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.skdbl == 0)
                {
                    xlSheet.Cells[iRowIndex, 8].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 8].Value = item.skdbl;
                }
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.phivnddbl == 0)
                {
                    xlSheet.Cells[iRowIndex, 9].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 9].Value = item.phivnddbl;
                }

                xlSheet.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.phiusddbl == 0)
                {
                    xlSheet.Cells[iRowIndex, 10].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 10].Value = item.phiusddbl;
                }

                xlSheet.Cells[iRowIndex, 10].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.twn == 0)
                {
                    xlSheet.Cells[iRowIndex, 11].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 11].Value = item.twn;
                }
                TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.sktwn == 0)
                {
                    xlSheet.Cells[iRowIndex, 12].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 12].Value = item.sktwn;
                }
                TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.phivndtwn == 0)
                {
                    xlSheet.Cells[iRowIndex, 13].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 13].Value = item.phivndtwn;
                }
                xlSheet.Cells[iRowIndex, 13].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.phiusdtwn == 0)
                {
                    xlSheet.Cells[iRowIndex, 14].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 14].Value = item.phiusdtwn;
                }
                xlSheet.Cells[iRowIndex, 14].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 14, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.tpl == 0)
                {
                    xlSheet.Cells[iRowIndex, 15].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 15].Value = item.tpl;
                }
                TrSetCellBorder(xlSheet, iRowIndex, 15, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 15].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.sktpl == 0)
                {
                    xlSheet.Cells[iRowIndex, 16].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 16].Value = item.sktpl;
                }

                TrSetCellBorder(xlSheet, iRowIndex, 16, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 16].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.phivndtpl == 0)
                {
                    xlSheet.Cells[iRowIndex, 17].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 17].Value = item.phivndtpl;
                }
                xlSheet.Cells[iRowIndex, 17].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 17, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 17].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (item.phiusdtpl == 0)
                {
                    xlSheet.Cells[iRowIndex, 18].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 18].Value = item.phiusdtpl;
                }

                xlSheet.Cells[iRowIndex, 18].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 18, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 18].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //RTN
                if (adTotalSk[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 19].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 19].Value = adTotalSk[0];
                }

                TrSetCellBorder(xlSheet, iRowIndex, 19, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 19].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalSk[1] == 0)
                {
                    xlSheet.Cells[iRowIndex, 20].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 20].Value = adTotalSk[1];
                }

                TrSetCellBorder(xlSheet, iRowIndex, 20, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 20].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalSk[2] == 0)
                {
                    xlSheet.Cells[iRowIndex, 21].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 21].Value = adTotalSk[2];
                }

                xlSheet.Cells[iRowIndex, 21].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 21, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 21].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalSk[3] == 0)
                {
                    xlSheet.Cells[iRowIndex, 22].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 22].Value = adTotalSk[3];
                }

                xlSheet.Cells[iRowIndex, 22].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 22, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 22].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                iRowIndex += 1;

                //end dong tong
            }
            if (iRowIndex > 0)
            {
                xlSheet.Cells[iRowIndex - 1, 1, iRowIndex - 1, 22].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            }

            xlSheet.Cells.AutoFitColumns();
            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "thongke_roomnite_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }
        #endregion

        #region Theo doi dieu hanh dich vụ
        #region Doan theo khach san
        [HttpGet]
        public ActionResult Doantheokhachsan(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            sortby = sortby ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            if (sortby == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doantheokhachsan");
            }


            var d = _baocaoRepository.ListDoantheokhachsan(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doantheokhachsan");
            }
            else
            {
                foreach (var item in d)
                {
                    item.phong = String.IsNullOrEmpty(item.phong) ? "" : item.phong.Replace(";", "<br/>") ?? "";
                }
                return View("Doantheokhachsan", d);
            }
        }
        [HttpPost]
        public ActionResult DoantheokhachsanToExcel(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? HttpContext.Session.GetString("phong");
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var d = _baocaoRepository.ListDoantheokhachsan(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "HOTEL BOOKING ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 8].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;
            xlSheet.Column(2).Width = 50;
            xlSheet.Column(3).Width = 20;
            xlSheet.Column(4).Width = 10;
            xlSheet.Column(5).Width = 5;
            xlSheet.Column(6).Width = 50;
            xlSheet.Column(7).Width = 20;
            xlSheet.Column(8).Width = 20;
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Tên khách sạn";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "Ngày ";
            xlSheet.Cells[5, 5].Value = "SK";
            xlSheet.Cells[5, 6].Value = "Phòng";
            xlSheet.Cells[5, 7].Value = "Ghi chú";
            xlSheet.Cells[5, 8].Value = "Thị trường";


            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoankhachsanViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenks;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.ngay.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = string.IsNullOrEmpty(vm.phong) ? "" : vm.phong.Replace(";", System.Environment.NewLine);
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ghichu;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.thitruong;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;



                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;

            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_doan_theo_ks" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }
        #endregion

        #region Doan theo nha hang
        [HttpGet]
        public ActionResult Doantheonhahang(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            sortby = sortby ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            listDsPhongban(thitruong);
            if (sortby == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doantheonhahang");
            }
            var d = _baocaoRepository.ListDoantheonhahang(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doantheonhahang");
            }
            else
            {
                return View("Doantheonhahang", d);
            }
        }
        [HttpPost]
        public ActionResult DoantheonhahangToExcel(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var d = _baocaoRepository.ListDoantheonhahang(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO ĐẶT ĂN NHÀ HÀNG ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 10].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 10].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 30;//ten nha hang
            xlSheet.Column(3).Width = 15;//doan
            xlSheet.Column(4).Width = 10;//ngay
            xlSheet.Column(5).Width = 10;//bua
            xlSheet.Column(6).Width = 10;//sk
            xlSheet.Column(7).Width = 10;// khach an
            xlSheet.Column(8).Width = 25;//tieu chuan
            xlSheet.Column(9).Width = 30;// ghi chu
            xlSheet.Column(10).Width = 20;//
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Tên nhà hàng";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "Ngày ";
            xlSheet.Cells[5, 5].Value = "Bữa";
            xlSheet.Cells[5, 6].Value = "SK đoàn";
            xlSheet.Cells[5, 7].Value = "Khách ăn";
            xlSheet.Cells[5, 8].Value = "Tiêu chuẩn";
            xlSheet.Cells[5, 9].Value = "Ghi chú";
            xlSheet.Cells[5, 10].Value = "Thị trường";

            xlSheet.Cells[5, 1, 5, 10].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 10].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoannhahangViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tennh;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.ngay.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.loai;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.khachan;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.tieuchuan;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 9].Value = vm.ghichu;
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 10].Value = vm.thitruong;
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 9].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 9].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 10].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 10].Style.Border.Top.Style = ExcelBorderStyle.None;

            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_doan_theo_nhahang" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }

        #endregion

        #region Doan theo cano/tau thuyen
        [HttpGet]
        public ActionResult Doantheocano(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            sortby = sortby ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            if (sortby == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doantheocano");
            }
            var d = _baocaoRepository.ListDoantheocano(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doantheocano");
            }
            else
            {
                return View("Doantheocano", d);
            }
        }
        [HttpPost]
        public ActionResult DoantheocanoToExcel(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var d = _baocaoRepository.ListDoantheocano(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "ĐẶT CANO, TÀU THUYỀN ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 8].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 30;//ten nha hang
            xlSheet.Column(3).Width = 15;//doan
            xlSheet.Column(4).Width = 10;//ngay
            xlSheet.Column(5).Width = 10;//SK
            xlSheet.Column(6).Width = 30;//noi dung
            xlSheet.Column(7).Width = 30;// ghi chu
            xlSheet.Column(8).Width = 20;// thi trong
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Tên hãng";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "Ngày ";
            xlSheet.Cells[5, 5].Value = "SK";
            xlSheet.Cells[5, 6].Value = "Nội dung";
            xlSheet.Cells[5, 7].Value = "Ghi chú";
            xlSheet.Cells[5, 8].Value = "Thị trường";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoancanoViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenhang;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.ngay.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.noidung;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ghichu;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.thitruong;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;



            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_doan_theo_cano" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );
        }


        #endregion

        #region Doan theo roi nuoc
        [HttpGet]
        public ActionResult Doanroinuoc(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            sortby = sortby ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            if (sortby == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanroinuoc");
            }
            var d = _baocaoRepository.ListDoantheoroinuoc(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doanroinuoc");
            }
            else
            {
                return View("Doanroinuoc", d);
            }
        }
        [HttpPost]
        public ActionResult DoanroinuocToExcel(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var d = _baocaoRepository.ListDoantheoroinuoc(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO ĐẶT VÉ MÚA RỐI NƯỚC ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 8].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 30;//ten nha hang
            xlSheet.Column(3).Width = 15;//doan
            xlSheet.Column(4).Width = 10;//ngay
            xlSheet.Column(5).Width = 10;//SK
            xlSheet.Column(6).Width = 30;//noi dung
            xlSheet.Column(7).Width = 30;// ghi chu
            xlSheet.Column(8).Width = 20;// thi trong
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Nhóm múa rối nước";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "Ngày ";
            xlSheet.Cells[5, 5].Value = "SK";
            xlSheet.Cells[5, 6].Value = "Nội dung";
            xlSheet.Cells[5, 7].Value = "Ghi chú";
            xlSheet.Cells[5, 8].Value = "Thị trường";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoancanoViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenhang;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.ngay.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.noidung;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ghichu;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.thitruong;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;



            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_doan_theo_roi_nuoc" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );
        }


        #endregion

        #region Doan theo van nghe
        [HttpGet]
        public ActionResult Doanvannghe(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            sortby = sortby ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            if (sortby == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanvannghe");
            }
            var d = _baocaoRepository.ListDoantheovannghe(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doanvannghe");
            }
            else
            {
                return View("Doanvannghe", d);
            }
        }
        [HttpPost]
        public ActionResult DoanvanngheToExcel(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var d = _baocaoRepository.ListDoantheovannghe(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO ĐẶT VÉ XEM CA NHẠC, VĂN NGHỆ ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 8].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 30;//ten nha hang
            xlSheet.Column(3).Width = 15;//doan
            xlSheet.Column(4).Width = 10;//ngay
            xlSheet.Column(5).Width = 10;//SK
            xlSheet.Column(6).Width = 30;//noi dung
            xlSheet.Column(7).Width = 30;// ghi chu
            xlSheet.Column(8).Width = 20;// thi trong
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Nhóm biễu diễn";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "Ngày ";
            xlSheet.Cells[5, 5].Value = "SK";
            xlSheet.Cells[5, 6].Value = "Nội dung";
            xlSheet.Cells[5, 7].Value = "Ghi chú";
            xlSheet.Cells[5, 8].Value = "Thị trường";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoancanoViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenhang;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.ngay.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.noidung;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ghichu;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.thitruong;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;



            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_doan_theo_van_nghe" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );
        }


        #endregion

        #region Doan theo xe lửa
        [HttpGet]
        public ActionResult Doanxelua(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            sortby = sortby ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            if (sortby == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanxelua");
            }
            var d = _baocaoRepository.ListDoantheoxelua(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doanxelua");
            }
            else
            {
                return View("Doanxelua", d);
            }
        }
        [HttpPost]
        public ActionResult DoanxeluaToExcel(string tungay, string denngay, string thitruong, string sortby)
        {
            thitruong = thitruong ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var d = _baocaoRepository.ListDoantheoxelua(tungay, denngay, thitruong, HttpContext.Session.GetString("chinhanh"), sortby);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO ĐẶT VÉ XE LỬA ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 8].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 30;//ten nha hang
            xlSheet.Column(3).Width = 15;//doan
            xlSheet.Column(4).Width = 10;//ngay
            xlSheet.Column(5).Width = 10;//SK
            xlSheet.Column(6).Width = 30;//noi dung
            xlSheet.Column(7).Width = 30;// ghi chu
            xlSheet.Column(8).Width = 20;// thi trong
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Hãng xe lửa";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "Ngày ";
            xlSheet.Cells[5, 5].Value = "SK";
            xlSheet.Cells[5, 6].Value = "Lộ trình";
            xlSheet.Cells[5, 7].Value = "Ghi chú";
            xlSheet.Cells[5, 8].Value = "Thị trường";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoancanoViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenhang;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.ngay.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.noidung;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ghichu;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.thitruong;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;



            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_doan_theo_xe_lua" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );
        }


        #endregion

        #region Doan theo diem tham quan

        [HttpGet]
        public ActionResult Doanthamquan(string tungay, string denngay, int[] aweekday, string sortby)
        {
            string thu = "";
            sortby = sortby ?? "A";

            foreach (int ithu in aweekday)
            {
                thu = thu + ithu + ",";

                switch (ithu)
                {
                    case 1:
                        @ViewBag.check1 = "checked";
                        break;
                    case 2:
                        @ViewBag.check2 = "checked";
                        break;
                    case 3:
                        @ViewBag.check3 = "checked";
                        break;
                    case 4:
                        @ViewBag.check4 = "checked";
                        break;
                    case 5:
                        @ViewBag.check5 = "checked";
                        break;
                    case 6:
                        @ViewBag.check6 = "checked";
                        break;
                    case 7:
                        @ViewBag.check7 = "checked";
                        break;
                }
            }

            if (thu.Length > 0)
            {
                //bo dau , sau cung
                if (thu.LastIndexOf(',') > 0)
                {
                    thu = thu.Substring(0, thu.Length - 1);
                }
            }

            thu = thu ?? "";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thu = thu;
            ViewBag.aweekday = aweekday;
            if (sortby == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }

            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanthamquan");
            }
            var d = _baocaoRepository.ListDoantheodiemthamquan(tungay, denngay, HttpContext.Session.GetString("chinhanh"), thu, sortby);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doanthamquan");
            }
            else
            {
                return View("Doanthamquan", d);
            }
        }
        [HttpPost]
        public ActionResult DoanthamquanToExcel(string tungay, string denngay, int[] aweekday, string sortby)
        {
            string thu = "", sNgayThamQuan = "";
            sortby = sortby ?? "A";

            foreach (int ithu in aweekday)
            {
                thu = thu + ithu + ",";

                switch (ithu)
                {
                    case 2:
                        @ViewBag.check2 = "checked";
                        sNgayThamQuan = sNgayThamQuan + "Thứ hai,";
                        break;
                    case 3:
                        @ViewBag.check3 = "checked";
                        sNgayThamQuan = sNgayThamQuan + "Thứ ba,";
                        break;
                    case 4:
                        @ViewBag.check4 = "checked";
                        sNgayThamQuan = sNgayThamQuan + "Thứ tư,";
                        break;
                    case 5:
                        @ViewBag.check5 = "checked";
                        sNgayThamQuan = sNgayThamQuan + "Thứ năm,";
                        break;
                    case 6:
                        @ViewBag.check6 = "checked";
                        sNgayThamQuan = sNgayThamQuan + "Thứ sáu,";
                        break;
                    case 7:
                        @ViewBag.check7 = "checked";
                        sNgayThamQuan = sNgayThamQuan + "Thứ bảy,";
                        break;
                    case 1:
                        sNgayThamQuan = sNgayThamQuan + "Chủ Nhật,";
                        @ViewBag.check1 = "checked";
                        break;
                }

            }

            if (aweekday.Length == 0) sNgayThamQuan = "Thứ hai, thứ ba, thứ tư, thứ năm, thứ sáu, thứ bảy, chủ nhật ";

            if (thu.Length > 0)
            {
                //bo dau , sau cung
                if (thu.LastIndexOf(',') > 0)
                {
                    thu = thu.Substring(0, thu.Length - 1);
                    sNgayThamQuan = sNgayThamQuan.Substring(0, sNgayThamQuan.Length - 1);
                }
            }

            thu = thu ?? "";

            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thu = thu;

            var d = _baocaoRepository.ListDoantheodiemthamquan(tungay, denngay, HttpContext.Session.GetString("chinhanh"), thu, sortby);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CÁC ĐIỂM THAM QUAN ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 5].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;

            xlSheet.Cells[4, 1].Value = "Có ngày tham quan là : " + sNgayThamQuan;
            xlSheet.Cells[4, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[4, 1, 4, 5].Merge = true;
            // Định dạng chiều dài cho cột
            //xlSheet.Column(1).Width = 8;//stt
            //xlSheet.Column(2).Width = 30; //doan
            //xlSheet.Column(3).Width = 15; 
            //xlSheet.Column(4).Width = 10; 
            //xlSheet.Column(5).Width = 10; 

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Code đoàn";
            xlSheet.Cells[5, 3].Value = "Khách nước ngoài ";
            xlSheet.Cells[5, 4].Value = "Khách Việt Nam  ";
            xlSheet.Cells[5, 5].Value = "Tổng số khách ";


            xlSheet.Cells[5, 1, 5, 5].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 5].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 5].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;

            //lay ra ten diem
            List<string> lstTenThanhpho = d.Select(x => x.diemtp).Distinct().ToList();
            int[] aiTongkhach = new int[3];

            foreach (string ten in lstTenThanhpho)
            {
                int iSttks = 0;
                List<DoanthamquanViewModel> lstDiemtq = d.Where(x => x.diemtp == ten).AsEnumerable().ToList();

                int iTongsk = 0;

                xlSheet.Cells[iRowIndex, 1].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = ten;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                // dong diemtq ===================================================================
                int[] aiTongkhachtheocode = new int[3];

                foreach (DoanthamquanViewModel item in lstDiemtq)
                {
                    iRowIndex += 1;
                    iSttks = iSttks + 1;
                    iTongsk = item.khachnuocngoai + item.khachviet;

                    aiTongkhachtheocode[0] = aiTongkhachtheocode[0] + item.khachnuocngoai;
                    aiTongkhachtheocode[1] = aiTongkhachtheocode[1] + item.khachviet;
                    aiTongkhachtheocode[2] = aiTongkhachtheocode[2] + item.khachnuocngoai + +item.khachviet;

                    aiTongkhach[0] = aiTongkhach[0] + item.khachnuocngoai;
                    aiTongkhach[1] = aiTongkhach[1] + item.khachviet;
                    aiTongkhach[2] = aiTongkhach[2] + item.khachnuocngoai + +item.khachviet;

                    xlSheet.Cells[iRowIndex, 1].Value = iSttks;
                    TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 2].Value = item.sgtcode;
                    TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 3].Value = item.khachnuocngoai;
                    TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 4].Value = item.khachviet;
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 5].Value = iTongsk;
                    TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    //reset
                    iTongsk = 0;
                } //diemtq      

                //dong tong                  
                iRowIndex += 1;

                xlSheet.Cells[iRowIndex, 1].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = "CỘNG";
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = aiTongkhachtheocode[0];
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = aiTongkhachtheocode[1];
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = aiTongkhachtheocode[2];
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //END dong tong
                iRowIndex += 1;


            }

            //dong tong cong
            //iRowIndex += 1;

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            xlSheet.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            xlSheet.Cells[iRowIndex, 3].Value = aiTongkhach[0];
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            xlSheet.Cells[iRowIndex, 4].Value = aiTongkhach[1];
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            xlSheet.Cells[iRowIndex, 5].Value = aiTongkhach[2];
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            //end du lieu            
            xlSheet.Cells[iRowIndex, 1, iRowIndex, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells.AutoFitColumns();

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_doan_cac_diemtq_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );
        }



        #endregion

        #region Doan theo dichvu khac
        [HttpGet]
        public ActionResult Doandichvukhac(string tungay, string denngay, string thitruong, string dichvu, string sortby)
        {
            thitruong = thitruong ?? "";
            dichvu = dichvu ?? "AIR";
            sortby = sortby ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            if (sortby == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }
            listDichvu_baocao(dichvu);
            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doandichvukhac");
            }
            var d = _baocaoRepository.ListDoantheodichvukhac(tungay, denngay, thitruong, dichvu, HttpContext.Session.GetString("chinhanh"), sortby);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doandichvukhac");
            }
            else
            {
                return View("Doandichvukhac", d);
            }
        }
        [HttpPost]
        public ActionResult DoandichvukhacToExcel(string tungay, string denngay, string thitruong, string dichvu, string sortby)
        {
            thitruong = thitruong ?? "";
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;
            var dv = _dichvuRepository.GetById(dichvu);
            var d = _baocaoRepository.ListDoantheodichvukhac(tungay, denngay, thitruong, dichvu, HttpContext.Session.GetString("chinhanh"), sortby);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "THỐNG KÊ DỊCH VỤ " + dv.Tendv.ToUpper();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 8].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 30;//ten nha hang
            xlSheet.Column(3).Width = 15;//doan
            xlSheet.Column(4).Width = 10;//ngay
            xlSheet.Column(5).Width = 10;//SK
            xlSheet.Column(6).Width = 30;//noi dung
            xlSheet.Column(7).Width = 30;// ghi chu
            xlSheet.Column(8).Width = 20;// thi trong
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Đơn vị";
            xlSheet.Cells[5, 3].Value = "Đoàn ";
            xlSheet.Cells[5, 4].Value = "Ngày ";
            xlSheet.Cells[5, 5].Value = "SK";
            xlSheet.Cells[5, 6].Value = "Nội dung";
            xlSheet.Cells[5, 7].Value = "Ghi chú";
            xlSheet.Cells[5, 8].Value = "Thị trường";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 8].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 8].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoancanoViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.tenhang;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.ngay.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.noidung;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ghichu;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.thitruong;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;



            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_doan_theo_dv_" + dv.Iddichvu + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );
        }


        #endregion

        #endregion

        #region Đoàn chưa phân hướng dẫn
        [HttpGet]
        public ActionResult HuongdanDoan(string tungay, string denngay, string giatri)
        {
            giatri = giatri ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            if (giatri == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }

            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("HuongdanDoan");
            }
            var d = _baocaoRepository.ListHuongdanDoan(tungay, denngay, HttpContext.Session.GetString("chinhanh"), giatri);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("HuongdanDoan");
            }
            else
            {
                return View("HuongdanDoan", d);
            }
        }

        [HttpPost]
        public ActionResult HuongdanDoanToExcel(string tungay, string denngay, string giatri)
        {

            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.ListHuongdanDoan(tungay, denngay, HttpContext.Session.GetString("chinhanh"), giatri);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;
            string title = "";
            if (giatri == "A")
            {
                title = "DANH SÁCH HƯỚNG DẪN CHƯA PHÂN ĐOÀN ";
            }
            else
            {
                title = "DANH SÁCH HƯỚNG DẪN ĐÃ PHÂN ĐOÀN";
            }
            xlSheet.Cells[2, 1].Value = title;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 10].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 10].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 15;//sgtcode
            xlSheet.Column(3).Width = 30;//lotrinh
            xlSheet.Column(4).Width = 8;//sk
            xlSheet.Column(5).Width = 30;//tenhd
            xlSheet.Column(6).Width = 15;//dien thoai
            xlSheet.Column(7).Width = 15;// ngoai ngu
            xlSheet.Column(8).Width = 10;// bat dau
            xlSheet.Column(9).Width = 10;// ketthuc
            xlSheet.Column(10).Width = 40;// cong viec
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Đoàn";
            xlSheet.Cells[5, 3].Value = "Reference ";
            xlSheet.Cells[5, 4].Value = "SK";
            xlSheet.Cells[5, 5].Value = "Tên hướng dẫn";
            xlSheet.Cells[5, 6].Value = "Điện thoại";
            xlSheet.Cells[5, 7].Value = "Ngoại ngữ";
            xlSheet.Cells[5, 8].Value = "Bắt đầu";
            xlSheet.Cells[5, 9].Value = "Kết thúc";
            xlSheet.Cells[5, 10].Value = "Công việc";

            xlSheet.Cells[5, 1, 5, 10].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 10].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (HuongdanDoanViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.tenhd;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.dienthoai;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ngoaingu;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.batdau.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 9].Value = vm.ketthuc.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 10].Value = vm.ndcongviec;
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 9].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 9].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 10].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 10].Style.Border.Top.Style = ExcelBorderStyle.None;



            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_huongdan_doan" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );
        }

        [HttpGet]
        public ActionResult DanhsachHuongdanDidoan(string tungay, string denngay)
        {
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("DanhsachHuongdanDidoan");
            }
            var d = _baocaoRepository.ListHuongdanDoanDiDoan(tungay, denngay, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                SetAlert("Không có thông tin", "error");
                return View("DanhsachHuongdanDidoan");
            }
            else
            {
                return View("DanhsachHuongdanDidoan", d);
            }
        }
        [HttpPost]
        public ActionResult DanhsachHuongdanDidoanToExcel(string tungay, string denngay)
        {
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.ListHuongdanDoanDiDoan(tungay, denngay, HttpContext.Session.GetString("chinhanh"));
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;
            string title = "DANH SÁCH HƯỚNG DẪN ĐI ĐOÀN";

            xlSheet.Cells[2, 1].Value = title;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 10].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 10].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 15;//sgtcode
            xlSheet.Column(3).Width = 30;//lotrinh
            xlSheet.Column(4).Width = 8;//sk
            xlSheet.Column(5).Width = 30;//tenhd
            xlSheet.Column(6).Width = 15;//dien thoai
            xlSheet.Column(7).Width = 15;// ngoai ngu
            xlSheet.Column(8).Width = 10;// bat dau
            xlSheet.Column(9).Width = 10;// ketthuc
            xlSheet.Column(10).Width = 40;// cong viec
            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Đoàn";
            xlSheet.Cells[5, 3].Value = "Reference ";
            xlSheet.Cells[5, 4].Value = "SK";
            xlSheet.Cells[5, 5].Value = "Tên hướng dẫn";
            xlSheet.Cells[5, 6].Value = "Điện thoại";
            xlSheet.Cells[5, 7].Value = "Ngoại ngữ";
            xlSheet.Cells[5, 8].Value = "Bắt đầu";
            xlSheet.Cells[5, 9].Value = "Kết thúc";
            xlSheet.Cells[5, 10].Value = "Công việc";

            xlSheet.Cells[5, 1, 5, 10].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 10].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (HuongdanDoanViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.tenhd.ToUpper();
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.dienthoai;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.ngoaingu;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.batdau.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 9].Value = vm.ketthuc.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 10].Value = vm.ndcongviec;
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 9].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 9].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 10].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 10].Style.Border.Top.Style = ExcelBorderStyle.None;

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "ds_huongdan_di_doan" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
           );
        }
        #endregion

        #region Đoàn chưa phân xe
        [HttpGet]
        public ActionResult XeDoan(string tungay, string denngay, string giatri)
        {
            giatri = giatri ?? "A";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            if (giatri == "A")
            {
                ViewBag.checka = "checked";
                ViewBag.checkb = "";
            }
            else
            {
                ViewBag.checka = "";
                ViewBag.checkb = "checked";
            }

            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("XeDoan");
            }
            var d = _baocaoRepository.ListXeDoan(tungay, denngay, HttpContext.Session.GetString("chinhanh"), giatri);

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("XeDoan");
            }
            else
            {
                return View("XeDoan", d);
            }
        }

        [HttpPost]
        public ActionResult XeDoanToExcel(string tungay, string denngay, string giatri)
        {

            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.ListXeDoan(tungay, denngay, HttpContext.Session.GetString("chinhanh"), giatri);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;
            string title = "";
            if (giatri == "A")
            {
                title = "DANH SÁCH CÁC ĐOÀN CHƯA PHÂN XE ";
            }
            else
            {
                title = "DANH SÁCH CÁC ĐOÀN ĐÃ PHÂN XE";
            }
            xlSheet.Cells[2, 1].Value = title;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 9].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 10].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 15;//sgtcode
            xlSheet.Column(3).Width = 30;//lotrinh
            xlSheet.Column(4).Width = 8;//sk
            xlSheet.Column(5).Width = 10;//Loại xe
            xlSheet.Column(6).Width = 20;//lái xe
            xlSheet.Column(7).Width = 10;// số xe
            xlSheet.Column(8).Width = 10;// điện thoại
            xlSheet.Column(9).Width = 10;// ketthuc
            xlSheet.Column(10).Width = 10;// ketthuc

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Đoàn";
            xlSheet.Cells[5, 3].Value = "Lộ trình";
            xlSheet.Cells[5, 4].Value = "SK";
            xlSheet.Cells[5, 5].Value = "Loại xe";
            xlSheet.Cells[5, 6].Value = "Lái xe";
            xlSheet.Cells[5, 7].Value = "Số xe";
            xlSheet.Cells[5, 8].Value = "Điện thoại";
            xlSheet.Cells[5, 9].Value = "Ngày đón";
            xlSheet.Cells[5, 10].Value = "Kết thúc";

            xlSheet.Cells[5, 1, 5, 10].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 10].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 10].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (XeDoanViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.lotrinh;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sokhach;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.loaixe;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.laixe;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.soxe;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.dienthoai;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 9].Value = vm.ngaydon.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 10].Value = vm.denngay.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 9].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 9].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 10].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 10].Style.Border.Top.Style = ExcelBorderStyle.None;



            //end dong tong



            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_xe_doan" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );
        }

        #endregion

        #region Báo cáo confirm dịch vụ
        [HttpGet]
        public ActionResult Confirmdichvu(string tungay, string denngay, string thitruong)
        {
            thitruong = thitruong ?? "";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;

            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Confirmdichvu");
            }
            return View("Confirmdichvu");
        }
        [HttpPost]
        public ActionResult ConfirmdichvuToExcel(string tungay, string denngay, string thitruong)
        {
            return View();
        }

        #endregion

        #region "Chi phi nha hang"

        [HttpGet]
        public ActionResult listCPNhaHang(string tungay, string denngay)
        {
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("listCPNhaHang");
            }

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.listCPNhaHang(tungay, denngay, HttpContext.Session.GetString("chinhanh"));

            ViewBag.rowtotalCPNhaHang = d.Where(x => x.thanhpho != null && x.tenkhachsan == null && x.srvtype != null).OrderBy(x => x.thanhpho).AsEnumerable().ToList();

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("listCPNhaHang");
            }
            else
            {
                return View("listCPNhaHang", d);
            }
        }
        [HttpPost]
        public ActionResult listCPNhaHangToExcel(string tungay, string denngay)
        {
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.listCPNhaHang(tungay, denngay, HttpContext.Session.GetString("chinhanh"));

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 14].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CHI PHÍ NHÀ HÀNG ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 14].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "ngày " + tungay;
            }
            else
            {
                fromTo = "từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = "Ngày bắt đầu tour " + fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 14].Merge = true;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 1, 6, 1].Merge = true;
            xlSheet.Cells[5, 2].Value = "Tên nhà hàng";
            xlSheet.Cells[5, 2, 6, 2].Merge = true;
            xlSheet.Cells[5, 3].Value = "Ăn sáng ";
            xlSheet.Cells[5, 3, 5, 5].Merge = true;
            xlSheet.Cells[5, 6].Value = "Ăn trưa ";
            xlSheet.Cells[5, 6, 5, 8].Merge = true;
            xlSheet.Cells[5, 9].Value = "Ăn tối";
            xlSheet.Cells[5, 9, 5, 11].Merge = true;
            xlSheet.Cells[5, 12].Value = "Tổng cộng";
            xlSheet.Cells[5, 12, 5, 14].Merge = true;

            xlSheet.Cells[6, 3].Value = "Khách";
            xlSheet.Cells[6, 4].Value = "CP VNĐ";
            xlSheet.Cells[6, 5].Value = "CP USD ";
            xlSheet.Cells[6, 6].Value = "Khách";
            xlSheet.Cells[6, 7].Value = "CP VNĐ";
            xlSheet.Cells[6, 8].Value = "CP USD ";
            xlSheet.Cells[6, 9].Value = "Khách";
            xlSheet.Cells[6, 10].Value = "CP VNĐ";
            xlSheet.Cells[6, 11].Value = "CP USD ";
            xlSheet.Cells[6, 12].Value = "Khách";
            xlSheet.Cells[6, 13].Value = "CP VNĐ";
            xlSheet.Cells[6, 14].Value = "CP USD ";

            xlSheet.Cells[5, 1, 6, 14].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Bold));
            xlSheet.Cells[5, 1, 6, 14].Style.WrapText = false;
            xlSheet.Cells[5, 1, 6, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[5, 1, 6, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Justify;
            // Set Border
            xlSheet.Cells[5, 1, 6, 14].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 6, 14].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 6, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 6, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 6, 14].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 6, 14].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 6, 14].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 6, 14].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 7;

            List<string> lstTenTP = d.Select(x => x.thanhpho).Distinct().ToList();


            foreach (string ten in lstTenTP)
            {
                int iSttks = 0;
                List<CPNhaHangViewModel> lstTp = d.Where(x => x.thanhpho == ten).AsEnumerable().ToList();

                int iTongsk = 0;
                decimal dTongvnd = 0, dTongusd = 0;
                decimal[] adTotalCp = new decimal[2];//an sang
                int[] aiTotalPax = new int[1];

                decimal[] adTotalCpLUN = new decimal[2];//an trua
                int[] aiTotalPaxLUN = new int[1];

                decimal[] adTotalCpDIN = new decimal[2];//an toi
                int[] aiTotalPaxDIN = new int[1];

                decimal[] adTotalColCp = new decimal[2];//cho 3 cot tong
                int[] aiTotalColPax = new int[1];

                //tinh truoc
                foreach (var itemtp in lstTp)
                {
                    iTongsk = iTongsk + itemtp.pax;
                    dTongvnd = dTongvnd + itemtp.cpvnd;
                    dTongusd = dTongusd + itemtp.cpusd;

                    adTotalColCp[0] = adTotalColCp[0] + dTongvnd;
                    adTotalColCp[1] = adTotalColCp[1] + dTongusd;
                    aiTotalColPax[0] = aiTotalColPax[0] + iTongsk;

                    //an sang
                    if (itemtp.srvtype == "BRK")
                    {
                        if (itemtp.pax == 0)
                        {
                        }
                        else
                        {
                            aiTotalPax[0] = aiTotalPax[0] + iTongsk;
                        }
                        if (itemtp.cpvnd == 0)
                        {

                        }
                        else
                        {
                            adTotalCp[0] = adTotalCp[0] + dTongvnd;
                        }

                        if (itemtp.cpusd == 0)
                        {
                        }
                        else
                        {
                            adTotalCp[1] = adTotalCp[1] + dTongusd;
                        }

                    }
                    //an trua
                    else if (itemtp.srvtype == "LUN")
                    {
                        if (itemtp.pax == 0)
                        {

                        }
                        else
                        {
                            aiTotalPaxLUN[0] = aiTotalPaxLUN[0] + iTongsk;
                        }

                        if (itemtp.cpvnd == 0)
                        {

                        }
                        else
                        {
                            adTotalCpLUN[0] = adTotalCpLUN[0] + dTongvnd;

                        }

                        if (itemtp.cpusd == 0)
                        {
                        }
                        else
                        {
                            adTotalCpLUN[1] = adTotalCpLUN[1] + dTongusd;

                        }

                    }
                    //an toi
                    else if (itemtp.srvtype == "DIN")
                    {

                        if (itemtp.pax == 0)
                        {

                        }
                        else
                        {
                            aiTotalPaxDIN[0] = aiTotalPaxDIN[0] + iTongsk;
                        }

                        if (itemtp.cpvnd == 0)
                        {

                        }
                        else
                        {
                            adTotalCpDIN[0] = adTotalCpDIN[0] + dTongvnd;
                        }

                        if (itemtp.cpusd == 0)
                        {

                        }
                        else
                        {
                            adTotalCpDIN[1] = adTotalCpDIN[1] + dTongusd;
                        }

                    }//end DINNER                  



                    //reset dong tiep theo
                    iTongsk = 0;
                    dTongusd = 0;
                    dTongvnd = 0;



                }//end lstTp

                //dong tong ===========================================================
                xlSheet.Cells[iRowIndex, 1].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = ten;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //an sang              
                if (aiTotalPax[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 3].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 3].Value = aiTotalPax[0];
                }
                xlSheet.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalCp[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 4].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 4].Value = adTotalCp[0];
                }
                xlSheet.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalCp[1] == 0)
                {
                    xlSheet.Cells[iRowIndex, 5].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 5].Value = adTotalCp[1];
                }
                xlSheet.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //an trua



                if (aiTotalPaxLUN[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 6].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 6].Value = aiTotalPaxLUN[0];
                }
                xlSheet.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalCpLUN[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 7].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 7].Value = adTotalCpLUN[0];
                }
                xlSheet.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalCpLUN[1] == 0)
                {
                    xlSheet.Cells[iRowIndex, 8].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 8].Value = adTotalCpLUN[1];
                }
                xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //      xlSheet.Cells[iRowIndex, 9, iRowIndex, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                //an toi              

                if (aiTotalPaxDIN[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 9].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 9].Value = aiTotalPaxDIN[0];
                }
                xlSheet.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalCpDIN[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 10].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 10].Value = adTotalCpDIN[0];
                }
                xlSheet.Cells[iRowIndex, 10].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalCpDIN[1] == 0)
                {
                    xlSheet.Cells[iRowIndex, 11].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 11].Value = adTotalCpDIN[1];
                }
                xlSheet.Cells[iRowIndex, 11].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                //3 cot tong
                if (aiTotalColPax[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 12].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 12].Value = aiTotalColPax[0];
                }
                xlSheet.Cells[iRowIndex, 12].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalColCp[0] == 0)
                {
                    xlSheet.Cells[iRowIndex, 13].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 13].Value = adTotalColCp[0];
                }
                xlSheet.Cells[iRowIndex, 13].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                if (adTotalColCp[1] == 0)
                {
                    xlSheet.Cells[iRowIndex, 14].Value = "";
                }
                else
                {
                    xlSheet.Cells[iRowIndex, 14].Value = adTotalColCp[1];
                }
                xlSheet.Cells[iRowIndex, 14].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 14, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex = iRowIndex + 1;
                //END dong tong ====================================================

                //gui
                foreach (var itemtp in lstTp)
                {
                    iSttks = iSttks + 1;
                    iTongsk = iTongsk + itemtp.pax;
                    dTongvnd = dTongvnd + itemtp.cpvnd;
                    dTongusd = dTongusd + itemtp.cpusd;

                    adTotalColCp[0] = adTotalColCp[0] + dTongvnd;
                    adTotalColCp[1] = adTotalColCp[1] + dTongusd;
                    aiTotalColPax[0] = aiTotalColPax[0] + iTongsk;

                    xlSheet.Cells[iRowIndex, 1].Value = iSttks;
                    TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 2].Value = itemtp.tenkhachsan;
                    TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    //an sang
                    if (itemtp.srvtype == "BRK")
                    {
                        if (itemtp.pax == 0)
                        {
                            xlSheet.Cells[iRowIndex, 3].Value = "";
                        }
                        else
                        {
                            aiTotalPax[0] = aiTotalPax[0] + iTongsk;
                            xlSheet.Cells[iRowIndex, 3].Value = itemtp.pax;
                        }
                        xlSheet.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        if (itemtp.cpvnd == 0)
                        {
                            xlSheet.Cells[iRowIndex, 4].Value = "";
                        }
                        else
                        {
                            adTotalCp[0] = adTotalCp[0] + dTongvnd;
                            xlSheet.Cells[iRowIndex, 4].Value = itemtp.cpvnd;
                        }
                        xlSheet.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        if (itemtp.cpusd == 0)
                        {
                            xlSheet.Cells[iRowIndex, 5].Value = "";
                        }
                        else
                        {
                            adTotalCp[1] = adTotalCp[1] + dTongusd;
                            xlSheet.Cells[iRowIndex, 5].Value = itemtp.cpusd;
                        }
                        xlSheet.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 7].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 8].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 9].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 10].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 11].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 3, iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[iRowIndex, 6, iRowIndex, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    //an trua
                    else if (itemtp.srvtype == "LUN")
                    {
                        xlSheet.Cells[iRowIndex, 3].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 4].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 5].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);

                        if (itemtp.pax == 0)
                        {
                            xlSheet.Cells[iRowIndex, 6].Value = "";
                        }
                        else
                        {
                            aiTotalPaxLUN[0] = aiTotalPaxLUN[0] + iTongsk;
                            xlSheet.Cells[iRowIndex, 6].Value = itemtp.pax;
                        }
                        xlSheet.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        if (itemtp.cpvnd == 0)
                        {
                            xlSheet.Cells[iRowIndex, 7].Value = "";
                        }
                        else
                        {
                            adTotalCpLUN[0] = adTotalCpLUN[0] + dTongvnd;
                            xlSheet.Cells[iRowIndex, 7].Value = itemtp.cpvnd;
                        }
                        xlSheet.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        if (itemtp.cpusd == 0)
                        {
                            xlSheet.Cells[iRowIndex, 8].Value = "";
                        }
                        else
                        {
                            adTotalCpLUN[1] = adTotalCpLUN[1] + dTongusd;
                            xlSheet.Cells[iRowIndex, 8].Value = itemtp.cpusd;
                        }
                        xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[iRowIndex, 9].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 10].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 11].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 3, iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    //an toi
                    else if (itemtp.srvtype == "DIN")
                    {
                        xlSheet.Cells[iRowIndex, 3].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 4].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 5].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 6].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 7].Value = "";
                        xlSheet.Cells[iRowIndex, 7].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 8].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);

                        xlSheet.Cells[iRowIndex, 3, iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        if (itemtp.pax == 0)
                        {
                            xlSheet.Cells[iRowIndex, 9].Value = "";
                        }
                        else
                        {
                            aiTotalPaxDIN[0] = aiTotalPaxDIN[0] + iTongsk;
                            xlSheet.Cells[iRowIndex, 9].Value = itemtp.pax;
                        }
                        xlSheet.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        if (itemtp.cpvnd == 0)
                        {
                            xlSheet.Cells[iRowIndex, 10].Value = "";
                        }
                        else
                        {
                            adTotalCpDIN[0] = adTotalCpDIN[0] + dTongvnd;
                            xlSheet.Cells[iRowIndex, 10].Value = itemtp.cpvnd;
                        }
                        xlSheet.Cells[iRowIndex, 10].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        if (itemtp.cpusd == 0)
                        {
                            xlSheet.Cells[iRowIndex, 11].Value = "";
                        }
                        else
                        {
                            adTotalCpDIN[1] = adTotalCpDIN[1] + dTongusd;
                            xlSheet.Cells[iRowIndex, 11].Value = itemtp.cpusd;
                        }
                        xlSheet.Cells[iRowIndex, 11].Style.Numberformat.Format = "#,##0";
                        TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[iRowIndex, 12].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 13].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 14].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 14, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 12, iRowIndex, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    }//end DINNER
                    else //truong hop k co du lieu
                    {
                        xlSheet.Cells[iRowIndex, 3].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 4].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 5].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 6].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 7].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 8].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 9].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 10].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 11].Value = "";
                        TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[iRowIndex, 3, iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    //3 cot tong
                    if (iTongsk == 0)
                    {
                        xlSheet.Cells[iRowIndex, 12].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 12].Value = iTongsk;
                    }
                    xlSheet.Cells[iRowIndex, 12].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (dTongvnd == 0)
                    {
                        xlSheet.Cells[iRowIndex, 13].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 13].Value = dTongvnd;
                    }
                    xlSheet.Cells[iRowIndex, 13].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    if (dTongusd == 0)
                    {
                        xlSheet.Cells[iRowIndex, 14].Value = "";
                    }
                    else
                    {
                        xlSheet.Cells[iRowIndex, 14].Value = dTongusd;
                    }
                    xlSheet.Cells[iRowIndex, 14].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 14, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    //reset dong tiep theo
                    iTongsk = 0;
                    dTongusd = 0;
                    dTongvnd = 0;

                    iRowIndex = iRowIndex + 1;

                }//end lstTp


            }//end lstTenTP

            if (iRowIndex > 0)
            {
                xlSheet.Cells[iRowIndex - 1, 1, iRowIndex - 1, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            }

            xlSheet.Cells.AutoFitColumns();
            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_chiphi_nhahang_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }

        #endregion

        #region Liet ke doan huy
        [HttpGet]
        public ActionResult Doanhuy(string tungay, string denngay, string thitruong)
        {
            thitruong = thitruong ?? "";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            ViewBag.thitruong = thitruong;

            listDsPhongban(thitruong);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanhuy");
            }
            return View("Doanhuy");

        }

        [HttpPost]
        public ActionResult DoanhuyToExcel(string tungay, string denngay, string thitruong)
        {
            return View();
        }

        #endregion

        #region Liệt kê doan xin visa
        public ActionResult Doanxinvisa(string tungay, string denngay)
        {
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("Doanxinvisa");
            }
            var d = _baocaoRepository.ListDoanxinVisa(tungay, denngay, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Doanxinvisa");
            }
            else
            {
                return View("Doanxinvisa", d);
            }

        }
        [HttpPost]
        public ActionResult DoanxinvisaToExcel(string tungay, string denngay)
        {
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.ListDoanxinVisa(tungay, denngay, HttpContext.Session.GetString("chinhanh"));
          
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 7, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // xlSheet.Cells[1, 1, 1, 5].Merge = true;
            string title = "BÁO CÁO ĐOÀN XIN VISA";

            xlSheet.Cells[2, 1].Value = title;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 7].Merge = true;
            string fromTo = "";
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 7].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 20;//sgtcode
            xlSheet.Column(3).Width = 20;//Thời gian
            xlSheet.Column(4).Width = 8;//sk
            xlSheet.Column(5).Width = 30;//thi truong
            xlSheet.Column(6).Width = 40;//Reference
            xlSheet.Column(7).Width = 15;// visa

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Đoàn";
            xlSheet.Cells[5, 3].Value = "Thời gian";
            xlSheet.Cells[5, 4].Value = "SK";
            xlSheet.Cells[5, 5].Value = "Thị trường";
            xlSheet.Cells[5, 6].Value = "Reference";
            xlSheet.Cells[5, 7].Value = "Visa";

            xlSheet.Cells[5, 1, 5, 7].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 7].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 7].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoanVisaViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.thoigian;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.thitruong;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.Reference;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                xlSheet.Cells[iRowIndex, 6].Style.WrapText = true;

                xlSheet.Cells[iRowIndex, 7].Value = vm.visa;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                idem += 1;
            }
            //set border dòng cuối

           // xlSheet.Cells[iRowIndex -1, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;
            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "ds_doanxinvisa" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
           );
        }
        #endregion

        #region Chi phi diem tham quan       

        [HttpGet]
        public ActionResult CPDTQ(string tungay, string denngay)
        {

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            //if (sortby == "A")
            //{
            //    ViewBag.checka = "checked";
            //    ViewBag.checkb = "";
            //}
            //else
            //{
            //    ViewBag.checka = "";
            //    ViewBag.checkb = "checked";
            //}

            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("CPDTQ");
            }
            var d = _baocaoRepository.listCPDiemTQ(tungay, denngay, HttpContext.Session.GetString("chinhanh"));

            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("CPDTQ");
            }
            else
            {
                return View("CPDTQ", d);
            }
        }
        [HttpPost]
        public ActionResult CPDTQToExcel(string tungay, string denngay)
        {
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;

            var d = _baocaoRepository.listCPDiemTQ(tungay, denngay, HttpContext.Session.GetString("chinhanh"));

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 6].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO CHI PHÍ CÁC ĐIỂM THAM QUAN ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 6].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày " + tungay;
            }
            else
            {
                fromTo = "Từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 6].Merge = true;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Điểm tham quan";
            xlSheet.Cells[5, 3].Value = "Khách NN ";
            xlSheet.Cells[5, 4].Value = "Khách VN  ";
            xlSheet.Cells[5, 5].Value = "Tổng SK ";
            xlSheet.Cells[5, 6].Value = "Chi phí VNĐ ";


            xlSheet.Cells[5, 1, 5, 6].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 6].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 6].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 6].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 6].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 6].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;

            //lay ra ten thanh pho
            List<string> lstTenThanhpho = d.Select(x => x.tentp).Distinct().ToList();

            int iTongKhachNN = 0, iTongKhachVN = 0, iTongsk = 0;
            decimal dChiphi = 0;

            foreach (string ten in lstTenThanhpho)
            {
                int iSttks = 0;
                List<CPDiemThamQuanViewModel> lstDiemtq = d.Where(x => x.tentp == ten).AsEnumerable().ToList();

                //tinh tong theo thanh pho truoc
                foreach (CPDiemThamQuanViewModel item in lstDiemtq)
                {
                    iTongKhachNN = iTongKhachNN + item.khachnuocngoai;//tong khach nn
                    iTongKhachVN = iTongKhachVN + item.khachviet;//tong khach vn
                    iTongsk = iTongsk + item.khachnuocngoai + item.khachviet;
                    dChiphi = dChiphi + item.cpvnd;
                }

                //dong tong theo thanh pho

                xlSheet.Cells[iRowIndex, 1].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = ten;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = iTongKhachNN;
                xlSheet.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = iTongKhachVN;
                xlSheet.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = iTongsk;
                xlSheet.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = dChiphi;
                xlSheet.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
                //reset
                iTongKhachNN = 0;
                iTongKhachVN = 0;
                iTongsk = 0;
                dChiphi = 0;

                // dong diemtq ===================================================================
                foreach (CPDiemThamQuanViewModel item in lstDiemtq)
                {

                    iSttks = iSttks + 1;
                    iTongsk = item.khachnuocngoai + item.khachviet;

                    xlSheet.Cells[iRowIndex, 1].Value = iSttks;
                    TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 2].Value = item.diemtq;
                    TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 3].Value = item.khachnuocngoai;
                    TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 4].Value = item.khachviet;
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 5].Value = iTongsk;
                    TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 6].Value = item.cpvnd;
                    xlSheet.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);
                    xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    //reset
                    iTongsk = 0;
                    iRowIndex += 1;
                }

                //DONG EMPTY
                xlSheet.Cells[iRowIndex, 1].Value = "";
                // TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);

                xlSheet.Cells[iRowIndex, 3].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);

                xlSheet.Cells[iRowIndex, 4].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);

                xlSheet.Cells[iRowIndex, 5].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);

                xlSheet.Cells[iRowIndex, 6].Value = "";
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Bold);

                xlSheet.Cells[iRowIndex, 1, iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //END DONG EMPTY
                iRowIndex += 1;
            }


            //end du lieu            

            if (iRowIndex > 0)
            {
                xlSheet.Cells[iRowIndex - 1, 1, iRowIndex - 1, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            }
            xlSheet.Cells.AutoFitColumns();

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_chiphi_diemtq_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );

        }


        #endregion

        #region List box

        public void listDsPhongban(string selected)
        {
            var model = _phongbanRepository.ListPhongban();

            List<SelectListItem> sel = new List<SelectListItem>();
            sel.Add(new SelectListItem { Text = "Tất cả thị trường", Value = "" });

            foreach (Phongban p in model)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = p.tenphong;
                sli.Value = p.maphong;
                sel.Add(sli);
            }

            ViewBag.dsphongban = new SelectList(sel, "Value", "Text", selected); ;
        }
        public void listChinhanh(string selected)
        {
            var model = _chinhanhRepository.ListChinhanh();

            List<SelectListItem> sel = new List<SelectListItem>();
            sel.Add(new SelectListItem { Text = "Tất cả chi nhánh", Value = "" });

            foreach (Dmchinhanh c in model)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = c.Macn;
                sli.Value = c.Macn;
                sel.Add(sli);
            }

            ViewBag.dschinhanh = new SelectList(sel, "Value", "Text", selected); 
        }
        public void listDichvu_baocao(string selected)
        {
            var model = _dichvuRepository.ListDichvu_baocao();

            List<SelectListItem> sel = new List<SelectListItem>();
            foreach (Dichvu c in model)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = c.Tendv;
                sli.Value = c.Iddichvu;
                sel.Add(sli);
            }

            ViewBag.dichvu = new SelectList(sel, "Value", "Text", selected); ;
        }


        #endregion

        #region Export Danh sách khách to Excel
      
        public ActionResult exportDsKhach(string sgtcode)
        {
            var d = _baocaoRepository.lisDanhsachkhachTour(sgtcode);
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 3].Merge = true;

            xlSheet.Cells[2, 1].Value = "DANH SÁCH KHÁCH ĐOÀN "+sgtcode;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 11].Merge = true;

            // Định dạng chiều dài cho cột

            xlSheet.Column(1).Width = 8;//stt
            xlSheet.Column(2).Width = 15;//makh
            xlSheet.Column(3).Width = 35;//ten khach
            xlSheet.Column(4).Width = 15;//ngay sinh
            xlSheet.Column(5).Width = 8;//phai
            xlSheet.Column(6).Width = 40;//dia chi
            xlSheet.Column(7).Width = 15;// dien thoai
            xlSheet.Column(8).Width = 15;// ho chieu
            xlSheet.Column(9).Width = 15;// cmnd
            xlSheet.Column(10).Width = 15;// quoctich
            xlSheet.Column(11).Width = 10;// loaiphong

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Mã KH";
            xlSheet.Cells[5, 3].Value = "Họ tên";
            xlSheet.Cells[5, 4].Value = "Ngày sinh";
            xlSheet.Cells[5, 5].Value = "Phái";
            xlSheet.Cells[5, 6].Value = "Địa chỉ";
            xlSheet.Cells[5, 7].Value = "Điện thoại";
            xlSheet.Cells[5, 8].Value = "Hộ chiếu";
            xlSheet.Cells[5, 9].Value = "CMND";
            xlSheet.Cells[5, 10].Value = "Quốc tịch";
            xlSheet.Cells[5, 11].Value = "Loại phòng";

            xlSheet.Cells[5, 1, 5, 11].Style.Font.SetFromFont(new Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 11].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 11].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 11].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 11].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 11].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 11].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 11].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER
            //du lieu
            int iRowIndex = 6;

            foreach (KhachTour k in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = k.stt;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = k.makh;
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = k.hoten;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = k.ngaysinh.HasValue? k.ngaysinh.Value.ToString("dd/MM/yyyy"):" / /";
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = k.phai ? "Nam" : "Nữ";
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = k.diachi;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 7].Value = k.dienthoai;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = k.hochieu;
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 9].Value = k.cmnd;
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 10].Value = k.quoctich;
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 11].Value = k.loaiphong;
                TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                iRowIndex += 1;
               
            }

            //set border dòng cuối

            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Formula = "";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 9].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 9].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 10].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 10].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 11].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 11].Style.Border.Top.Style = ExcelBorderStyle.None;

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "DSKhachDoan_"+sgtcode+"_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
                );

        }

        #endregion

    }
}