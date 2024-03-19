namespace SearchBarService.Insfrastructure.Utilities.HangFire
{
    public interface IHangFireJob
    {
        Task Process();
    }
}
