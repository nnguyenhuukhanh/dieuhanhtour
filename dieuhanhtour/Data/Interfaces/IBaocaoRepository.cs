using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IBaocaoRepository:IRepository<Tourinf>
    {
        IEnumerable<DoanbatdauViewModel> listDoanbatdau(string tungay,string denngay,string thitruong,string chinhanh);

        IEnumerable<DoanbatdauViewModel> listDoanketthuc(string tungay, string denngay, string thitruong, string chinhanh);

        IEnumerable<DoanbatdauViewModel> listDoanlocktour(string tungay, string denngay, string thitruong, string chinhanh);

        IEnumerable<ChuyenbayViewModel> listChuyenbayden(string tungay, string denngay, string thitruong,string sanbay, string chinhanh);

        IEnumerable<ChuyenbayViewModel> listChuyenbaydi(string tungay, string denngay, string thitruong, string sanbay, string chinhanh);

        IEnumerable<ChuyenbayViewModel> listChuyenbaydenvadi(string tungay, string denngay, string thitruong, string sanbay, string chinhanh);

        IEnumerable<ChuyenbayViewModel> listChuyenbaydoan(string tungay, string denngay, string thitruong, string chinhanh);

        IEnumerable<RoomniteViewModel> listRoomNite(string tungay, string denngay, string chinhanh);
        
        IEnumerable<DoankhachsanViewModel> ListDoantheokhachsan(string tungay, string denngay,string thitruong, string chinhanh,string sortby);

        IEnumerable<DoannhahangViewModel> ListDoantheonhahang(string tungay, string denngay, string thitruong, string chinhanh,string sortby);

        IEnumerable<DoancanoViewModel> ListDoantheocano(string tungay, string denngay, string thitruong, string chinhanh,string sortby);

        IEnumerable<DoancanoViewModel> ListDoantheoroinuoc(string tungay, string denngay, string thitruong, string chinhanh,string sortby);

        IEnumerable<DoancanoViewModel> ListDoantheovannghe(string tungay, string denngay, string thitruong, string chinhanh, string sortby);

        IEnumerable<DoancanoViewModel> ListDoantheoxelua(string tungay, string denngay, string thitruong, string chinhanh, string sortby);

        IEnumerable<DoanthamquanViewModel> ListDoantheodiemthamquan(string tungay, string denngay, string chinhanh, string thu, string sortby);

        IEnumerable<DoancanoViewModel> ListDoantheodichvukhac(string tungay, string denngay, string thitruong,string dichvu, string chinhanh, string sortby);

        IEnumerable<HuongdanDoanViewModel> ListHuongdanDoan(string tungay, string denngay, string chinhanh, string giatri);

        IEnumerable<HuongdanDoanViewModel> ListHuongdanDoanDiDoan(string tungay, string denngay, string chinhanh);

        IEnumerable<XeDoanViewModel> ListXeDoan(string tungay, string denngay, string chinhanh, string giatri);

        IEnumerable<CPNhaHangViewModel> listCPNhaHang(string tungay, string denngay, string chinhanh);

        IEnumerable<CPDiemThamQuanViewModel> listCPDiemTQ(string tungay, string denngay, string chinhanh);

        IEnumerable<DoanVisaViewModel> ListDoanxinVisa(string tungay, string denngay, string chinhanh);

        IEnumerable<KhachTour> lisDanhsachkhachTour(string sgtcode);

    }
}
