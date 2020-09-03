using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Repository;
using dieuhanhtour.Data.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace dieuhanhtour
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            services.AddDbContext<qltourContext>(option => option.UseSqlServer(Configuration.GetConnectionString("qlTourConectionString")));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

          
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDichvuRepository, DichvuRepository>();
            services.AddTransient<ITuyentqRepository, TuyentqRepository>();
            services.AddTransient<IThanhphoRepository, ThanhphoRepository>();
            services.AddTransient<IChinhanhRepository, ChinhanhRepository>();
            services.AddTransient<ITourTempRepository, TourTempRepository>();
            services.AddTransient<ILoaixeRepository, LoaixeRepository>();
            services.AddTransient<ITournodeRepository, TournodeRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IQuocgiaRepository, QuocgiaRepository>();
            services.AddTransient<IDiemtqRepository, DiemtqRepository>();
            services.AddTransient<ITourProgTempRepository, TourProgTempRepository>();
            services.AddTransient<INgoaiteRepository, NgoaiteRepository>();
            services.AddTransient<ITourTempNoteRepository, TourTempNoteRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<ITourkindRepository, TourkindRepository>();
            services.AddTransient<IHotelTempRepository, HotelTempRepository>();
            services.AddTransient<ISightseeingTempRepository, SightseeingTempRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ITourinfRepository, TourinfRepository>();
            services.AddTransient<ITourprogRepository, TourprogRepository>();
            services.AddTransient<IHotelRepository, HotelRepository>();
            services.AddTransient<ISightseeingRepository, SightseeingRepository>();
            services.AddTransient<IPhongbanRepository, PhongbanRepository>();
            services.AddTransient<IPasstypeRepository, PasstypeRepository>();
            services.AddTransient<IUserinfoRepository, UserinfoRepository>();
            services.AddTransient<IDieuxeRepository, DieuxeRepository>();
            services.AddTransient<IHuongdanRepository, HuongdanRepository>();
            services.AddTransient<IKhachTourRepository, KhachTourRepository>();
            services.AddTransient<ILoaiphongRepository, LoaiphongRepository>();
            services.AddTransient<IChiphiRepository, ChiphiRepository>();
            services.AddTransient<IBaocaoRepository, BaocaoRepository>();
            services.AddTransient<IBookedRepository, BookedRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var supportedCultures = new[] { new CultureInfo("en-AU") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-AU"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
