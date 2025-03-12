namespace OSControl.Application.Helpers
{
    public class BCryptHash
    {
        private const int WorkFactor = 12;

        public string Hash(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha, WorkFactor);
        }

        public bool Verify(string senha, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(senha, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}
