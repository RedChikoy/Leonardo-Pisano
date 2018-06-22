
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Interfaces;
using System.Configuration;

namespace BLL.Services
{
    public class ApiTransportService: IApiTransportService
    {
        private const string ApiCaclulateMethod = "api/calc/сaclulate";
        private static HttpClient ContinuerClient => InitContinuerClient();

        public async Task<HttpResponseMessage> SendValueAsync(Chisler value)
        {
            InitContinuerClient();

            var response = await ContinuerClient.PostAsJsonAsync(ApiCaclulateMethod, value);

            return response;
        }

        private static HttpClient InitContinuerClient()
        {
            var continuerUrl = ConfigurationManager.AppSettings["ContinuerApiUrl"];

            var result = new HttpClient {BaseAddress = new Uri(continuerUrl)};
            result.DefaultRequestHeaders.Accept.Clear();
            result.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return result;
        }
    }
}
