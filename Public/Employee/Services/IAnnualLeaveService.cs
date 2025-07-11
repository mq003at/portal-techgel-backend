public interface IAnnualLeaveService
{
    Task AccrueMonthlyLeaveAsync();
    // Task ResetLeaveBalancesAsync();
    // Task<List<LeaveTransactionDTO>> GetAccrualHistoryAsync(int employeeId);
    // Task RecalculateLeaveAsync(int employeeId);
}
