using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Interface.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace crudSegnalR.Infrastructure.httpclient.Http
{
    public class GenericHttp: IGenericHttp
    {
        private readonly HttpClient _httpClient = new  HttpClient();
        public async Task<GenericApiResponse<string>> Post(string requestUri, dynamic jsonData) 
        {
            GenericApiResponse<string> genericApiResponse = new GenericApiResponse<string>();
            try
            {
                string jsonSerialize = JsonConvert.SerializeObject(jsonData);

                var content = new StringContent(jsonSerialize, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    string targetResult = await response.Content.ReadAsStringAsync();
                    genericApiResponse.payload= targetResult;
                    return genericApiResponse;
                }
                genericApiResponse.messages.Add("ocurrió un error");
                genericApiResponse.success = false;
                return genericApiResponse;
            }
            catch (Exception ex)
            {
                genericApiResponse.messages.Add(ex.Message);
                genericApiResponse.success = false;
                return genericApiResponse;               
            }
           
        }
        public async Task<GenericApiResponse<string>> Get(string requestUri) 
        {
            GenericApiResponse<string> genericApiResponse = new GenericApiResponse<string>();
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    string targetResult = await response.Content.ReadAsStringAsync();
                    genericApiResponse.payload = targetResult;
                    return genericApiResponse;
                }
                genericApiResponse.messages.Add("ocurrió un error");
                genericApiResponse.success = false;
                return genericApiResponse;
            }
            catch (Exception ex)
            {
                genericApiResponse.messages.Add(ex.Message);
                genericApiResponse.success = false;
                return genericApiResponse;
            }
        }       
    }
}
