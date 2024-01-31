using Generator.Models;
namespace Generator.Business.MediatR.Create;
internal interface IMediatRCreateAddMethodManager
{
    Task CreateAddMethodRequestAsync(CreateAggregateClassRequest request);
}
