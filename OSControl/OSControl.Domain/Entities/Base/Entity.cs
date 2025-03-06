namespace OSControl.Domain.Entities.Base
{
    public class Entity
    {
        public long Id { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}