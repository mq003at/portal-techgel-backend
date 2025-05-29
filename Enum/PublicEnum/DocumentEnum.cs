namespace portal.Enums;

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

public enum DocumentSubTypeEnum
{
    RESOLUTION,
    DECISION,
    DIRECTIVE,
    RULES,
    NOTICE,
    GUIDELINE,
    PLAN,
    PROJECT_PROPOSAL,
    SCHEME,
    REPORT,
    MINUTES,
    SUBMISSION,
    CONTRACT,
    CORRESPONDANCE,
    MEMORANDUM,
    AGREEMENT,
    AUTHORIZATION_DOCUMENT,
    INVITATION_DOCUMENT,
    INTRODUCTION_DOCUMENT,
    DISPATCH_SLIP,
    TRANSFER_SLIP,
    NOTICE_SLIP,
    OFFICIAL_LETTER,
    PROGRAM,
    PROJECT,
    POLICY,
    REGULATION,
    OTHER,
    NOTIFICATION
}

/// <summary>
/// Top-level categories for grouping documents.
/// </summary>
public enum DocumentCategoryEnum
{
    LEGAL,
    EMPLOYMENT,
    ACCOUNTING,
    INTERNAL,
    PROJECT,
    DESIGN,
    EQUIPMENT,
    GUIDELINE,
    CLIENT,
    PR,
    COPYRIGHT,
    ARCHIVE
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
