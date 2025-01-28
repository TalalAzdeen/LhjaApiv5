using AutoMapper;
using CardShapping.Api.RepositoryAPI.SerchRepository;
using Dto.ModelAi;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.ServiceLayer.LayerModel
{



    public class ModelService : IModelService
    {

        private readonly ISearchRepository<ModelAi> _repository;
        protected readonly IMapper _mapperr;
        public ModelService(ISearchRepository<ModelAi> repository, IMapper mapper)
        {
            _mapperr = mapper;
            _repository = repository;
        }

      

        public     IEnumerable<ModelAi> GetAllModels()
        {
            return _repository.ReadAllAsyncSerchName(x => true, "").Result.ToList();
        }

      
        public IEnumerable<ModelAi> GetModelByCategory(string Category)
        {
            return _repository.ReadAllAsyncSerchName(x => x.Category == Category, nameof(ModelAi.Category)).Result.ToList();
        }

       
        public IEnumerable<ModelAi> GetModelByGender(string Gender)
        {
            return _repository.ReadAllAsyncSerchName(x => x.Gender == Gender, nameof(ModelAi.Gender)).Result.ToList();
        }

        // البحث حسب المعرف (Id)
        public ModelAi? GetModelById(string id)
        {
            return _repository.SerchasyncId(x => x.Id == id, int.Parse(id)).Result.FirstOrDefault();
        }

        // البحث حسب اللهجة (Dialect)
        public IEnumerable<ModelAi> GetModelByIsDialect(string Dialect)
        {
            return _repository.ReadAllAsyncSerchName(x => x.Dialect == Dialect, nameof(ModelAi.Dialect)).Result.ToList();
        }

        // البحث حسب اللغة واللهجة
        public IEnumerable<ModelAi> GetModelByIsLanguageAndDialect(string Language, string Dialect)
        {
            return _repository.ReadAllAsyncSerchName(
                x => x.Language == Language && x.Dialect == Dialect,
                $"{nameof(ModelAi.Language)}, {nameof(ModelAi.Dialect)}").Result.ToList();
        }

        // البحث حسب اللغة، اللهجة، والنوع
        public IEnumerable<ModelAi> GetModelByIsLanguageAndDialectAndType(string Language, string Dialect, string Type)
        {
            return _repository.ReadAllAsyncSerchName(
                x => x.Language == Language && x.Dialect == Dialect && x.Type == Type,
                $"{nameof(ModelAi.Language)}, {nameof(ModelAi.Dialect)}, {nameof(ModelAi.Type)}").Result.ToList();
        }

        // البحث حسب المعيارية (IsStandard)
        public IEnumerable<ModelAi> GetModelByIsStandard(string IsStandard)
        {
            bool isStandardValue = bool.Parse(IsStandard);
            return _repository.ReadAllAsyncSerchName(x => x.IsStandard == isStandardValue, nameof(ModelAi.IsStandard)).Result.ToList();
        }

   
        public IEnumerable<ModelAi> GetModelByIsTypeAndGender(string Type, string Gender)
        {
            return _repository.ReadAllAsyncSerchName(
                x => x.Type == Type && x.Gender == Gender,
                $"{nameof(ModelAi.Type)}, {nameof(ModelAi.Gender)}").Result.ToList();
        }

        // البحث حسب اللغة
        public IEnumerable<ModelAi> GetModelByLanguage(string Language)
        {
            return _repository.ReadAllAsyncSerchName(x => x.Language == Language, nameof(ModelAi.Language)).Result.ToList();
        }

        // البحث حسب النوع (Type)


       
        public IEnumerable<ModelAi> GetModelByType(string Type)
        {
            return _repository.ReadAllAsyncSerchName(x => x.Type == Type, nameof(ModelAi.Type)).Result.ToList();
        }

        public Task<List<string>> GetAllLangage(string tab)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllCategory(string tab)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllDialect(string tab)
        {
            throw new NotImplementedException();
        }

         

            public async Task<IEnumerable<ModelAi>> FilterMaodelAi(FilterModelAI searchModel)
            {
                if (searchModel == null)
                    throw new ArgumentNullException(nameof(searchModel));

                
                var query =await  _repository.GetAll();

              
                if (!string.IsNullOrWhiteSpace(searchModel.Type))
                {
                     query = query.Where(x => x.Type== searchModel.Type);
                }

                if (!string.IsNullOrWhiteSpace(searchModel.Name))
                {
                    query = query.Where(x => x.Name.Contains(searchModel.Name));
                }

                if ((!string.IsNullOrWhiteSpace(searchModel.Dialect)))
                {
                    query = query.Where(x => x.Dialect == searchModel.Dialect);
                }

            if ((!string.IsNullOrWhiteSpace(searchModel.Gender)))
                {
                query = query.Where(x => x.Gender == searchModel.Gender);
            }

            if ((!string.IsNullOrWhiteSpace(searchModel.Language)))
                {
                query = query.Where((x => x.Language != searchModel.Language));
            }
            if ((!string.IsNullOrWhiteSpace(searchModel.Category)))
            {
                query = query.Where(x => x.Category == searchModel.Category);
            
            }

            var results = await query.ToListAsync();

            
            return results;
            }

        public Task<IEnumerable<ModelAi>> GetModelSender(FilterModelAI serchModel)
        {
            throw new NotImplementedException();
        }
    }
}
