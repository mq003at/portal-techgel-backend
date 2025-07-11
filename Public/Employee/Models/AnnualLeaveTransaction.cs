namespace portal.Models;

using portal.Enums;

public class AnnualLeaveTransaction : BaseLog
{
    public int EmployeeId { get; set; }
    public LeaveTransactionType Type { get; set; } // Accrual, Reset, ManualAdjustment, Usage
    public decimal Amount { get; set; } // +1, -3, etc.
    public string? Notes { get; set; }
}
