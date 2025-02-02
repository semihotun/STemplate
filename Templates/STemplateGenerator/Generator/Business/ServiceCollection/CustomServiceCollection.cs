﻿using Generator.Business.Classes;
using Generator.Business.Dtos;
using Generator.Business.Mapper;
using Generator.Business.MediatR.Create;
using Generator.Business.MediatR.Delete;
using Generator.Business.MediatR.GetById;
using Generator.Business.MediatR.Grid;
using Generator.Business.MediatR.Template;
using Generator.Business.MediatR.Update;
using Generator.Business.VsStore;
namespace Generator.Business.ServiceCollection;

/// <summary>
/// Temporary
/// </summary>
internal static class CustomServiceCollection
{
    public static IMediatRCreateGridManager MediatRCreateGridManager() => new MediatRCreateGridManager();
    public static IDtoCreatorManager DtoManager() => new DtoCreatorManager();
    public static IClassesMethodManager ClassesMethodManager() => new ClassesMethodManager();
    public static IMediatRCreateGetByIdMethodManager MediatRCreateGetByIdMethodManager() => new MediatRCreateGetByIdMethodManager();
    public static IMediatRCreateDeleteMethodManager MediatRCreateDeleteMethodManager() => new MediatRCreateDeleteMethodManager();
    public static IMediatRCreateAddMethodManager MediatRCreateAddMethodManager() => new MediatRCreateAddMethodManager();
    public static ICreateAddMapperlyMapperManager CreateAddMapperlyMapperManager() => new CreateAddMapperlyMapperManager();
    public static IVsWritableSettingsStoreManager VsWritableSettingsStoreManager() => new VsWritableSettingsStoreManager();
    public static IMediatrTemplate MediatrTemplate() => new MediatrTemplate();
    public static IMediatRCreateUpdateMethodManager MediatRCreateUpdateMethodManager() => new MediatRCreateUpdateMethodManager();
    public static IMediatRCreateGetAllListMethodManager MediatRCreateGetAllListMethodManager() => new MediatRCreateGetAllListMethodManager();
}