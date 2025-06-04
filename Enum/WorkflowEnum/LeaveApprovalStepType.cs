public enum LeaveApprovalStepType
{
    CreateForm = 0,
    ManagerApproval = 1,
    HRHeadApproval = 2,
    ExecutiveApproval = 3,
}

public enum LeaveAprrovalCategory
{
    // Nghỉ phép
    // 0: Nghỉ phép hàng năm
    // 1: Nghỉ ốm
    // 2: Nghỉ thai sản
    // 3: Nghỉ tang / cưới
    // 4: Nghỉ không lương
    // 5: Nghỉ khác
    AnnualLeave = 0,
    SickLeave = 1,
    MaternityLeave = 2,
    PaternityLeave = 3,
    UnpaidLeave = 4,
    Other = 5,
}

public enum DayNightEnum
{
    Day = 0,
    Night = 1,
}
