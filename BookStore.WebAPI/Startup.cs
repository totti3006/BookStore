using BookStore.Application.Configurations;
using BookStore.Application.Interfaces.Repositories;
using BookStore.Application.Interfaces.Services;
using BookStore.Application.Services;
using BookStore.Infrastructure.Context;
using BookStore.Infrastructure.Repositories;
using BookStore.WebAPI.Extensions;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace BookStore.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                             .WriteTo.Console()
                             .WriteTo.File(path: "logs/log.txt", 
                                           fileSizeLimitBytes: 5 * 1024 * 1024,
                                           rollingInterval: RollingInterval.Hour)
                             .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.Configure<JWTSection>(Configuration.GetSection("JWTSection"));

            services.AddSingleton<JWTSection>();

            services.Configure<SMTPConfig>(Configuration.GetSection("SMTPConfig"));

            services.AddSingleton<SMTPConfig>();

            services.AddSerilog();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("JWTSection")["Issuer"],
                        ValidAudience = Configuration.GetSection("JWTSection")["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            Configuration.GetSection("JWTSection")["SecretKey"])),
                    };
                });

            services.AddHangfire(config => config.UseMemoryStorage());

            services.AddHangfireServer();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbInitializer>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IReportService, ReportService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string cronExpr = "59 23 * * *"; // everyday at 23:59
            string cronExprTest = "*/1 * * * *"; // every a minute pass

            RecurringJob.AddOrUpdate<IReportService>("Report", r => r.GenerateDailyReport(), cronExpr);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCustomGlobalExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
