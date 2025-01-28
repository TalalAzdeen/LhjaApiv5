using Entities;
using MediatR;

namespace Api.Commands
{
    public record AddPlanCommand(Plan Plan) : IRequest;
}
