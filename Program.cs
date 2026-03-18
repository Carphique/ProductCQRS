using ProductCQRS.Profiles;
using ProductCQRS.Services;
using Serilog;
using System.Threading.RateLimiting;

namespace ProductCQRS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.Configure<AppSettingsProfile>(
                builder.Configuration.GetSection("AppSettings"));   

            builder.Services.Configure<AdminUsersProfile>(
                builder.Configuration.GetSection("AdminUserProfile"));

            builder.Services.Configure<PaginationProfile>(
                builder.Configuration.GetSection("Pagination"));

            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<PaginationService>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(
                    "logs/log.txt",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //For appsettings.json(Loger)
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            //rate limited
            var rateLimited = builder.Configuration.GetSection("RateLimiting");

            var permitLimit = rateLimited.GetValue<int>("PermitLimit");
            var windowLimit = rateLimited.GetValue<int>("WindowMinutes");
            var queueLimit = rateLimited.GetValue<int>("QueueLimit");

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.GlobalLimiter = PartitionedRateLimiter
                .Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: "global",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = TimeSpan.FromMinutes(windowLimit),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = queueLimit,
                            AutoReplenishment = true
                        }));
                
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}