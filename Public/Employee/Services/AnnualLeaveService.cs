using System;
using portal.Db;
using portal.Enums;
using portal.Models;

public class AnnualLeaveService : IAnnualLeaveService
{
    private readonly ApplicationDbContext _dbContext;

    public AnnualLeaveService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AccrueMonthlyLeaveAsync()
    {
        var companyInfos = _dbContext
            .CompanyInfos.Where(c => c.EmploymentStatus == EmploymentStatus.ACTIVE)
            .ToList();

        foreach (var companyInfo in companyInfos)
        {
            companyInfo.AnnualLeaveTotalDays += 1;

            _dbContext.AnnualLeaveTransactions.Add(
                new AnnualLeaveTransaction
                {
                    EmployeeId = companyInfo.EmployeeId,
                    Type = LeaveTransactionType.ACCRUAL,
                    Amount = 1,
                    Notes =
                        $"+1 ngày phép cho nhân viên {companyInfo.EmployeeId} tháng {DateTime.UtcNow:MMMM yyyy}",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Hệ thống Hangfire tự động."
                }
            );
        }

        await _dbContext.SaveChangesAsync();
    }

    // public async Task ResetLeaveBalancesAsync()
    // {
    //     var employees = await _dbContext.CompanyInfos.ToListAsync();

    //     foreach (var companyInfo in employees)
    //     {
    //         var oldBalance = companyInfo.AnnualLeaveTotalDays;
    //         companyInfo.AnnualLeaveTotalDays = 0;

    //         _dbContext.AnnualLeaveTransactions.Add(
    //             new AnnualLeaveTransaction
    //             {
    //                 EmployeeId = companyInfo.EmployeeId,
    //                 Type = LeaveTransactionType.RESET,
    //                 Amount = -1 * (decimal)oldBalance,
    //                 Notes = "Reset leave on April 30",
    //                 CreatedAt = DateTime.UtcNow,
    //                 CreatedBy = _currentUser.UserId
    //             }
    //         );
    //     }

    //     await _dbContext.SaveChangesAsync();
    // }

    // public async Task<List<LeaveTransactionDTO>> GetAccrualHistoryAsync(int employeeId)
    // {
    //     var transactions = await _dbContext
    //         .AnnualLeaveTransactions.Where(t => t.EmployeeId == employeeId)
    //         .OrderByDescending(t => t.CreatedAt)
    //         .ToListAsync();

    //     return transactions
    //         .Select(t => new LeaveTransactionDTO
    //         {
    //             Id = t.Id,
    //             EmployeeId = t.EmployeeId,
    //             Type = t.Type,
    //             Amount = t.Amount,
    //             Date = t.CreatedAt,
    //             Notes = t.Notes ?? "",
    //             EmployeeName = "" // Optional: Load name separately if needed
    //         })
    //         .ToList();
    // }

    // public async Task RecalculateLeaveAsync(int employeeId)
    // {
    //     var companyInfo = await _dbContext.CompanyInfos.FirstOrDefaultAsync(c =>
    //         c.EmployeeId == employeeId
    //     );

    //     if (companyInfo == null)
    //         return;

    //     var accruals = await _dbContext
    //         .AnnualLeaveTransactions.Where(t => t.EmployeeId == employeeId)
    //         .SumAsync(t => t.Amount);

    //     companyInfo.AnnualLeaveTotalDays = (double)accruals;

    //     await _dbContext.SaveChangesAsync();
    // }
}
