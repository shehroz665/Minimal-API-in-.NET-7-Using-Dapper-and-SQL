using Web.Api.Models;

namespace Web.Api.IRepository
{
    public interface IPersonRepository
    {
        Task<Response<List<Person>>> GetAllPersons();

        Task<Response<Person>> GetPersonById(int id);

        Task<Response<List<Person>>> GetAllPersonsBySearch(string search); 

        Task<Response<Person>> CreateorUpdate(Person obj,bool isCreate);
        Task<Response<PaginatedResult<Person>>> GetPaginated(int pageNumber = 1, int pageSize = 10, int col = 1, bool direction = true, string search = "");

        Task<Response<Person>> DeletePerson(int id);
    }
}
