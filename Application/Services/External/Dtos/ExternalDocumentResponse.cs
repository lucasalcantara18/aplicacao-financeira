using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.External.Dtos
{
    public class ExternalDocumentResponse
    {
        public string Document { get; set; }
        public bool Status { get; set; }
        public string Reason { get; set; }
    }
}
