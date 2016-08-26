using System;

namespace EFCoreSample.Entities
{
    public class Friend : Entity<Guid>
    {
        public int Age { get; set; }
    }
}
