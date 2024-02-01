using Generator.Business.MediatR.Grid.RequestModel;
using System.Threading.Tasks;
namespace Generator.Business.MediatR.Grid;
internal interface IMediatRCreateGridManager
{
    Task<bool> GenerateCodeAsync(GridGenerateVeriables request);
}
