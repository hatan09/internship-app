using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipApp.Api.DataObjects
{
    public class BaseDTO<T>
    {
        public T? Id { get; set; }
    }
}
