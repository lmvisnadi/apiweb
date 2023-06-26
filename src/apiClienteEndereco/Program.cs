using Microsoft.AspNetCore;
using apiClienteEndereco;
using Autofac.Extensions.DependencyInjection;

namespace KeL.Admin.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>()
                .Build();
    }
}

