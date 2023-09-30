
namespace crudSignalR.Core.Application.Common
{
    public class GenericApiResponse<T>
    {
        public T? payload { get; set; }
        public bool success { get; set; } = true;
        public int statuscode { get; set; }
        public List<string> messages { get; set; } = new List<string> { };  
    }
}
