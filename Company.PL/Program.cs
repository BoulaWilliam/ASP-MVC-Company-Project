using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Company.DAL.Contexts;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.PL.MapperProfile;
using Microsoft.AspNetCore.Identity;
using Company.DAL.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            var Builder = WebApplication.CreateBuilder(args);

            #region Configure Services That Allow Dependacy Injection
            Builder.Services.AddControllersWithViews();
            Builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            Builder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            Builder.Services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));

            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;

            }).AddEntityFrameworkStores<CompanyDbContext>()
            .AddDefaultTokenProviders(); //Default Tokens 



            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "Account/Login";
                        options.AccessDeniedPath = "Home/Error";

                    });// Default Schema
            #endregion

            var app = Builder.Build();
            var env = Builder.Environment;
            #region Configure Http Request Pipline

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion

            app.Run();
        }

    }
}
