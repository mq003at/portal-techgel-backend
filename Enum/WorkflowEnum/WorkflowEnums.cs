namespace portal.Enums;

public enum GeneralWorkflowStatusType
{
    Pending,
    Approved,
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
