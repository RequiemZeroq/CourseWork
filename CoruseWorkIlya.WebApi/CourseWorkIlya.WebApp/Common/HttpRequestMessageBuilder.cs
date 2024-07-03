using CourseWork.Utility;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace CourseWork.WebApp.Common
{
    public class HttpRequestMessageBuilder
    {
        private HttpRequestMessage _message;
        public HttpRequestMessageBuilder()
        {
            _message = new HttpRequestMessage();
        }

        public HttpRequestMessageBuilder SetHttpMethod(ApiMethod method)
        {
            switch (method)
            {
                case ApiMethod.GET:
                    _message.Method = HttpMethod.Get;
                    break;
                case ApiMethod.POST:
                    _message.Method = HttpMethod.Post;
                    break;
                case ApiMethod.PUT:
                    _message.Method = HttpMethod.Put;
                    break;
                case ApiMethod.DELETE:
                    _message.Method = HttpMethod.Delete;
                    break;
            }

            return this;
        }

        public HttpRequestMessageBuilder SetRequestUri(string uri)
        {
            _message.RequestUri = new Uri(uri);

            return this;
        }

        public HttpRequestMessageBuilder SetRequestData(object data)
        {
            if (data is not null)
            {
                _message.Content = new StringContent(JsonConvert.SerializeObject(data),
                    Encoding.UTF8, MediaTypeNames.Application.Json);
            }

            return this;
        }

        public HttpRequestMessage Build()
            => _message;

    }
}
