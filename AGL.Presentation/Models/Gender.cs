using AGL.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGL.Presentation.Models
{
    public class Gender
    {
        public GenderTypeEnum GenderType { get; set; }
        public List<Cat> Cats { get; set; }
    }
}
