using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Cepres.Patients.EntityFrameworkCore
{
    public static class PatientsDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<PatientsDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<PatientsDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
