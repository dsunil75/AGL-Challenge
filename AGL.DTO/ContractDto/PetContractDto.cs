using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGL.Dto.ContractDto
{
    public class PetContractDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public PetTypeEnum Type { get; set; }
    }
}
