using OSControl.Domain.ValueObjects;

namespace OSControl.Domain.DTOs
{
    public class UsuarioModel
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Especialidade { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public bool Ativo { get; set; }
    }
}
