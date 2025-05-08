using OSControl.Domain.Entities.Base;

namespace OSControl.Domain
{
    public class Cliente : Entity
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
