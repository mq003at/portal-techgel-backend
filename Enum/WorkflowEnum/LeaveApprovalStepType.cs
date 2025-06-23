public enum LeaveApprovalStepType
{
    CreateForm = 0,
    ExecutiveApproval = 1,
}

public enum LeaveApprovalCategory
{
    // Nghỉ phép
    // 0: Nghỉ phép hàng năm
    // 1: Nghỉ ốm
    // 2: Nghỉ thai sản
    // 3: Nghỉ tang / cưới
    // 4: Nghỉ không lương
    // 5: Nghỉ bù
    AnnualLeave = 0,
    SickLeave = 1,
    MaternityLeave = 2,
    PaternityLeave = 3,
    UnpaidLeave = 4,
    CompensatoryLeave = 5,
}

public enum DayNightEnum
{
    Day = 0,
    Night = 1,
}
