using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CryptoTrading.API.Models
{
    public class ErrorResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
