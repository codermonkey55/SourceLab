namespace ODACSample.EF.Entities
{
    public class Product
    {
        public Product()
        {

        }

        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long SupplierId { get; set; }
        public long CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public long UnitPrice { get; set; }
        public long UnitsInStock { get; set; }
        public long UnitsOnOrder { get; set; }
        public long ReorderLevel { get; set; }
        public string Discontinued { get; set; }

        public Category Category { get; set; }
    }
}
