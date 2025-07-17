namespace portal.Db;

using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using portal.Models;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly AppDbContextSaveChangesInterceptor _saveChangesInterceptor;
    private readonly ILogger<ApplicationDbContext> _logger;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AppDbContextSaveChangesInterceptor saveChangesInterceptor,
        ILogger<ApplicationDbContext> logger
    )
        : base(options)
    {
        _saveChangesInterceptor = saveChangesInterceptor;
        _logger = logger;
        _logger.LogError("ApplicationDbContext CREATED");
        // File.AppendAllText(
        //     "notification-debug.txt",
        //     $"[{DateTime.UtcNow}] ApplicationDbContext CREATED\n"
        // );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations dynamically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseModelConfiguration<>).Assembly);
    }

    // CONVERT ENUM TO STRING -> CHANGE ALL INT ENUMS BEFOREHAND OR HASCONVERSION() THEM
    // protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    // {
    //     configurationBuilder.Properties<Enum>().HaveConversion<string>();
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_saveChangesInterceptor); // Register SaveChangesInterceptor
        //optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();
    }

    public override int SaveChanges()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DebugCheck("SaveChangesAsync");
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void DebugCheck(string source)
    {
        var notifications = ChangeTracker
            .Entries<Notification>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .ToList();

        if (notifications.Any())
        {
            // File.AppendAllText(
            //     "notification-debug.txt",
            //     $"[{DateTime.UtcNow}] 🚨 {source} triggered Notification INSERT:\n"
            //         + string.Join(
            //             "\n",
            //             notifications.Select(n =>
            //                 $"- Title: {n.Title}, EmpId: {n.EmployeeId}, Category: {n.NotificationCategoryId}"
            //             )
            //         )
            //         + "\n\n"
            // );
            _logger.LogError(
                "🚨 {Source} triggered Notification INSERT: {Notifications}",
                source,
                string.Join(
                    ", ",
                    notifications.Select(n =>
                        $"Title: {n.Title}, EmpId: {n.EmployeeId}, Category: {n.NotificationCategoryId}"
                    )
                )
            );
        }
    }

    public DbSet<OrganizationEntity> OrganizationEntities { get; set; }

    // Employee Setup
    public DbSet<Employee> Employees { get; set; }
    public DbSet<PersonalInfo> PersonalInfos { get; set; }
    public DbSet<EmployeeQualificationInfo> EmployeeQualificationInfos { get; set; }
    public DbSet<CompanyInfo> CompanyInfos { get; set; }
    public DbSet<CareerPathInfo> CareerPathInfos { get; set; }
    public DbSet<TaxInfo> TaxInfos { get; set; }
    public DbSet<InsuranceInfo> InsuranceInfos { get; set; }
    public DbSet<EmergencyContactInfo> EmergencyContactInfos { get; set; }
    public DbSet<ScheduleInfo> ScheduleInfos { get; set; }
    public DbSet<Signature> Signatures { get; set; }
    public DbSet<AnnualLeaveTransaction> AnnualLeaveTransactions { get; set; }

    // Ending Employee Setup
    // Join table Emp - OrganizationEntity
    public DbSet<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentSignature> DocumentSignatures { get; set; }

    public DbSet<DocumentAssociation> DocumentAssociations { get; set; }
    public DbSet<WorkflowNodeParticipant> WorkflowNodeParticipants { get; set; }

    // Workflow setip
    public DbSet<LeaveRequestWorkflow> LeaveRequestWorkflows { get; set; }
    public DbSet<LeaveRequestNode> LeaveRequestNodes { get; set; }

    public DbSet<GatePassWorkflow> GatePassWorkflows { get; set; }
    public DbSet<GatePassNode> GatePassNodes { get; set; }

    public DbSet<GeneralProposalWorkflow> GeneralProposalWorkflows { get; set; }
    public DbSet<GeneralProposalNode> GeneralProposalNodes { get; set; }

    // Notification Setup
    public DbSet<NotificationCategory> NotificationCategories { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<OnlyForOrganizationEntity> OnlyForOrganizationEntities { get; set; }

    // Survey & Feedback Setup
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
    public DbSet<SurveyResponse> SurveyResponses { get; set; }

    // Inventory Management Setup
    public DbSet<Material> Materials { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<WarehouseLocation> WarehouseLocations { get; set; }
    public DbSet<StockLocation> StockLocations { get; set; }

    // public DbSet<GoodsReceiptNote> GoodsReceiptNotes { get; set; }
    // public DbSet<GoodsReceiptItem> GoodsReceiptItems { get; set; }
    // public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    // public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
}
