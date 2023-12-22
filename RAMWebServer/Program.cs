using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using RAMWebServer;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
        //var host = new WebHostBuilder()
        //    .UseKestrel()
        //    .UseUrls("http://localhost:12345")
        //    .Configure(app =>
        //    {
        //        app.Run(async context =>
        //        {
        //            string filePath = context.Request.Query["file"];
        //            if (!string.IsNullOrEmpty(filePath))
        //            {
        //                //await SendFileAsync(context.Response, filePath);
        //            }
        //            else
        //            {
        //                await context.Response.WriteAsync("Не указан путь к файлу для скачивания.");
        //            }
        //        });
        //    })
        //    .Build();

        //host.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseKestrel().UseUrls("http://localhost:12345");
            webBuilder.UseStartup<Startup>();
        });
}