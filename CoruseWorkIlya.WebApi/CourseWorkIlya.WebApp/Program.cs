using CourseWork.WebApp.Services;
using CourseWork.WebApp.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;
using CourseWork.Utility;

namespace CourseWork.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //ApiService
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IProductService, ProductService>();   

            //HTTP
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();

            //MVC
            builder.Services.AddControllersWithViews();

            //Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:5001";

                options.ClientId = builder.Configuration["Authentication:IdentityServer:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:IdentityServer:ClientSecret"]!;
                options.ResponseType = "code";

                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                //Scopes 
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("apiAccess");
                options.Scope.Add("offline_access");

                options.ClaimActions.MapJsonKey(
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Role);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                };

                options.MapInboundClaims = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.SaveTokens = true;
            });

            //Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", options =>
                {
                    options.RequireAuthenticatedUser();
                    options.RequireRole(WebConstants.AdminRole);
                });

                options.AddPolicy("User", options =>
                {
                    options.RequireAuthenticatedUser();
                });
            });

            //Mapping
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
