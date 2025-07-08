namespace portal.Enums;

public enum GeneralWorkflowStatusType
{
    PENDING,
    APPROVED,
    ONWAITING,
    REJECTED,
    CANCELLED,
    EXPIRED,
    DRAFT
}

// Thể thức phê duyệt trong 1 node / quy trình
// 1. SEQUENTIAL: Người dùng sẽ phê duyệt theo thứ tự đã định.
// 2. PARALLEL: Người dùng có thể phê duyệt đồng thời,
// 3. MAJORITY: Số lượng người phê duyệt phải đạt đa số.
// 4. QUORUM: Số lượng người phê duyệt phải đạt một
//            tỷ lệ nhất định (ví dụ: 50% + 1).
// 5. ABSOLUTE_MAJORITY: Số lượng người phê duyệt phải đạt đa số tuyệt đối (ví dụ: 2/3).

public enum WorkflowApprovalLogicType
{
    SEQUENTIAL,
    PARALLEL,
    MAJORITY,
    QUORUM,
    ABSOLUTE_MAJORITY,
    ANY_ONE
}

public enum WorkflowParticipantRoleType
{
    RESPONSIBLE,
    ACCOUNTABLE,
    CONSULTED,
    INFORMED,
    QUALITY_ASSURANCE
}

public enum WorkflowNodeApprovalLogic
{
    ANY_ONE = 0,
    EVERYONE = 1,
    MAJORITY = 2
}
