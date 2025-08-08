using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.External.Dtos
{
    public class ExternalCodeResponse
    {
        public string UserId { get; set; }
        public string AuthCode { get; set; }
    }
}
