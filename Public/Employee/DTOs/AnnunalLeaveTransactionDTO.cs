using portal.DTOs;
using portal.Enums;

public class LeaveTransactionDTO : BaseModelDTO
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public LeaveTransactionType Type { get; set; }

    public decimal Amount { get; set; }

    public string? Notes { get; set; }
}
