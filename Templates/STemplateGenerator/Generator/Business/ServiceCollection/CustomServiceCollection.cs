using Generator.Business.Mapper;
using Generator.Business.VsStore;
namespace Generator.Business.ServiceCollection;

/// <summary>
/// Temporary
/// </summary>
internal static class CustomServiceCollection
{
    public static ICreateAddMapperlyMapperManager CreateAddMapperlyMapperManager() => new CreateAddMapperlyMapperManager();
    public static IVsWritableSettingsStoreManager VsWritableSettingsStoreManager() => new VsWritableSettingsStoreManager();
}