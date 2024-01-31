using Generator.Business.VsStore.RequestModel;
using Generator.Models;

namespace Generator.Business.VsStore;

internal interface IVsWritableSettingsStoreManager
{
    public VsWritableSetting GetMySetting(GetMySettingRequestModel request);
    public void SetMySetting(SetMySettingRequestModel request);
}