namespace portal.Enums;

public enum LeaveTransactionType
{
    ACCRUAL = 0, // Monthly accrual of leave
    RESET = 1, // Reset leave balance at the start of a new year
    MANUAL_ADJUSTMENT = 2, // Manual adjustments made by HR or admin
    USAGE = 3 // Leave taken by the employee
}
