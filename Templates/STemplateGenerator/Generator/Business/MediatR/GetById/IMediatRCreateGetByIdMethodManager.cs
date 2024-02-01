using Generator.Models;

namespace Generator.Business.MediatR.GetById;
internal interface IMediatRCreateGetByIdMethodManager
{
    Task CreateGetByIdMethodRequestAsync(CreateAggregateClassRequest request);
}