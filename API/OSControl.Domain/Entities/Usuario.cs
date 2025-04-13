using OSControl.Domain.Entities.Base;
using OSControl.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace OSControl.Domain.Entities
{
    public class Usuario : Entity
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Telefone { get; set; }
        [Required]
        public TipoUsuario TipoUsuario { get; set; }
        [Required]
        public string Especialidade { get; set; }
        public DateTime? UltimoAcesso { get; set; }
        [Required]
        public bool Ativo { get; set; }
    }
}
