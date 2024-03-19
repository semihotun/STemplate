namespace PaymentService.Insfrastructure.Utilities.HangFire
{
    public interface IHangFireJob
    {
        Task Process();
    }
}
