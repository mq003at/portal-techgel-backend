namespace portal.Documents.Props;

public record LeaveRequestProps(
    string templatePath,
    DateTime DraftDate,
    string EmployeeName,
    string Position,
    string Department,
    DateTime EmploymentStartDate,
    float AnnualLeaveDaysPerYear,
    float FinalEmployeeAnnualLeaveTotalDays,
    string HrName,
    string WorkAssignedIdCardNumber,
    string WorkAssignedIdCardIssuedLocation,
    DateTime IdCardIssuedDate,
    string PhoneNumber,
    string Reason,
    string SupervisorName,
    string SupervisorPosition,
    DateTime WorkAssignedToDateOfBirth,
    string WorkAssignedToName,
    string GeneralDirectorName,
    string? GeneralDirectorSignature,
    string? EmployeeSignature,
    string? HrSignature,
    string? SupervisorSignature
);
