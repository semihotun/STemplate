using Generator.Models;
namespace Generator.Business.MediatR.Delete;

internal interface IMediatRCreateDeleteMethodManager
{
    Task CreateDeleteMethodRequestAsync(CreateAggregateClassRequest request);
}