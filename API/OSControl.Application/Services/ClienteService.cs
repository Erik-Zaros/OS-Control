using OSControl.Application.Services.Base;
using OSControl.Domain;
using OSControl.Domain.Repositories.Base;
using OSControl.Domain.Services;

namespace OSControl.Application.Services
{
    public class ClienteService : ServiceBase<Cliente>, IClienteService
    {
        public ClienteService(IRepositorioBase<Cliente> repositorioBase) : base(repositorioBase)
        {
        }
    }
}
