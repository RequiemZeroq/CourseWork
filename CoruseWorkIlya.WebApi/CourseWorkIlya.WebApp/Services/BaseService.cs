using CourseWork.WebApp.Models;
using CourseWork.WebApp.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using CourseWork.WebApp.Common;

namespace CourseWork.WebApp.Services
{
    public abstract class BaseService : IBaseService
    {
        private readonly IHttpContextAccessor _httpContext;
        public IHttpClientFactory HttpClient { get; set; }
        protected abstract string ServiceUrl { get; set; }
        public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContext)
        {
            HttpClient = httpClient;
            _httpContext = httpContext;
        }
        public async Task<TResponse> SendAsync<TResponse>(APIRequest request)
        {
            try
            {
                var client = HttpClient.CreateClient();

                HttpRequestMessage message = new HttpRequestMessageBuilder()
                    .SetHttpMethod(request.ApiMethod)
                    .SetRequestUri(request.Url)
                    .SetRequestData(request.Data)
                    .Build();

                HttpResponseMessage apiResponse;

                await SetAccessToken(client);

                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                TResponse ApiResponseParsed = JsonConvert.DeserializeObject<TResponse>(apiContent)!;
                return ApiResponseParsed;

            }
            catch (Exception ex) 
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var result = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<TResponse>(result);
                return APIResponse;
            }
        }

        private async Task SetAccessToken(HttpClient client)
        {
            if (await _httpContext!.HttpContext!.GetTokenAsync("access_token") is not null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    await _httpContext!.HttpContext!.GetTokenAsync("access_token"));
            }
        }
    }
}
