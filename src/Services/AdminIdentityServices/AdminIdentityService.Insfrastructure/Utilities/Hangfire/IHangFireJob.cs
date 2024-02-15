namespace AdminIdentityService.Insfrastructure.Utilities.Hangfire;

public interface IHangFireJob
{
    Task Process();
}
