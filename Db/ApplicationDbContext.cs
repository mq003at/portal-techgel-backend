namespace portal.Db;

using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseModelConfiguration<>).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_saveChangesInterceptor); // Register SaveChangesInterceptor
    }

    public override int SaveChanges()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        return base.SaveChanges();
    }

    public DbSet<OrganizationEntity> OrganizationEntities { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; }
    public DbSet<Signature> Signatures { get; set; }
    public DbSet<Document> Documents { get; set; }

    public DbSet<LeaveRequestWorkflow> LeaveRequestWorkflows { get; set; }
    public DbSet<LeaveRequestNode> LeaveRequestNodes { get; set; }

    // public DbSet<GeneralWorkflow> GeneralWorkflows { get; set; }
    // public DbSet<ApprovalWorkflowNode> ApprovalWorkflowNodes { get; set; }
}
