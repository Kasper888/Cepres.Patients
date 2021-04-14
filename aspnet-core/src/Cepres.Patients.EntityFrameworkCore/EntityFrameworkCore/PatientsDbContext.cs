using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Cepres.Patients.Authorization.Roles;
using Cepres.Patients.Authorization.Users;
using Cepres.Patients.MultiTenancy;
using Cepres.Patients.Patients;

namespace Cepres.Patients.EntityFrameworkCore
{
  public class PatientsDbContext : AbpZeroDbContext<Tenant, Role, User, PatientsDbContext>
  {
    public DbSet<Visit> Visit { get; set; }
    public DbSet<Patient> Patient { get; set; }

    public PatientsDbContext(DbContextOptions<PatientsDbContext> options)
            : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Patient>().HasIndex(u => u.NationalId).IsUnique();
      base.OnModelCreating(builder);
    }
  }
}
