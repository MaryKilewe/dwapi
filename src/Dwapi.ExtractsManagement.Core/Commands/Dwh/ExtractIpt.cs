using Dwapi.SharedKernel.Model;
using MediatR;

namespace Dwapi.ExtractsManagement.Core.Commands.Dwh
{
    public class ExtractIpt: IRequest<bool>{public DbExtract Extract { get; set; }public DbProtocol DatabaseProtocol { get; set; }}
}