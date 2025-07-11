using portal.Services;

public class LeaveAccrualJob
{
    private readonly IAnnualLeaveService _annualLeaveService;

    public LeaveAccrualJob(IAnnualLeaveService annualLeaveService)
    {
        _annualLeaveService = annualLeaveService;
    }

    public async Task Execute()
    {
        await _annualLeaveService.AccrueMonthlyLeaveAsync();
    }
}
