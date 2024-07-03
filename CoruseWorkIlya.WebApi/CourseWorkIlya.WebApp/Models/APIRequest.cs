using CourseWork.Utility;
using System.Net.Mime;

namespace CourseWork.WebApp.Models
{
    public class APIRequest
    {
        public ApiMethod ApiMethod { get; set; } 
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
