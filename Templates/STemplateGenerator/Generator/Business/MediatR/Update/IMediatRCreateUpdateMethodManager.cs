using Generator.Models;
namespace Generator.Business.MediatR.Update;
internal interface IMediatRCreateUpdateMethodManager
{
    Task CreateUpdateMethodRequestAsync(CreateAggregateClassRequest request);
}