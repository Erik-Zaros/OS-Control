using OSControl.API.Controllers.Base;
using OSControl.Domain;
using OSControl.Domain.Services.Base;

namespace OSControl.API.Controllers
{
    public class ClienteController : BaseController<Cliente>
    {
        public ClienteController(IServiceBase<Cliente> service) : base(service)
        {
        }
    }
}
