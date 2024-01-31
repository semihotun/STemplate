using Generator.Business.MediatR.Template.Models.Base;
using Generator.Business.MediatR.Template.Models.Command;
using Generator.Business.MediatR.Template.Models.Dto;
using Generator.Business.MediatR.Template.Models.Query;
namespace Generator.Business.MediatR.Template;
internal interface IMediatrTemplate
{
    string GetRequestString(GetRequestModel request);
    string GetRequestHandlerString(GetRequestModel request);
    bool IsAggregateUsing(IsAggregateUsingRequestModel isAggregateUsingRequestModel);
    #region Command
    string GetCommandHandlerPrimaryConstructorParameters(GetCommandHandlerPrimaryConstructorParametersRequestModel request);
    string GetCommandRequestHandlerUsing(GetCommandRequestHandlerUsingRequestModel request);
    string GetCommandRequestUsing(GetCommandRequestUsingRequestModel request);
    string GetCommandConstructorString(GetCommandConstructorStringRequestModel request);
    #endregion
    #region Dto
    string GetDtoRequestHandlerUsing(GetDtoRequestHandlerUsing request);
    string GetDtoRequestUsing(GetDtoRequestUsingRequestModel request);
    string GetDtoConstructer();
    string GetDtoPrimaryConstructerParameters();
    #endregion
    #region Query
    string GetQueryHandlerPrimaryConstructorParameters(GetQueryHandlerPrimaryConstructorParametersRequestModel request);
    string GetQueryConstructorString(GetQueryConstructorStringRequestModel request);
    string GetQueryRequestUsing(GetQueryRequestUsingRequestModel request);
    string GetQueryRequestHandlerUsing(GetQueryRequestHandlerUsingRequestModel request);
    #endregion

}