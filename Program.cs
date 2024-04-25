using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityWorkShop.Data;
using IdentityWorkShop.Areas.Identity.Data;
namespace IdentityWorkShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = "server=localhost;Database=entitytest;user=root;password=root";
            builder.Services.AddDbContext<databaseContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddDefaultIdentity<IdentityWorkShopUser>(options => options.SignIn.RequireConfirmedAccount = false)
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<databaseContext>();

            var roleManager = builder.Services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Moderator", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
        }
    }
}
