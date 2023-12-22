using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RAMWebServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration= configuration;
        }

        public IConfiguration Configuration { get; }

        // Этот метод вызывается средой выполнения. Используйте этот метод для добавления служб в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            //Добавление сервиса для создания базы данных
            services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("RAM_Db")));


            //Добавление сервиса для аутентификации
            //services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

            services.AddControllers();
        }

        // Этот метод вызывается средой выполнения. Используйте этот метод для настройки конвейера HTTP-запросов.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
