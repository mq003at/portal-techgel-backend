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

public enum GeneralWorkflowLogicType
{
    SEQUENTIAL,
    PARALLEL,
    MAJORITY,
    QUORUM,
    ABSOLUTE_MAJORITY
}

public enum WorkflowParticipantRoleType
{
    RESPONSIBLE,
    ACCOUNTABLE,
    CONSULTED,
    INFORMED,
    QUALITY_ASSURANCE
}