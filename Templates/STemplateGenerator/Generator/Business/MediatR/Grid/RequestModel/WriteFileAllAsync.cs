using Generator.Models;
using System.Collections.Generic;

namespace Generator.Business.MediatR.Grid.RequestModel;
internal record WriteFileAllAsync(CreateAggregateClassRequest CreateAggregateClassRequest,
        GridGenerateVeriables GridGenerateVeriables,
        string DtoFilePath,
        string DtosFolderPath,
        List<PropertyInfoByClass> PropertyInfoList);