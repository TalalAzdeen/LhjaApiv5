//using ASG.Api2.Notifications;
//using ASG.Api2.Repositories;
//using MediatR;

//namespace ASG.Api2.Handlers
//{
//    public class EmailHandler (IPlanRepository planRepository) : INotificationHandler<PlanAddedNotification>
//    {
//        public async Task Handle(PlanAddedNotification notification, CancellationToken cancellationToken)
//        {
//            await planRepository.EventOccured(notification.Plan, 50);
//            await Task.CompletedTask;
//        }
//    }
//}
