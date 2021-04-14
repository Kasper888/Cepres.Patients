using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Cepres.Patients.Configuration;
using Cepres.Patients.Web;

namespace Cepres.Patients.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class PatientsDbContextFactory : IDesignTimeDbContextFactory<PatientsDbContext>
    {
        public PatientsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PatientsDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            PatientsDbContextConfigurer.Configure(builder, configuration.GetConnectionString(PatientsConsts.ConnectionStringName));

            return new PatientsDbContext(builder.Options);
        }
    }
}
