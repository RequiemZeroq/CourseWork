
using CourseWork.WebApi.Data;
using CourseWork.WebApi.Data.Repository;
using CourseWork.WebApi.Data.Repository.IRepository;
using CourseWork.WebApi.Models.DTOs;
using CourseWork.WebApi.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Text.Json.Serialization;
using CourseWork.Utility;

namespace CourseWork.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(),
                              "logs/important.json",
                              restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.File("logs/all-.logs",
                              rollingInterval: RollingInterval.Day)
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            //Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            //DB
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Mapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            //Validation
            builder.Services.AddScoped<IValidator<CategoryCreateDTO>, CategoryCreateDTOValidator>();
            builder.Services.AddScoped<IValidator<CategoryUpdateDTO>, CategoryUpdateDTOValidator>();
            builder.Services.AddScoped<IValidator<ProductUpdateDTO>, ProductUpdateDTOValidator>();
            builder.Services.AddScoped<IValidator<ProductCreateDTO>, ProductCreateDTOValidator>();

            //MVC
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.TokenValidationParameters.ValidateAudience = false;
                });

            //Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "apiAccess");
                });

                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "apiAccess");
                    policy.RequireRole(WebConstants.AdminRole);
                });
            });

            var app = builder.Build();

            using(var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                DbInitializer.Initialize(serviceProvider);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
