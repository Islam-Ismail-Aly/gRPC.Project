using Google.Protobuf.WellKnownTypes;
using gRPC.API.Entites.Entites;
using gRPC.API.Entites.Services;
using gRPC.API.Server.Protos;
using Grpc.Core;

public class ProductService : InventoryService.InventoryServiceBase
{
    private List<ProductEntity> _products = ProductDbService.products;

    public override Task<ProductServiceMessage> GetProductById(ProductIdRequest request, ServerCallContext context)
    {
        var productEntity = _products.FirstOrDefault(p => p.Id == request.Id);

        if (productEntity != null)
        {
            var product = new Product
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Price = (double)productEntity.Price,
                Qty = productEntity.Qty
            };

            return Task.FromResult(new ProductServiceMessage
            {
                Status = true,
                Product = product
            });
        }

        return Task.FromResult(new ProductServiceMessage
        {
            Status = false,
            Message = "Product not found"
        });
    }

    public override Task<ProductServiceMessage> AddProduct(Product request, ServerCallContext context)
    {
        if (_products.Any(p => p.Id == request.Id))
        {
            return Task.FromResult(new ProductServiceMessage
            {
                Status = false,
                Message = "Product with this ID already exists"
            });
        }

        var productEntity = new ProductEntity
        {
            Id = request.Id,
            Name = request.Name,
            Price = (decimal)request.Price,
            Qty = request.Qty
        };

        _products.Add(productEntity);

        return Task.FromResult(new ProductServiceMessage
        {
            Status = true,
            Product = request
        });
    }

    public override Task<ProductServiceMessage> UpdateProduct(Product request, ServerCallContext context)
    {
        var productEntity = _products.FirstOrDefault(p => p.Id == request.Id);

        if (productEntity != null)
        {
            productEntity.Name = request.Name;
            productEntity.Price = (decimal)request.Price;
            productEntity.Qty = request.Qty;

            return Task.FromResult(new ProductServiceMessage
            {
                Status = true,
                Product = request
            });
        }

        return Task.FromResult(new ProductServiceMessage
        {
            Status = false,
            Message = "Product not found"
        });
    }

    public override async Task<NumOfInsertedRowsMsg> AddBulkProd(IAsyncStreamReader<Product> requestStream, ServerCallContext context)
    {
        int num = 0;
        await foreach (var request in requestStream.ReadAllAsync())
        {
            this.AddProduct(request, context);
            num++;
        }
        return await Task.FromResult(new NumOfInsertedRowsMsg() { NumOfInsertedRows = num });
    }

    public override async Task GetProductsByCriteria(ProductCriteriaMsg request, IServerStreamWriter<Product> responseStream, ServerCallContext context)
    {
        var filteredProducts = ProductDbService.products.AsQueryable();

        if (request.Category != gRPC.API.Server.Protos.Category.Unknown)
            filteredProducts = filteredProducts.Where(p => p.Category == (gRPC.API.Entites.Entites.Category)request.Category);

        // Optional: Order by price if requested
        if (request.OrderByPrice)
            filteredProducts = filteredProducts.OrderBy(p => p.Price);


        foreach (var product in filteredProducts)
        {
            var productMsg = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Qty = product.Qty,
                Price = (double)product.Price,
                Category = (gRPC.API.Server.Protos.Category)product.Category
            };

            // Write product to the response stream
            await responseStream.WriteAsync(productMsg);
        }
    }
}
