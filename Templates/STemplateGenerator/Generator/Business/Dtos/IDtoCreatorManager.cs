using Generator.Business.MediatR.Grid.RequestModel;

namespace Generator.Business.Dtos;
internal interface IDtoCreatorManager
{
    Task CreateDtoAsync(CreateDtoRequest request);
}