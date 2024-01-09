using Dapper;
using Web.Api.IRepository;
using Web.Api.Models;
using Web.Api.Services;

namespace Web.Api.Endpoints
{
    public static class PersonEndpoints
    {
        public static void MapPersonEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("persons", async (IPersonRepository repo) =>
            {
                return await repo.GetAllPersons();
            });
            builder.MapGet("person/search",async (string search, IPersonRepository repo) =>
            {
                return await repo.GetAllPersonsBySearch(search);
            });
            builder.MapGet("person/id", async (IPersonRepository repo,int id) =>
            {
                return await repo.GetPersonById(id);
            });
            builder.MapPost("person", async (Person obj, IPersonRepository repo) =>
            {
                return await repo.CreateorUpdate(obj, true);
            });
            builder.MapPut("person", async (Person obj, IPersonRepository repo) =>
            {
                return await repo.CreateorUpdate(obj, false);
            });

            builder.MapGet("person/paginated", async (IPersonRepository repo, int pageNumber, int pageSize, int col, bool direction, string? search) =>
            {
                var result = await repo.GetPaginated(pageNumber, pageSize, col, direction, search);
                return result;
            });

            builder.MapDelete("person", async (IPersonRepository repo,int id) =>
            {
                return await repo.DeletePerson(id);
            });
        }
    }
}
