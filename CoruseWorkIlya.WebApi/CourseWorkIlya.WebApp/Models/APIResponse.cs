using System.Net;

namespace CourseWork.WebApp.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
            = default!;
        public object Result { get; set; }
            = default!;
    }
}
