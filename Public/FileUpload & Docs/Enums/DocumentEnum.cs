namespace portal.Enums;


public enum DocumentApprovalLogicEnum
{
    SEQUENTIAL, // Each node must be approved in order
    PARALLEL,   // All nodes must approve simultaneously
    MAJORITY,   // Majority of nodes must approve
    QUORUM,     // A minimum number of nodes must approve
    ABSOLUTE_MAJORITY // More than half of the total nodes must approve
}
public enum DocumentTypeEnum
{
    DICISION,
    NOTIFICATION,
    TEMPLATE,
    CONTRACT,
    OTHER
}

public enum DocumentStatusEnum
{
    DRAFT,
    PUBLISHED,
    ARCHIVED,
    PENDING_APPROVAL,
    REJECTED,
    CANCELLED,
    EXPIRED,
    APPROVED,
    UNKNOWN
}

public enum DocumentCategoryEnum
{
    RESOLUTION,
    NOTIFICATION,
    TEMPLATE,
    LETTER,
    MEMO,
    AGREEMENT,
    POLICY,
    PROCEDURE,
    INSTRUCTION,
    MANUAL,
    FORM,
    REPORT,
    PROPOSAL,
    REQUEST,
    APPLICATION,
    
    DECISION,
    DIRECTIVE,
    RULES,
    NOTICE,
    GUIDELINE,
    PLAN,
    PROJECT_PROPOSAL,
    SCHEME,
    MINUTES,
    SUBMISSION,
    CONTRACT,
    CORRESPONDANCE,
    MEMORANDUM,
    AUTHORIZATION_DOCUMENT,
    INVITATION_DOCUMENT,
    INTRODUCTION_DOCUMENT,
    DISPATCH_SLIP,
    TRANSFER_SLIP,
    NOTICE_SLIP,
    OFFICIAL_LETTER,
    PROGRAM,
    PROJECT,
    REGULATION,
    OTHER,
    
}



/// <summary>
/// Confidentiality levels for document access control.
/// </summary>
public enum DocumentConfidentialityLevelEnum
{
    PUBLIC,
    INTERNAL,
    SECRET,
    SELF
}

public static class DocumentAssociationTypes
{
    public const string Workflow = "Workflow";
    public const string WorkflowNode = "WorkflowNode";
    public const string LeaveRequest = "LeaveRequest";
}