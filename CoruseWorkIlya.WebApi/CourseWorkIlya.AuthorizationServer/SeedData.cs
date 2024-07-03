using CourseWork.Utility;
using CourseWork.AuthorizationServer.Data;
using CourseWork.AuthorizationServer.Models;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace CourseWork.AuthorizationServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                //Identity Initialize
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                var adminRole = roleMgr.FindByNameAsync(WebConstants.AdminRole).Result;
                if(adminRole is null)
                {
                    adminRole = new IdentityRole
                    {
                        Name = WebConstants.AdminRole,
                    };

                    var result = roleMgr.CreateAsync(adminRole).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }


                var adminUser = userMgr.FindByNameAsync("admin").Result;
                if(adminUser is null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = WebConstants.AdminRole,
                        Email = "admin@admin.com",
                        EmailConfirmed = true,
                    };

                    var result = userMgr.CreateAsync(adminUser, "admin").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRoleAsync(adminUser, WebConstants.AdminRole).Result;

                    if(!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }



                //OIDC and OAuth Initialize
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var configurationContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configurationContext!.Database.Migrate();

                if (!configurationContext.Clients.Any())
                {
                    foreach(var client in Config.Clients) 
                    {
                        configurationContext.Clients.Add(client.ToEntity());
                    }

                    configurationContext.SaveChanges();
                }

                if(!configurationContext.ApiScopes.Any())
                {
                    foreach(var apiScope in Config.ApiScopes)
                    {
                        configurationContext.ApiScopes.Add(apiScope.ToEntity());
                    }

                    configurationContext.SaveChanges();
                }

                if (!configurationContext.IdentityResources.Any())
                {
                    foreach(var apiResource in Config.IdentityResources)
                    {
                        configurationContext.IdentityResources.Add(apiResource.ToEntity());
                    }

                    configurationContext.SaveChanges();
                }
            }
        }
    }
}
