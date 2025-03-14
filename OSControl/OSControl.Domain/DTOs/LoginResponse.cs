namespace OSControl.Domain.DTOs
{
    public class LoginResponse
    {
        public AuthResponseModel Usuario { get; set; }
        public string Token { get; set; }
    }
}
