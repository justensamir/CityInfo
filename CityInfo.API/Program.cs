using CityInfo.API.Data;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace CityInfo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/cityinfo.text", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
              .AddXmlDataContractSerializerFormatters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #if DEBUG
                builder.Services.AddTransient<IMailService, LocalMailService>();
#else
                builder.Services.AddTransient<IMailService, CloudMailService>();
#endif
            builder.Services.AddDbContext<CityContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("city_conn"));
            });
            builder.Services.AddSingleton<CityDataStore>();
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
            builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
            
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options => options.TokenValidationParameters = new ()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
               
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeFromCairo", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("city", "Cairo");
                    policy.RequireRole("Admin", "Owner");
                });
            });

            builder.Services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}