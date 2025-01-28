using Api.Notifications;
using Api.Repositories;
using MediatR;

namespace ASG.Api2.Handlers
{
    public class CacheInvalidationHandler(IPlanRepository planRepository) : INotificationHandler<PlanAddedNotification>
    {
        public async Task Handle(PlanAddedNotification notification, CancellationToken cancellationToken)
        {
            planRepository.EventOccured(notification.Plan, 300);
            await Task.CompletedTask;
        }
    }
}
