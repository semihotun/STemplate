namespace Generator.Business.VsStore.RequestModel;

internal record SetMySettingRequestModel(string CollectionPath, string SettingName, int Value);