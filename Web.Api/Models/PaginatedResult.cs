using System.Net.NetworkInformation;

namespace Web.Api.Models
{
    public class PaginatedResult<T>
    {
        public int total { get; set; }

        public int page { get; set; }

        public int pageSize { get; set; }

        public List<T> data { get; set; }

    }
}
