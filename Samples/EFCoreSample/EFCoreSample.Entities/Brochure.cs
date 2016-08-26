namespace EFCoreSample.Entities
{
    public class Brochure : Entity<int>
    {
        public Brochure()
        {

        }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }
    }
}
