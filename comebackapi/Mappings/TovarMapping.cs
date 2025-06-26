using comebackapi.Infrastructure;
using comebackapi.Models;
using comebackapi.Repositories;

namespace comebackapi.Mappings;

public static class TovarMapping
{
    public static IEndpointRouteBuilder MapTovarApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/tovar");

        group.MapGet("/", async (ITovarRepository tovarRepository) =>
        {
            return await tovarRepository.GetAllAsync();
        });

        group.MapPost("/", async (ITovarRepository tovarRepository,TovarRequest request) =>
        {
            var tovar = new Tovar() { Name = request.Name ,Price = request.Price };
            await tovarRepository.AddAsync(tovar);
        });

        group.MapPut("/{id}", async (ITovarRepository tovarRepository, TovarRequest request,int id) =>
        {
            var tovar = new Tovar() { Name = request.Name ,Price = request.Price };
            await tovarRepository.UpdateAsync(id, tovar);
        });

        group.MapDelete("/{id}", async (ITovarRepository tovarRepository, int id) =>
        {
            await tovarRepository.DeleteAsync(id);
        });
        return routes;
    }
}