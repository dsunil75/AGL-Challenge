using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGL.Library
{
    public class Response<T>
    {
        public List<string> Errors { get; set; } = new List<string>();
        public ResponseStatusEnum ResponseStatus => Errors.Any() ? ResponseStatusEnum.Failure : ResponseStatusEnum.Success;
        public T Data { get; set; }
    }
}
