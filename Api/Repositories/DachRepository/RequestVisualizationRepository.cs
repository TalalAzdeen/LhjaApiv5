 using AutoMapper;
using Data;
using Dto.DachBoard;
using Entities;

namespace Api.Repositories.DachRepository
{


    public interface IRequestVisualizationRepository: IBaseRepository<Request>
    {
        Dictionary<string, int> GetServiceData();
        List<ServiceUsageData> GetServiceUsageData();
    }


    public class RequestVisualizationRepository : BaseRepository<Request>, IRequestVisualizationRepository
    {

        private readonly IMapper _mapper;
        public RequestVisualizationRepository(DataContext db,IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }

        public Dictionary<string, int> GetServiceData()
        {
            throw new NotImplementedException();
        }

    
        public List<ServiceUsageData> GetServiceUsageData()
        {
            var data = new List<ServiceUsageData>
    {
        new ServiceUsageData
        {
            ServiceType = "Used Requests",
            UsageCount = 300
        },
        new ServiceUsageData
        {
            ServiceType = "Remaining Requests",
            UsageCount = 150
        }
    };

            return data;
        }
    }
}
