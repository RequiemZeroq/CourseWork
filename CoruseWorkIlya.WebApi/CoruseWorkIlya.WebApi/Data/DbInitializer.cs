using AutoMapper.Features;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.WebApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider services)
        {
            var db = services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            if(db.Database.GetPendingMigrations().Count() > 0)
            {
                db.Database.Migrate();
            }
        }
    }
}
