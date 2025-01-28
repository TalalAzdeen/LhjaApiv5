using Azure.Core;
using MediatR;

namespace Api.Commands
{
    /// <summary>
    /// Update Number of Current Requests
    /// </summary>
    /// <param name="Id"></param>
    public record CreateRequestCommand(Request Request) : IRequest<long>;
}
