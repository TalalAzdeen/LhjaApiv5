using Api.Repositories;
using AutoMapper;
using Dto.Service;
using Entities;
using FluentResults;
using Microsoft.Data.SqlClient;

namespace Api.Services
{
    public class ServiceService(IServiceRepository repo, IMapper mapper)
    {

        public async Task<Result<List<ServiceResponse>>> GetAll()
        {
            var items = await repo.GetAllAsync();
            var result = mapper.Map<List<ServiceResponse>>(items);
            return Result.Ok(result);
        }



        public async Task<Result<ServiceResponse>> GetById(string id)
        {
            var service = await repo.GetByAsync(p => p.Id == id);
            if (service == null)
            {
                return Result.Fail(new Error($"Service with id {id} is not found"));
            }
            var result = mapper.Map<ServiceResponse>(service);

            return Result.Ok(result);
        }


        public async Task<Result<ServiceResponse>> GetByToken(string token)
        {
            var service = await repo.GetByAsync(p => p.Token == token);
            if (service == null)
            {
                return Result.Fail(new Error($"Service with token {token} is not found"));
            }
            var result = mapper.Map<ServiceResponse>(service);

            return Result.Ok(result);
        }


        public async Task<Result<ServiceResponse>> Create(ServiceCreate model)
        {
            try
            {
                var item = mapper.Map<Service>(model);
                await repo.CreateAsync(item);
                var result = mapper.Map<ServiceResponse>(item);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error(ex.InnerException?.Message).CausedBy(ex));
            }
        }

        public async Task<Result<ServiceResponse>> Update(string id, ServiceUpdate model)
        {
            try
            {
                Service service = await repo.GetByAsync(p => p.Id == id);
                if (service is null)
                {
                    var message = $"Service with id {id} not exists";
                    return Result.Fail(new Error(message));
                }

                service.Name = model.Name;
                service.Token = model.Token;

                var item = await repo.UpdateAsync(service);
                var result = mapper.Map<ServiceResponse>(item);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error(ex.InnerException?.Message).CausedBy(ex));
            }
        }

        public async Task<Result<string>> Delete(string id)
        {
            try
            {
                if (!await repo.Exists(p => p.Id == id))
                {
                    return Result.Fail($"Service with id {id} not exists");
                }
                try
                {
                    await repo.RemoveAsync(p => p.Id == id);
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("The DELETE statement conflicted"))
                    {
                        return Result.Fail("This service cannot be deleted because it has one or more plans.");
                    }
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error(ex.InnerException?.Message).CausedBy(ex));
            }
        }
    }
}
