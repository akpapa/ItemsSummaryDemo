using ItemsSummary.BlazorServer.Components;
using ItemsSummary.Common.Services;
using ItemsSummary.ViewModel;
using Microsoft.Extensions.FileProviders;

namespace ItemsSummary.BlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                ConfigKestrel(builder);
                // Add services to the container.
                ConfigServices(builder.Services);

                var app = builder.Build();
                ConfigApp(app);
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ConfigApp(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            ConfigResources(app);
            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
        }

        private static void ConfigKestrel(WebApplicationBuilder builder)
        {
            builder.WebHost.UseKestrel(options =>
            {
                //only ipv4
                options.Listen(System.Net.IPAddress.Parse(GetLocalIpV4Address()), 6165);
            });
        }
        private static void ConfigServices(IServiceCollection services)
        {
            services.AddRazorComponents()
                    .AddInteractiveServerComponents();
            services.AddSingleton<IEnvironmentService, EnvironmentService>();
            services.AddSingleton<IFileService, FileServiceForBlazor>();
            services.AddSingleton<MainWindowViewModel>();
        }
        /// <summary>
        /// 写真の物理Pathを/imgにMapする
        /// </summary>
        /// <param name="app"></param>
        private static void ConfigResources(WebApplication app)
        {
            IFileService fileService = app.Services.GetRequiredService<IFileService>();
            string? oneDrivePath = fileService.GetOneDrivePath();
            if (string.IsNullOrEmpty(oneDrivePath)) return;
            string imagePath = $"{oneDrivePath}\\{Common.Constants.IMAGE_RELATIVE_PATH}";
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(imagePath),
                RequestPath = "/imgs"
            });
        }

        /// <summary>
        /// 念のためIPv4だけを使う
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EntryPointNotFoundException"></exception>
        private static string GetLocalIpV4Address()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    string ipStr = ip.ToString();
                    if (ipStr.ToString().StartsWith("192.168"))
                    {
                        return ipStr;
                    }
                }
            }
            throw new EntryPointNotFoundException("IPV4アドレスが見つかりません");
        }
    }
}
