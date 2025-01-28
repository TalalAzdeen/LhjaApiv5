using AutoMapper;
using Data;
using Dto.DachBoard;
using Entities;

namespace Api.Repositories.DachRepository
{


    public interface IPlanVisualizationRepository : IBaseRepository<Request>
    {
        Dictionary<string, int> GetPlanData();
        List<ServiceUsageData> GetServiceUsageData();

    }

    public class PlanVisualizationRepository : BaseRepository<Request>, IPlanVisualizationRepository
    {
        private readonly IMapper _mapper;
        public PlanVisualizationRepository(DataContext db, IMapper mapper) : base(db)
        {
            _mapper=mapper;
        }

        public Dictionary<string, int> GetPlanData()
        {
            throw new NotImplementedException();
        }

        public List<ServiceUsageData> GetServiceUsageData()
        {
            return new List<ServiceUsageData>
    {

        new ServiceUsageData { ServiceType = "Text to Speech", UsageCount = 100 },
        new ServiceUsageData { ServiceType = "Text to Dialect", UsageCount = 200 },
        new ServiceUsageData { ServiceType = "Speech to Speech", UsageCount = 200 }
    };
        }
    }
}
