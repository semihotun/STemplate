namespace NotificationService.Insfrastructure.Utilities.Caching.Redis
{
    /// <summary>
    /// redis configure setting
    /// </summary>
    public class RedisSetting
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required string? User { get; set; }
        public required string Password { get; set; }
        public required bool Ssl { get; set; }
    }
}
