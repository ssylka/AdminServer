using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Services;

namespace WebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var conn = builder.Configuration.GetConnectionString("Default");

            if (string.IsNullOrEmpty(conn))
            {
                conn = "postgresql://postgre:tLZCpu7ssG8IQnok41aBGd37mA6uIVcD@dpg-d7cljnvlk1mc73ftr2ag-a.oregon-postgres.render.com/postgre_loth";
            }

            var uri = new Uri(conn);
            var port = uri.Port > 0 ? uri.Port : 5432;

            var userInfo = uri.UserInfo.Split(':');

            var connectionString =
                $"Host={uri.Host};" +
                $"Port={port};" +
                $"Database={uri.AbsolutePath.Trim('/')};" +
                $"Username={userInfo[0]};" +
                $"Password={userInfo[1]};" +
                $"SSL Mode=Require;Trust Server Certificate=true";


            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped<EmailService>();


            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/User/Login";
                });

            builder.Services.AddAuthorization();

            //builder.Services.AddDbContext<ApplicationDBContext>(options =>
            //options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            var app = builder.Build();

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

            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                db.Database.Migrate();
            }

            app.Run();

        }
    }
}
