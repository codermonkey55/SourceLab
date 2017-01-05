using System.Collections.Generic;

namespace ODACSample.EF.Entities
{
    public class Category
    {
        public Category()
        {

        }

        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public ICollection<Product> Products { get; internal set; }
    }
}
