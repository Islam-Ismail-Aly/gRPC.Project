using gRPC.API.Entites.Entites;

namespace gRPC.API.Entites.Services
{
    public static class ProductDbService
    {
        public static List<ProductEntity> products = new List<ProductEntity>
        {
            new ProductEntity { Id = 1, Name = "Wireless Mouse", Price = 29.99m, Qty = 150, Category = Category.COMPUTER_ACCESSORIES },
            new ProductEntity { Id = 2, Name = "Mechanical Keyboard", Price = 89.99m, Qty = 80, Category = Category.COMPUTER_ACCESSORIES },
            new ProductEntity { Id = 3, Name = "Gaming Headset", Price = 59.99m, Qty = 120, Category = Category.AUDIO },
            new ProductEntity { Id = 4, Name = "27-inch Monitor", Price = 199.99m, Qty = 45, Category = Category.MONITORS },
            new ProductEntity { Id = 5, Name = "External Hard Drive", Price = 79.99m, Qty = 200, Category = Category.STORAGE },
            new ProductEntity { Id = 6, Name = "USB-C Hub", Price = 49.99m, Qty = 100, Category = Category.COMPUTER_ACCESSORIES },
            new ProductEntity { Id = 7, Name = "Bluetooth Speaker", Price = 39.99m, Qty = 250, Category = Category.AUDIO },
            new ProductEntity { Id = 8, Name = "Smartphone Stand", Price = 19.99m, Qty = 300, Category = Category.OTHER },
            new ProductEntity { Id = 9, Name = "Laptop Cooling Pad", Price = 25.99m, Qty = 75, Category = Category.COMPUTER_ACCESSORIES },
            new ProductEntity { Id = 10, Name = "Wireless Charger", Price = 27.99m, Qty = 180, Category = Category.COMPUTER_ACCESSORIES }
        };
    }
}
