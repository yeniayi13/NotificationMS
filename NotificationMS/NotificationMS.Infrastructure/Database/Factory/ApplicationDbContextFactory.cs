using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using NotificationMS.Infrastructure.Database.Context.Postgres;

namespace NotificationMS.Infrastructure.Database.Factory.Postgres
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=ProductMs;Username=postgres;Password=yeniree0813");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
