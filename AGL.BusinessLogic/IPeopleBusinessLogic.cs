using AGL.Dto;
using AGL.Dto.ViewDto;
using AGL.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AGL.BusinessLogic
{
    public interface IPeopleBusinessLogic
    {
        /// <summary>
        /// Retrieves People from the external web service and outputs a list of all the cats in alphabetical order 
        /// under a heading of the gender of their owner.
        /// </summary>
        Task<Response<List<GenderViewDto>>> GetPeople();
    }
}
