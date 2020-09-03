using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace dieuhanhtour.Data.Repository
{
    public class BaocaoRepository : Repository<Tourinf>, IBaocaoRepository
    {
        public BaocaoRepository(qltourContext context) : base(context)
        {
        }

        public IEnumerable<ChuyenbayViewModel> listChuyenbayden(string tungay, string denngay, string thitruong, string sanbay, string chinhanh)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),                  
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@sanbay",sanbay),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            var d = _context.ChuyenbayViewModel.FromSql("spDsChuyenbayden @tungay,@denngay,@thitruong,@sanbay, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<ChuyenbayViewModel> listChuyenbaydenvadi(string tungay, string denngay, string thitruong, string sanbay, string chinhanh)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@sanbay",sanbay),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            var d = _context.ChuyenbayViewModel.FromSql("spDsChuyenbaydenvadi @tungay,@denngay,@thitruong,@sanbay, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<ChuyenbayViewModel> listChuyenbaydi(string tungay, string denngay, string thitruong, string sanbay, string chinhanh)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@sanbay",sanbay),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            var d = _context.ChuyenbayViewModel.FromSql("spDsChuyenbaydi @tungay,@denngay,@thitruong,@sanbay, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<ChuyenbayViewModel> listChuyenbaydoan(string tungay, string denngay, string thitruong, string chinhanh)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            var d = _context.ChuyenbayViewModel.FromSql("spDsChangbaydoan @tungay,@denngay,@thitruong, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<DoanbatdauViewModel> listDoanbatdau(string tungay, string denngay, string thitruong,string chinhanh)
        {
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh)
              };
            var d = _context.DoanbatdauViewModel.FromSql("spDsTourbatdau @tungay,@denngay,@thitruong, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<DoanbatdauViewModel> listDoanketthuc(string tungay, string denngay, string thitruong, string chinhanh)
        {
            var parameter = new SqlParameter[]
             {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh)
             };
            var d = _context.DoanbatdauViewModel.FromSql("spDsTourketthuc @tungay,@denngay,@thitruong, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<DoanbatdauViewModel> listDoanlocktour(string tungay, string denngay, string thitruong, string chinhanh)
        {
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh)
              };
            var d = _context.DoanbatdauViewModel.FromSql("spDsTourlock @tungay,@denngay,@thitruong, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<DoancanoViewModel> ListDoantheocano(string tungay, string denngay, string thitruong, string chinhanh,string sortby)
        {
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@sortby",sortby)
              };
            var d = _context.DoancanoViewModel.FromSql("spDoantheoCano @tungay,@denngay,@thitruong,@chinhanh,@sortby", parameter);
            return d;
        }
        public IEnumerable<DoancanoViewModel> ListDoantheoroinuoc(string tungay, string denngay, string thitruong, string chinhanh,string sortby)
        {
            var parameter = new SqlParameter[]
             {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@sortby",sortby)
             };
            var d = _context.DoancanoViewModel.FromSql("spDoantheoRoinuoc @tungay,@denngay,@thitruong,@chinhanh, @sortby", parameter);
            return d;
        }
        public IEnumerable<DoankhachsanViewModel> ListDoantheokhachsan(string tungay, string denngay, string thitruong, string chinhanh,string sortby)
        {
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@sortby",sortby)
              };
            var d = _context.DoankhachsanViewModel.FromSql("spDsDoantheokhachsan @tungay,@denngay,@thitruong,@chinhanh,@sortby", parameter);
            return d;
        }

        public IEnumerable<DoannhahangViewModel> ListDoantheonhahang(string tungay, string denngay, string thitruong, string chinhanh,string sortby)
        {
            var parameter = new SqlParameter[]
             {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@sortby",sortby)
             };
            var d = _context.DoannhahangViewModel.FromSql("spDoantheoNhahang @tungay,@denngay,@thitruong,@chinhanh,@sortby", parameter);
            return d;
        }

       

        public IEnumerable<RoomniteViewModel> listRoomNite(string tungay, string denngay, string chinhanh)
        {
            var parameter = new SqlParameter[]
          {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@chinhanh",chinhanh)
          };
            var d = _context.RoomniteViewModel.FromSql("spDsRoomNite @tungay,@denngay, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<DoancanoViewModel> ListDoantheovannghe(string tungay, string denngay, string thitruong, string chinhanh, string sortby)
        {
            var parameter = new SqlParameter[]
             {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@sortby",sortby)
             };
            var d = _context.DoancanoViewModel.FromSql("spDoantheovannghe @tungay,@denngay,@thitruong,@chinhanh, @sortby", parameter);
            return d;
        }

        public IEnumerable<DoancanoViewModel> ListDoantheoxelua(string tungay, string denngay, string thitruong, string chinhanh, string sortby)
        {
            var parameter = new SqlParameter[]
             {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@sortby",sortby)
             };
            var d = _context.DoancanoViewModel.FromSql("spDoantheoxelua @tungay,@denngay,@thitruong,@chinhanh, @sortby", parameter);
            return d;
        }

        public IEnumerable<DoanthamquanViewModel> ListDoantheodiemthamquan(string tungay, string denngay, string chinhanh, string thu, string sortby)
        {
            var parameter = new SqlParameter[]
            {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@thustr",thu),
                    new SqlParameter("@sortby",sortby)
            };
            var d = _context.DoanthamquanViewModel.FromSql("spDoantheodiemthamquan @tungay,@denngay,@chinhanh,@thustr, @sortby", parameter);
            return d;
        }


        public IEnumerable<DoancanoViewModel> ListDoantheodichvukhac(string tungay, string denngay, string thitruong, string dichvu, string chinhanh, string sortby)
        {
            var parameter = new SqlParameter[]
            {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@thitruong",thitruong),
                    new SqlParameter("@dichvu",dichvu),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@sortby",sortby)
            };
            var d = _context.DoancanoViewModel.FromSql("spDoantheodichvukhac @tungay,@denngay,@thitruong,@dichvu,@chinhanh, @sortby", parameter);
            return d;
        }

        public IEnumerable<HuongdanDoanViewModel> ListHuongdanDoan(string tungay, string denngay, string chinhanh, string giatri)
        {
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@giatri",giatri)
              };
            var d = _context.HuongdanDoanViewModel.FromSql("spDsHuongdanDoan @tungay,@denngay, @chinhanh, @giatri", parameter);
            return d;
        }

        public IEnumerable<HuongdanDoanViewModel> ListHuongdanDoanDiDoan(string tungay, string denngay, string chinhanh)
        {
            var parameter = new SqlParameter[]
             {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@chinhanh",chinhanh)
             };
            var d = _context.HuongdanDoanViewModel.FromSql("spDsHuongdanDoanDiDoan @tungay,@denngay, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<XeDoanViewModel> ListXeDoan(string tungay, string denngay, string chinhanh, string giatri)
        {
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@chinhanh",chinhanh),
                    new SqlParameter("@giatri",giatri)
              };
            var d = _context.XeDoanViewModel.FromSql("spDsXeDoan @tungay,@denngay, @chinhanh, @giatri", parameter);
            return d;
        }

        public IEnumerable<CPNhaHangViewModel> listCPNhaHang(string tungay, string denngay, string chinhanh)
        {
            var parameter = new SqlParameter[]
          {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@chinhanh",chinhanh)
          };
            var d = _context.CPNhaHangViewModel.FromSql("spDsChiPhiNhaHang @tungay,@denngay, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<CPDiemThamQuanViewModel> listCPDiemTQ(string tungay, string denngay, string chinhanh)
        {
            var parameter = new SqlParameter[]
             {
                            new SqlParameter("@tungay",tungay),
                            new SqlParameter("@denngay",denngay),
                            new SqlParameter("@chinhanh",chinhanh)
             };
            var d = _context.CPDiemThamQuanViewModel.FromSql("spDsCpDiemThamQuan @tungay,@denngay, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<DoanVisaViewModel> ListDoanxinVisa(string tungay, string denngay, string chinhanh)
        {
            var parameter = new SqlParameter[]
             {
                            new SqlParameter("@tungay",tungay),
                            new SqlParameter("@denngay",denngay),
                            new SqlParameter("@chinhanh",chinhanh)
             };
            var d = _context.DoanVisaViewModel.FromSql("spDoanxinVisa @tungay,@denngay, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<KhachTour> lisDanhsachkhachTour(string sgtcode)
        {
            return _context.KhachTour.Where(x => x.sgtcode == sgtcode && x.del==false);
        }
    }
}
