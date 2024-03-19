namespace ShippingService.Insfrastructure.Utilities.HangFire
{
    public interface IHangFireJob
    {
        Task Process();
    }
}
