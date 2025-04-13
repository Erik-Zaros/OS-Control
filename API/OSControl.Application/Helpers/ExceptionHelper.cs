namespace OSControl.Application.Helpers
{
    public static class ExceptionHelper
    {
        public static void Nulo(object? value, string message)
        {
            if (value == null) throw new KeyNotFoundException(message);
        }

        public static void Inativo(bool isActive, string message)
        {
            if (!isActive) throw new InvalidOperationException(message);
        }

        public static void Executado(bool executado, string message)
        {
            if (executado) throw new InvalidOperationException(message);
        }

        public static void Existe(bool exists, string message)
        {
            if (exists) throw new InvalidOperationException(message);
        }

        public static void Obrigatorio(string? value, string message)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException(message);
        }

        public static void NaoAutorizado(string message)
        {
            throw new UnauthorizedAccessException(message);
        }
    }
}