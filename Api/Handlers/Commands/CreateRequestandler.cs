using Api.Commands;
using Api.Repositories;
using MediatR;

namespace ASG.Api2.Handlers.Commands
{
    public class CreateRequestandler(IUserRepository userRepository) : IRequestHandler<CreateRequestCommand, long>
    {
        public async Task<long> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            //return await userRepository.UpdateNumberCurrentRequestAsync();
            return 45;
        }
    }
}
