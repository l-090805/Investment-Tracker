using Investment_Tracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Investment_Tracker.Services;
using Microsoft.Extensions.Caching.Memory;
namespace Investment_Tracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient<TwelveDataPriceService>();
            builder.Services.AddScoped<PortfolioService>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FlutterFlow", p =>
                    p.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod());
            });

            var app = builder.Build();

            app.UseCors("FlutterFlow");

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await DbInitializer.SeedAsync(db);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
