using AGL.Dto;
using AGL.Dto.ContractDto;
using AGL.Library;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGL.ExternalServices
{
    public class PeopleExternalService : BaseExternalService, IPeopleExternalService
    {
        private IConfiguration _configuration { get; }

        public PeopleExternalService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns a List of PersonDto's or failure with errors.
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<PersonContractDto>>> GetPeople()
        {
            var response = new Response<List<PersonContractDto>>();

            var uri = new Uri(_configuration["PeopleUrl"]);

            var peopleResponse = await GetString(uri);
            if (peopleResponse.ResponseStatus == ResponseStatusEnum.Failure)
            {
                response.Errors.AddRange(peopleResponse.Errors);
                return response;
            }
            else
            {
                try
                {
                    response.Data = JsonConvert.DeserializeObject<List<PersonContractDto>>(peopleResponse.Data);
                    if(response.Data == null || !response.Data.Any())
                    {
                        response.Errors.Add(ErrorMessages.CannotDeserializePeople_02);
                    }
                }
                catch (JsonException)
                {
                    response.Errors.Add(ErrorMessages.CannotDeserializePeople_02);
                }
            }
            
            return response;
        }
    }
}
