// Registering recurring job for leave accrual and reset
using Hangfire;

public static class LeaveRecurringJobRegistry
{
    public static void Register()
    {
        RecurringJob.AddOrUpdate<LeaveAccrualJob>(
            "monthly-leave-accrual",
            job => job.Execute(),
            "59 23 L * *" // last day of month, 11:59 PM
        );
    }
}
