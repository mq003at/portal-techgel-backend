namespace portal.Db;

using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using portal.Config.Models;
using portal.Models;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly AppDbContextSaveChangesInterceptor _saveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AppDbContextSaveChangesInterceptor saveChangesInterceptor
    )
        : base(options)
    {
        _saveChangesInterceptor = saveChangesInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations dynamically
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_saveChangesInterceptor); // Register SaveChangesInterceptor
    }

    public override int SaveChanges()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        return base.SaveChanges();
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Division> Divisions { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<Section> Sections { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<EmployeeDivision> EmployeeDivisions { get; set; }
    public DbSet<EmployeeDepartment> EmployeeDepartment { get; set; }
    public DbSet<EmployeeTeam> EmployeeTeam { get; set; }
    public DbSet<EmployeeUnit> EmployeeUnit { get; set; }
    public DbSet<EmployeeSection> EmployeeSection { get; set; }
}
