namespace OrderService.Insfrastructure.Utilities.HangFire
{
    public interface IHangFireJob
    {
        Task Process();
    }
}
