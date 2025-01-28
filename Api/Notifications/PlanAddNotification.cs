using Entities;
using MediatR;

namespace Api.Notifications
{
    public record PlanAddedNotification(Plan Plan) : INotification;

}
