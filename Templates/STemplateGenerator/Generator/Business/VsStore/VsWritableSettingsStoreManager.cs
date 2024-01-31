using Generator.Business.VsStore.RequestModel;
using Generator.Models;
using Microsoft.VisualStudio.Shell.Interop;

namespace Generator.Business.VsStore;

internal class VsWritableSettingsStoreManager : IVsWritableSettingsStoreManager
{
    private readonly IVsWritableSettingsStore _writableSettingsStore;
    /// <summary>
    /// Get writableSettingsStore Dont add ThreadHelper.ThrowIfNotOnUIThread();
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public VsWritableSettingsStoreManager()
    {
        if (ServiceProvider.GlobalProvider.GetService(typeof(SVsSettingsManager)) is IVsSettingsManager settingsManager)
        {
            settingsManager.GetWritableSettingsStore((uint)__VsSettingsScope.SettingsScope_UserSettings, out _writableSettingsStore);
        }
        else
        {
            throw new InvalidOperationException("IVsSettingsManager is not available.");
        }
    }
    /// <summary>
    /// Get Setting
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public VsWritableSetting GetMySetting(GetMySettingRequestModel request)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        _ = _writableSettingsStore.CollectionExists(request.CollectionPath, out var pfExists);
        if (pfExists == 1)
        {
            _writableSettingsStore.GetInt(request.CollectionPath, request.SettingName, out var mySettingValue);
            return new VsWritableSetting(mySettingValue);
        }
        return null;
    }
    /// <summary>
    /// Set My Setting
    /// </summary>
    /// <param name="request"></param>
    public void SetMySetting(SetMySettingRequestModel request)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_writableSettingsStore.CollectionExists(request.CollectionPath, out _) != 0)
        {
            _writableSettingsStore.CreateCollection(request.CollectionPath);
            _writableSettingsStore.SetInt(request.CollectionPath, request.SettingName, request.Value);
        }
        else
        {
            _writableSettingsStore.SetInt(request.CollectionPath, request.SettingName, request.Value);
        }
    }
}