using Api.Commands;
using Api.Repositories;
using MediatR;

namespace ASG.Api2.Handlers
{
    public class AddPlanHandler(IPlanRepository planRepository) : IRequestHandler<AddPlanCommand>
    {
        public async Task Handle(AddPlanCommand request, CancellationToken cancellationToken) => await planRepository.CreateAsync(request.Plan);
    }
}
