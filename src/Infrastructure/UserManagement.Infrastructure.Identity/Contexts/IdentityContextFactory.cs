#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserManagement.Infrastructure.Identity.Contexts;

public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
{
    public IdentityContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();

        var connectionString = config.GetConnectionString("IdentityConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new IdentityContext(optionsBuilder.Options);
    }
}
