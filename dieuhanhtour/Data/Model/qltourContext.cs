using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;

namespace dieuhanhtour.Data.Model
{
    public class qltourContext : DbContext
    {
        public qltourContext(DbContextOptions<qltourContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Passenger>().HasKey(table => new
            {
                table.Sgtcode,
                table.Stt
            });
            builder.Entity<BookedViewModel>().HasKey(table => new
            {
                table.Sgtcode,
                table.Supplierid
            });
            

            builder.Entity<Booked>().Property(a => a.Idbooking)
                    .HasColumnName("Idbooking")
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

            //builder.Entity<Booked>().HasKey(table => new
            //{
            //    table.Sgtcode,
            //    table.Times
            //});
            builder.Entity<TourProgTemp>().Property(a => a.Id)
                    .HasColumnName("Id")
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();
            builder.Entity<Tourprog>().Property(a => a.Id)
                    .HasColumnName("Id")
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

            builder.Entity<Dieuxe>().Property(a => a.Idxe)
                   .HasColumnName("Idxe")
                   .HasColumnType("decimal(18, 0)")
                   .ValueGeneratedOnAdd();

            builder.Entity<Hoteltemp>().Property(a => a.Id)
                   .HasColumnName("Id")
                   .HasColumnType("decimal(18, 0)")
                   .ValueGeneratedOnAdd();
           

            builder.Entity<SightseeingTemp>().Property(a => a.Id)
               .HasColumnName("Id")
               .HasColumnType("decimal(18, 0)")
               .ValueGeneratedOnAdd();
            builder.Entity<Sightseeing>().Property(a => a.Id)
               .HasColumnName("Id")
               .HasColumnType("decimal(18, 0)")
               .ValueGeneratedOnAdd();
            builder.Entity<Huongdan>().Property(a => a.IdHuongdan)
                  .HasColumnName("IdHuongdan")
                  .HasColumnType("decimal(18, 0)")
                  .ValueGeneratedOnAdd();

            builder.Entity<KhachTour>().Property(a => a.IdKhach)
                .HasColumnName("IdKhach")
                .HasColumnType("decimal(18, 0)")
                .ValueGeneratedOnAdd();
            builder.Entity<Chiphikhac>().Property(a => a.idorthercost)
               .HasColumnName("idorthercost")
               .HasColumnType("decimal(18, 0)")
               .ValueGeneratedOnAdd();
            builder.Entity<Hotel>().Property(a => a.Id)
              .HasColumnName("Id")
              .HasColumnType("decimal(18, 0)")
              .ValueGeneratedOnAdd();
        }

        public DbSet<LoginModel> LoginModel { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Dmchinhanh> Dmchinhanh { get; set; }
        public DbSet<Phongban> Phongban { get; set; }
        public DbSet<Dmdaily> Dmdaily { get; set; }
        public DbSet<Thanhpho> Thanhpho { get; set; }
        public DbSet<Quan> Quan { get; set; }
        public DbSet<Quocgia> Quocgia { get; set; }
        public DbSet<Dichvu> Dichvu { get; set; }
        public DbSet<Tuyentq> Tuyentq { get; set; }
        public DbSet<TourTemplate> Tourtemplate { get; set; }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<DsLoaixe> DsLoaixe { get; set; }
        public DbSet<Huongdan> Huongdan { get; set; }
        public DbSet<Tournode> Tournode { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Dieuxe> Dieuxe { get; set; }
        public DbSet<Booked> Booked { get; set; }
        public DbSet<Dmdiemtq> Dmdiemtq { get; set; }
        public DbSet<TourProgTemp> TourProgTemp { get; set; }
        public DbSet<TourTempNote> TourTempNote { get; set; }
        public DbSet<Ngoaite> Ngoaite { get; set; }
        public DbSet<country> country { get; set; }
        public DbSet<Tourkind> Tourkind { get; set; }
        public DbSet<Hoteltemp> Hoteltemp { get; set; }
        public DbSet<SightseeingTemp> SightseeingTemp { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Tourinf> Tourinf { get; set; }
        public DbSet<Tourprog> Tourprog { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Sightseeing> Sightseeing { get; set; }
        public DbSet<PassType> PassType { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Chiphikhac> Chiphikhac { get; set; }
        public DbSet<KhachTour> KhachTour { get; set; }
        public DbSet<DmLoaiphong> DmLoaiphong { get; set; }
        public DbSet<Vungmien> Vungmien { get; set; }
        public DbSet<Tinh> Tinh { get; set; }
        public DbSet<vTinh> vTinh { get; set; }
        public DbSet<vDmdiemtq> vDmdiemtq { get; set; }

        //// Dành cho các báo cáo

        public DbSet<DoanbatdauViewModel> DoanbatdauViewModel{get;set;}

        //// Dành cho các báo cáo

        public DbSet<ChuyenbayViewModel> ChuyenbayViewModel { get; set; }

        public DbSet<RoomniteViewModel> RoomniteViewModel { get; set; }

        public DbSet<DoankhachsanViewModel> DoankhachsanViewModel { get; set; }

        public DbSet<DoannhahangViewModel> DoannhahangViewModel { get; set; }

        public DbSet<DoancanoViewModel> DoancanoViewModel { get; set; }

        public DbSet<ConfirmdichvuViewModel> ConfirmdichvuViewModel { get; set; }

        public DbSet<HuongdanDoanViewModel> HuongdanDoanViewModel { get; set; }

        public DbSet<XeDoanViewModel> XeDoanViewModel { get; set; }

        public DbSet<CPNhaHangViewModel> CPNhaHangViewModel { get; set; }

        public DbSet<DoanhuyViewModel> DoanhuyViewModel { get; set; }

        public DbSet<DoanthamquanViewModel> DoanthamquanViewModel { get; set; }
        public DbSet<CPDiemThamQuanViewModel> CPDiemThamQuanViewModel { get; set; }
        public DbSet<DoanVisaViewModel> DoanVisaViewModel { get; set; }

        public DbSet<SupplierByCode> SupplierByCodes { get; set; }

        public DbSet<BookedViewModel> BookedViewModels { get; set; }
    }
}
