using Catalog.API.Grpc;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistance;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                // Port 8080 dla zwyk³ego API (REST) - obs³uguje HTTP/1.1 (Swagger, przegl¹darka)
                options.ListenAnyIP(8080, listenOptions =>
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1);

                // Port 8081 specjalnie dla gRPC - obs³uguje HTTP/2 (komunikacja miêdzy serwisami)
                options.ListenAnyIP(8081, listenOptions =>
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<CatalogContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddGrpc();

            var app = builder.Build();

            app.MapGrpcService<CatalogGrpcService>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<CatalogContext>();

                context.Database.Migrate();

                await CatalogContextSeed.SeedAsync(context);
            }

            app.Run();
        }
    }
}
