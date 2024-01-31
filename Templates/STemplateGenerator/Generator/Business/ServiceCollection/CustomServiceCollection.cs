using Generator.Business.Mapper;
using Generator.Business.MediatR.Create;
using Generator.Business.MediatR.Template;
using Generator.Business.VsStore;
namespace Generator.Business.ServiceCollection;

/// <summary>
/// Temporary
/// </summary>
internal static class CustomServiceCollection
{
    public static IMediatRCreateAddMethodManager MediatRCreateAddMethodManager() => new MediatRCreateAddMethodManager();
    public static ICreateAddMapperlyMapperManager CreateAddMapperlyMapperManager() => new CreateAddMapperlyMapperManager();
    public static IVsWritableSettingsStoreManager VsWritableSettingsStoreManager() => new VsWritableSettingsStoreManager();
    public static IMediatrTemplate MediatrTemplate() => new MediatrTemplate();
}