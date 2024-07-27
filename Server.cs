using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

public class Server
{
    private IHost _host;

    public async Task StartAsync(int port)
    {
        _host = CreateHostBuilder(port).Build();

        await _host.StartAsync();

    }

    public async Task StopAsync()
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
    }

    public static IHostBuilder CreateHostBuilder(int port)
    {
        var wwwrootpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wwwroot");
        var hostbuilder = new HostBuilder()
            //TODO : ?
            .UseContentRoot(wwwrootpath)

            // using ConfigureWebHostDefaults is crucial, loads of opaque configuration.
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(options =>
                {
                    options.ListenLocalhost(port);
                });
                webBuilder.Configure(app =>
                {
                    app.UseStaticFiles();
                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/", async context =>
                        {

                            var filePath = Path.Combine(wwwrootpath, "index.html");
                            if (!File.Exists(filePath))
                            {
                                throw new FileNotFoundException("index.html not found", filePath);
                            }
                            var htmlContent = File.ReadAllText(filePath);
                            await context.Response.WriteAsync(htmlContent);
                        });
                    });
                });
            });
        return hostbuilder;
    }
}
