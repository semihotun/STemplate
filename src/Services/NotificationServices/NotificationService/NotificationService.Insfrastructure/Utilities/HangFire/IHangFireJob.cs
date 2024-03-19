namespace NotificationService.Insfrastructure.Utilities.HangFire
{
    public interface IHangFireJob
    {
        Task Process();
    }
}
