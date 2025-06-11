namespace portal.Enums;

public enum GeneralWorkflowStatusType
{
    Pending,
    Approved,
    Onwaiting,
    Rejected,
    Cancelled,
    Expired,
    Draft
}

public enum GeneralWorkflowLogicType
{
    Sequential,
    Parallel,
    Majority,
    Quorum,
    AbsoluteMajority
}

public enum WorkflowParticipantRoleType
{
    Responsible,
    Accountable,
    Consulted,
    Informed,
    QualityAssurance
}