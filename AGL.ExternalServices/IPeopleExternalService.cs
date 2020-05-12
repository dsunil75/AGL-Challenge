using AGL.Dto;
using AGL.Dto.ContractDto;
using AGL.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AGL.ExternalServices
{
    public interface IPeopleExternalService
    {
        /// <summary>
        /// Returns a List of PersonDto's or failure with errors.
        /// </summary>
        /// <returns></returns>
        Task<Response<List<PersonContractDto>>> GetPeople();
    }
}
