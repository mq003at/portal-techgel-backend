public static class WorkflowResolver
{
    private static readonly Dictionary<string, string> _displayNames =
        new()
        {
            { "GeneralProposal", "Tờ trình" },
            { "LeaveRequest", "Đơn nghỉ" },
            { "GatePass", "Giấy ra vào cổng" },
        };

    public static string GetDisplayName(Type workflowType)
    {
        var name = workflowType.Name;
        return _displayNames.TryGetValue(name, out var vn) ? vn : name;
    }

    public static string GetDisplayName(string workflowTypeName)
    {
        return _displayNames.TryGetValue(workflowTypeName, out var vn) ? vn : workflowTypeName;
    }
}
