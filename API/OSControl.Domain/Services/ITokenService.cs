using OSControl.Domain.Entities;

namespace OSControl.Domain.Services
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
