using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRPC.API.Entites.Interfaces
{
    public interface IApiKeyAuthenticationService
    {
        bool Authenticate();
    }
}
