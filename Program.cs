
using DirWatcher.Data;
using DirWatcher.Models;
using DirWatcher.Services;
using DirWatcher.Services.WatcherManagementService;
using Microsoft.EntityFrameworkCore;

namespace DirWatcher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<WatcherService>();
            builder.Services.AddScoped<IDirWatcherRepository, DirWatcherRepository>();
            builder.Services.AddScoped<IWatcherService, WatcherService>();
            builder.Services.AddSingleton<IDirWatcherBgService, DirWatcherBgService>();
            builder.Services.AddHostedService<DirWatcherBgService>();

            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlite(builder.Configuration.GetConnectionString("DirWatcherSqlite")));
            builder.Services.Configure<TaskDatabaseSettings>(builder.Configuration.GetSection("TaskDatabase"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            Console.WriteLine("--> Prepping DB...");
            PrepDb.PrepPopulation(app);

            app.Run();
        }
    }
}
