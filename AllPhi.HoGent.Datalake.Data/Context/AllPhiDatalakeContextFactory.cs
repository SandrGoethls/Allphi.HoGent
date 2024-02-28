using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AllPhi.HoGent.Datalake.Data.Context
{
    public class AllPhiDatalakeContextFactory : IDesignTimeDbContextFactory<AllPhiDatalakeContext>
    {
        public AllPhiDatalakeContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json")
                                            .AddJsonFile($"appsettings.{env}.json", optional: true)
                                            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AllPhiDatalakeContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(env));

            return new AllPhiDatalakeContext(optionsBuilder.Options);
        }
    }
}
