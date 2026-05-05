using Ocelot.DependencyInjection;
using Ocelot.Middleware; // Dodaj ten using!

namespace ApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args) // Zmieniamy na async Task
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            app.MapGet("/", () => "Hello World! Gateway jest uruchomiony.");

            // TO JEST KLUCZOWA LINIA:
            await app.UseOcelot();

            await app.RunAsync();
        }
    }
}