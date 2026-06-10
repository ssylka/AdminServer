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

            var conn = builder.Configuration.GetConnectionString("Default")
    ?? "postgresql://postgre:RU61uyXeOEMcwHmB2isCA0C3GNcY94zg@dpg-d80pug3eo5us73fp870g-a/inventory_p6z2";

            var uri = new Uri(conn);
            var port = uri.Port > 0 ? uri.Port : 5432;
            var userInfo = uri.UserInfo.Split(':');
            var isInternal = !uri.Host.Contains(".");
            var sslPart = isInternal ? "SSL Mode=Disable" : "SSL Mode=Require;Trust Server Certificate=true";
            
            var connectionString =
                $"Host={uri.Host};" +
                $"Port={port};" +
                $"Database={uri.AbsolutePath.Trim('/')};" +
                $"Username={userInfo[0]};" +
                $"Password={userInfo[1]};" +
                sslPart;
            
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
