using OSControl.Domain;
using OSControl.Domain.Repositories;
using OSControl.Infrastructure.Data;
using OSControl.Infrastructure.Repositories.Base;

namespace OSControl.Infrastructure.Repositories
{
    public class ClienteRepository : RepositorioBase<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext contexto) : base(contexto)
        {
        }
    }
}
