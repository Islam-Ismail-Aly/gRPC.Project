namespace gRPC.API.Entites.Entites
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public Category Category { get; set; }
    }

    public enum Category
    {
        UNKNOWN = 0,
        ELECTRONICS = 1,
        COMPUTER_ACCESSORIES = 2,
        AUDIO = 3,
        MONITORS = 4,
        STORAGE = 5,
        OTHER = 6
    }
}
