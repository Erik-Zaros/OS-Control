using OSControl.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace OSControl.Domain.Entities
{
    public class Produto : Entity
    {
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public bool Ativo { get; set; }
    }
}
