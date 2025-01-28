using Api.Repositories;
using ASG.Api2.Results;
using AutoMapper;
using Data;
using Dto.Subscription;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class SubscriptionService
    {
        private readonly DataContext _db;
        private readonly ISubscriptionRepository _userSubscriptionRepo;
        private readonly IMapper _mapper;

        public SubscriptionService(DataContext db, ISender sender, IMapper mapper, ISubscriptionRepository userSubscriptionRepo)
        {
            _db = db;
            //_userSubscriptionRepo = new UserSubscriptionRepo(db);
            _mapper = mapper;
            _userSubscriptionRepo = userSubscriptionRepo;
        }

        public async Task<List<SubscriptionResponse>> GetAllAsync()
        {
            var items = await _userSubscriptionRepo.GetAllAsync(setInclude: s => s.Include(u => u.Plan));
            List<SubscriptionResponse> itemsResponse = _mapper.Map<List<SubscriptionResponse>>(items);
            return itemsResponse;
        }

        public async Task<SubscriptionResponse> GetByIdAsync(string id)
        {
            var item = await _userSubscriptionRepo.GetByAsync(p => p.Id == id, false, s => s.Include(us => us.Plan));

            var result = _mapper.Map<SubscriptionResponse>(item);
            return result;
        }

        public async Task<SubscriptionResponse> GetByCustomerAsync(string customerId)
        {
            var item = await _userSubscriptionRepo.GetByAsync(p => p.CustomerId == customerId);

            var result = _mapper.Map<SubscriptionResponse>(item);
            return result;
        }

        public async Task<SubscriptionResponse> CreateAsync(Subscription subscription)
        {
            var item = await _userSubscriptionRepo.CreateAsync(subscription);

            var result = _mapper.Map<SubscriptionResponse>(item);
            return result;
        }

        public async Task<SubscriptionResponse> UpdateAsync(Subscription subscription)
        {
            var item = await _userSubscriptionRepo.UpdateAsync(subscription);

            var result = _mapper.Map<SubscriptionResponse>(item);
            return result;
        }


        public async Task<Result> DeleteAsync(string id)
        {
            var userSubscription = await _userSubscriptionRepo.GetByAsync(p => p.Id == id);
            if (userSubscription == null)
            {
                var message = $"UserSubscription with id {id} not exists";
                return Result.Failure(Error.RecordNotFound(message));
            }
            await _userSubscriptionRepo.RemoveAsync(userSubscription);
            return Result.Ok();
        }
    }
}
