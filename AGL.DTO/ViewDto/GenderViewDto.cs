using System;
using System.Collections.Generic;
using System.Text;

namespace AGL.Dto.ViewDto
{
    public class GenderViewDto
    {
        public GenderTypeEnum Gender { get; set; }
        public List<CatViewDto> Cats { get; set; }
    }
}
