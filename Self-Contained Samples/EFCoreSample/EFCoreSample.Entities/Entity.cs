namespace EFCoreSample.Entities
{
    public class Entity<TKey> : AuditableEntity where TKey : struct
    {
        public virtual TKey Id { get; protected set; }
    }
}
