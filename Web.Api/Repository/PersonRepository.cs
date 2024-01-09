using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Web.Api.IRepository;
using Web.Api.Models;
using Web.Api.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Api.Repository
{
    public class PersonRepository: IPersonRepository
    {
        private readonly SqlConnectionFactory _connection;
        public PersonRepository(SqlConnectionFactory connection)
        {
            _connection= connection;
        }
        public async Task<Response<List<Person>>> GetAllPersons()
        {
            try
            {
                using var connections = _connection.Create();
                var query = $@"Select * From Person";
                var result = await connections.QueryAsync<Person>(query);
               if(result != null && result.Any())
                {
                    return Response<List<Person>>.Success(200, "Result Found", result.ToList());
                }
                return Response<List<Person>>.Error(404, $"Result not found");

            }
            catch (Exception ex)
            {
                return Response<List<Person>>.Error(500, $"Error in finding data: ${ex.Message}");
            }

        }
        public async Task<Response<Person>> GetPersonById(int id)
        {
            try
            {
                using var connections = _connection.Create();
                var query = $@"Select * From Person Where Id={id}";
                var result = await connections.QueryAsync<Person>(query);
                if(result != null)
                {
                return Response<Person>.Success(200,"Result Found", result.First());
                }
                return Response<Person>.Error(404, $"Result not found");
            }
            catch(Exception ex) {
                return Response<Person>.Error(500,$"Error in finding data: ${ex.Message}");
            }
    
        }

        public async Task<Response<List<Person>>> GetAllPersonsBySearch(string search)
        {
            try
            {
            using var connections = _connection.Create();
            var query = $@"Select * From Person WHERE Name Like '%'+'{search}'+'%' OR Email Like '%'+'{search}'+'%'";
            var result = await connections.QueryAsync<Person>(query);
            if(result!= null && result.Any())
            {
                    return Response<List<Person>>.Success(200, "Result Found", result.ToList());
                }
                return Response<List<Person>>.Error(404, $"Result not found");
            }
            catch(Exception ex) {
                return Response<List<Person>>.Error(500, $"Error in finding data: ${ex.Message}");
            }

        }
        public async Task<Response<Person>> CreateorUpdate(Person obj, bool isCreate)
        {
            try
            {
                using var connections = _connection.Create();
                var parameters = new DynamicParameters();
                parameters.Add("@Create", isCreate ? 1 : 0);
                parameters.Add("@Id", obj.Id);
                parameters.Add("@Name", obj.Name);
                parameters.Add("@Email", obj.Email);
                parameters.Add("@OutputKey",dbType:DbType.Int32,direction:ParameterDirection.Output);

                var result = await connections.QueryAsync<int>("[dbo].[CreateOrUpdatePerson]", parameters, commandType: CommandType.StoredProcedure);
                int updatedId = result.First();
                var getQuery = $@"Select * From Person Where Id={updatedId}";
                var resultgetQuery = await connections.QueryAsync<Person>(getQuery);
                if(resultgetQuery!=null && resultgetQuery.Any())
                {
                    if (isCreate)
                    {
                        return Response<Person>.Success(201, $"Result Created", resultgetQuery.FirstOrDefault());

                    }
                    else
                    {
                        return Response<Person>.Success(200, $"Result Updated", resultgetQuery.FirstOrDefault());
                    }
                }
                return Response<Person>.Error(404, $"Result not found");

            }
            catch (Exception ex)
            {
                return Response<Person>.Error(500, $"Error in finding data: ${ex.Message}");
            }
        }
        public async Task<Response<PaginatedResult<Person>>> GetPaginated(int pageNumber = 1, int pageSize = 10, int col = 1, bool direction = true, string search = "")
        {
            try
            {
                using var connections = _connection.Create();
                var dir = direction ? "asc" : "desc";
                var parameters = new DynamicParameters();
                parameters.Add("@pageNumber", pageNumber);
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@col", col);
                parameters.Add("@direction", dir);
                parameters.Add("@search", search);
                var result = connections.QueryMultiple("[dbo].[PaginatedPerson]", parameters, commandType: CommandType.StoredProcedure);
                var Data = result.Read<Person>();
                var Total = result.ReadSingle<int>();
                var paginated = new PaginatedResult<Person>
                {
                    data = Data.ToList(),
                    total = Total,
                    page = pageNumber,
                    pageSize = pageSize
                };
                if (paginated != null)
                {
                    return Response<PaginatedResult<Person>>.Success(200, "Success", paginated);
                }
                return Response<PaginatedResult<Person>>.Error(404, $"Result not found");
            }
            catch (Exception ex)
            {
                return Response<PaginatedResult<Person>>.Error(500, $"Error in finding data: ${ex.Message}");
            }

        }
        public async Task<Response<Person>> DeletePerson(int id)
        {
            try
            {
            using var connection= _connection.Create();
            var query = $@"Select * From Person where Id={id}";
            var result = await connection.QueryAsync<Person>(query);
                if (result != null && result.Any())
                {
                    var queryDelete = $@"Delete From Person where Id={id}";
                    var resultDelete = await connection.QueryAsync(queryDelete);
                    return Response<Person>.Success(200, $"Result deleted", result.First());
                }
                return Response<Person>.Error(404, $"Result not found");
            }
            catch (Exception ex)
            {
                return Response<Person>.Error(500, $"Error in finding data: ${ex.Message}");
            }

        }
    }
}
