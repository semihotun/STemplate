namespace SearchBarService.Insfrastructure.Utilities.Outboxes
{
    public enum OutboxState
    {
        Started = 1,
        Pending = 2,
        Completed = 3,
        CanceledStarted = 4,
        CanceledPending = 5,
        CanceledCompleted = 6
    }
}
