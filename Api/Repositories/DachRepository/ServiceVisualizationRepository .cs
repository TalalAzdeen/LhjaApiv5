using AutoMapper;
using Data;
using Dto.DachBoard;
using Entities;

namespace Api.Repositories.DachRepository
{


    public interface IServiceVisualizationRepository : IBaseRepository<Request>
    {
        List<RequestData> GetRequestsByTime();
        List<ServiceDataTod> GetErrorsByTime();
        IEnumerable<RequestData> FilterServiceData(string serviceType);
    }
    public class ServiceVisualizationRepository : BaseRepository<Request>, IServiceVisualizationRepository
    {
        private readonly IMapper _mapper;
        public ServiceVisualizationRepository(DataContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }



        public IEnumerable<RequestData> FilterServiceData(string serviceType)
        {
             var _serviceData = GetRequestsByTime();
            if (serviceType == "ALL") return _serviceData;
            return _serviceData.Where(d => d.ServiceType == serviceType);
        }
        public List<ServiceDataTod> GetErrorsByTime()
        {
            return new List<ServiceDataTod>
        {
            new ServiceDataTod { Value = 100, TypeData = "requests", ServiceType = "Text to Speech" },
            new ServiceDataTod { Value = 50, TypeData = "errors", ServiceType = "Text to Speech" },
            new ServiceDataTod { Value = 100, TypeData = "requests", ServiceType = "Text to Dialect" },
            new ServiceDataTod { Value = 50, TypeData = "errors", ServiceType = "Text to Dialect" },
            new ServiceDataTod { Value = 100, TypeData = "requests", ServiceType = "Speech to Speech" },
            new ServiceDataTod { Value = 50, TypeData = "errors", ServiceType = "Speech to Speech" }
        };
        }
        public List<RequestData> GetRequestsByTime()
        {
            
                return new List<RequestData>
        {
            new RequestData { Time = new DateTime(2025, 1, 1, 0, 0, 0), Requests = 15, Errors = 1, ServiceType = "Text to Speech" },
            new RequestData { Time = new DateTime(2025, 1, 1, 1, 0, 0), Requests = 12, Errors = 0, ServiceType = "Text to Dialect" },
            new RequestData { Time = new DateTime(2025, 1, 1, 2, 0, 0), Requests = 18, Errors = 2, ServiceType = "Speech to Speech" },
            new RequestData { Time = new DateTime(2025, 1, 1, 3, 0, 0), Requests = 10, Errors = 1, ServiceType = "Text to Speech" },
            new RequestData { Time = new DateTime(2025, 1, 1, 4, 0, 0), Requests = 14, Errors = 0, ServiceType = "Text to Dialect" }
        };

               
            
        }
    }
}
