using AGL.Dto;
using AGL.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AGL.ExternalServices
{
    public abstract class BaseExternalService
    {
        /// <summary>
        /// A simple Http Request that returns a string or error
        /// </summary>
        public async Task<Response<string>> GetString(Uri uri, Dictionary<string, string> Headers = null)
        {
            var response = new Response<string>();

            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage();
                    request.RequestUri = uri;

                    if (Headers != null && Headers.Any())
                    {
                        Headers.ToList().ForEach(h =>
                        {
                            request.Headers.Add(h.Key, h.Value);
                        });
                    }

                    var httpResponse = await client.SendAsync(request);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        response.Data = responseContent;
                        return response;
                    }
                }
            }
            catch (Exception)
            {
                response.Errors.Add(ErrorMessages.CannotConnectToServer_01);
            }
            return response;
        }
    }
}
