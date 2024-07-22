using Generator.Models;

namespace Generator.Business.MediatR.GetById;
internal interface IMediatRCreateGetAllListMethodManager
{
    Task CreateGetAllListMethodRequestAsync(CreateAggregateClassRequest request);
}