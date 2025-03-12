namespace OSControl.Domain.DTOs
{
    public class AlterarSenhaModel
    {
        public long UserId { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
    }
}
