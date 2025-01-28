using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Seeds
{
    public static class DefaultModals
    {
        public static async Task SeedAsync(DataContext context)
        {
            if (await context.ModelGateways.FirstOrDefaultAsync(p => p.Name == "huggingface") == null)
            {

                await context.ModelGateways.AddAsync(new ModelGateway
                {
                    Name = "huggingface",
                    Url = "https://huggingface.co/wasmdashai",
                    IsDefault = true
                });
                await context.SaveChangesAsync();
            }



        }
    }
}