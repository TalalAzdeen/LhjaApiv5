using Dto.ModelAi;
using Entities;

namespace Api.ServiceLayer.LayerModel
{
    public interface IModelService
    {
        IEnumerable<ModelAi> GetAllModels();

        IEnumerable<ModelAi> GetModelByType(string Type);
        IEnumerable<ModelAi> GetModelByLanguage(string Language);
        IEnumerable<ModelAi> GetModelByCategory(string Category);
        IEnumerable<ModelAi> GetModelByGender(string Gender);
        IEnumerable<ModelAi> GetModelByIsStandard(string IsStandard);
        IEnumerable<ModelAi> GetModelByIsDialect(string Dialect);
        Task<IEnumerable<ModelAi>>FilterMaodelAi(FilterModelAI serchModel);
        IEnumerable<ModelAi> GetModelByIsLanguageAndDialectAndType(string Language, string Dialect,string Type);
        IEnumerable<ModelAi> GetModelByIsLanguageAndDialect(string Language ,string Dialect);

        Task<IEnumerable<ModelAi>>GetModelSender(FilterModelAI serchModel);
        IEnumerable<ModelAi> GetModelByIsTypeAndGender(string Language, string Gender);

        Task<List<string>>GetAllLangage(string tab);
        Task<List<string>> GetAllCategory(string tab);
        Task<List<string>> GetAllDialect(string tab);




    }
}
