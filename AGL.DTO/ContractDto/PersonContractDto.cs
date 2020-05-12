using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AGL.Dto.ContractDto
{
    public class PersonContractDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gender")]
        public GenderTypeEnum Gender { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("pets")]
        public List<PetContractDto> Pets { get; set; }
    }
}
