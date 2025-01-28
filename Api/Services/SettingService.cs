using Api.Repositories;
using AutoMapper;
using Dto.Setting;
using Entities;
using FluentResults;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace Api.Services
{
    public class SettingService(ISettingRepository repo, IMapper mapper)
    {

        public async Task<Result<List<SettingResponse>>> GetAll()
        {
            var items = await repo.GetAllAsync();
            var result = mapper.Map<List<SettingResponse>>(items);
            return Result.Ok(result);
        }

        public async Task<Result<SettingResponse>> GetBy(Expression<Func<Setting, bool>> expression)
        {
            var service = await repo.GetByAsync(expression);
            if (service == null)
            {
                return Result.Fail(new Error($"Setting is not found"));
            }
            var result = mapper.Map<SettingResponse>(service);

            return Result.Ok(result);
        }


        public async Task<Result<SettingResponse>> CreateAsync(SettingCreate model)
        {
            var item = mapper.Map<Setting>(model);
            await repo.CreateAsync(item);
            var result = mapper.Map<SettingResponse>(item);
            return Result.Ok(result);
        }

        public async Task<Result<SettingResponse>> UpdateAsync(string name, SettingUpdate model)
        {
            Setting service = await repo.GetByAsync(p => p.Name == name);
            if (service is null)
            {
                var message = $"Setting with name {name} not exists";
                return Result.Fail(new Error(message));
            }

            service.Value = model.Value;

            var item = await repo.UpdateAsync(service);
            var result = mapper.Map<SettingResponse>(item);
            return Result.Ok(result);
        }

        public async Task<Result<string>> Delete(string name)
        {
            if (!await repo.Exists(p => p.Name == name))
            {
                return Result.Fail($"Setting with name {name} not exists");
            }
            try
            {
                await repo.RemoveAsync(p => p.Name == name);
            }
            catch (SqlException ex)
            {
                return Result.Fail(ex.Message);
            }
            return Result.Ok();
        }
    }
}
