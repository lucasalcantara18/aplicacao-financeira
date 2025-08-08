using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.External.Dtos
{
    public class ExternalApiResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
    }
}
