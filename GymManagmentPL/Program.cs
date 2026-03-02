using GymManagmentBLL;
using GymManagmentBLL.Service.Classes;
using GymManagmentBLL.Service.Classes.AttachmentServices;
using GymManagmentBLL.Service.Interfaces;
using GymManagmentBLL.Service.Interfaces.AttachmentService;
using GymManagmentBLL.Services.Classes;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Data.DataSeeding;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repository.Classes;
using GymManagmentDAL.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});
            //builder.Services.AddScoped(typeof(IGenericRepositorycs<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlaneRepository, PlaneRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService,TrainerService>();
            builder.Services.AddScoped<IPlanServicecs, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentSeervice>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(confg =>
            {
                //confg.Password.RequireUppercase=true;
                //confg.Password.RequireLowercase=true;
                //confg.Password.RequiredLength = 6;
                confg.User.RequireUniqueEmail=true;
            }).AddEntityFrameworkStores<GymDBContext>();
            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/Account/AccessDenied";
            });

			var app = builder.Build();
            #region DataSeed
            using var scope= app.Services.CreateScope();
            var dbcontext=scope.ServiceProvider.GetRequiredService<GymDBContext>();
            var roleManager= scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager=scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var pendingMigration= dbcontext.Database.GetPendingMigrations();
            if (pendingMigration?.Any() ?? false)
            {
                dbcontext.Database.Migrate();
			}
			GymDbContextSeeding.SeedData(dbcontext, app.Environment.ContentRootPath);
            IdentityDbContextSeeding.SeedData(roleManager, userManager);
			#endregion

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
     
			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id:?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
