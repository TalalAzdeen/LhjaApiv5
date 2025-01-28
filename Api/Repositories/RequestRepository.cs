using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public interface IRequestRepository : IBaseRepository<Request>
{
    Task<int> GetCount(string userId, string serviceId, string status = "Succeed");
}
public class RequestRepository(DataContext db) : BaseRepository<Request>(db), IRequestRepository
{
    public async Task<int> GetCount(string userId, string serviceId, string status = "Succeed")
    {
        return await GetQuery().CountAsync(
            r => r.UserId == userId &&
            r.ServiceId == serviceId &&
            r.Status == status);
    }
}