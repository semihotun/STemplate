using Generator.Business.VsStore;
namespace Generator.Business.ServiceCollection
{
    /// <summary>
    /// Temporary
    /// </summary>
    internal static class CustomServiceCollection
    {
        public static IVsWritableSettingsStoreManager VsWritableSettingsStoreManager() => new VsWritableSettingsStoreManager();
    }
}