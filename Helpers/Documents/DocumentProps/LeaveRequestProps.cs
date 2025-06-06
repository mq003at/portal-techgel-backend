namespace portal.Documents.Props;

public record LeaveRequestProps(
    string TemplatePath,
    // --- Top Section ---
    string EmployeeName,
    DateTime LeaveRequestStartHour,
    string Department,
    DateTime LeaveRequestEndHour,
    string Position,
    string Reason,
    string LeaveApprovalCategory,

    // --- Assignee Section ---
    string AssigneeName,
    string AssigneePhoneNumber,
    string AssigneeEmail,
    string AssigneeAddress,

    // --- Leave Stats ---
    float EmployeeAnnualLeaveTotalDays,
    float FinalEmployeeAnnualLeaveTotalDays,
    float TotalDays,

    // --- Sign Dates ---
    DateTime? EmployeeSignDate,
    DateTime? SupervisorSignDate,
    DateTime? HrSignDate,
    DateTime? GeneralDirectorSignDate,

    // --- Signatures ---
    string? EmployeeSignature,
    string? GeneralDirectorSignature,
    string? HrSignature,
    string? SupervisorSignature,

    // --- Names (Signers) ---
    string HrName,
    string SupervisorName,
    string GeneralDirectorName
);