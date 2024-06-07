
using Google.Protobuf.WellKnownTypes;
using gRPC.API.Server.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace gRPC.API.Client.Services
{
    public class GrpcInventoryClientService
    {
        private readonly InventoryService.InventoryServiceClient _client;

        public GrpcInventoryClientService(InventoryService.InventoryServiceClient client)
        {
            _client = client;
        }

        public async Task<ProductServiceMessage> GetProductByIdAsync(int id)
        {
            var request = new ProductIdRequest 
            { 
                Id = id 
            };

            return await _client.GetProductByIdAsync(request);
        }

        public async Task<ProductServiceMessage> AddProductAsync(Product product)
        {
            return await _client.AddProductAsync(product);
        }

        public async Task<ProductServiceMessage> UpdateProductAsync(Product product)
        {
            return await _client.UpdateProductAsync(product);
        }

        public async Task<NumOfInsertedRowsMsg> AddBulkProduct(List<Product> products)
        {
            var call = _client.AddBulkProd();
            foreach (var product in products)
            {
                await call.RequestStream.WriteAsync(new Product()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Qty = product.Qty,
                    Category = (gRPC.API.Server.Protos.Category)product.Category,

                });
                await Task.Delay(1000);
            }

            await call.RequestStream.CompleteAsync();
            var response = await call.ResponseAsync;

            return new NumOfInsertedRowsMsg()
            {
                NumOfInsertedRows = response.NumOfInsertedRows
            };
        }

        public async Task<List<Product>> GetListProducts(ProductCriteriaMsg criteriaMsg)
        {
            var productList = new List<Product>();

            using (var call = _client.GetProductsByCriteria(criteriaMsg))
            {
                // Iterate over the response stream and collect products
                while (await call.ResponseStream.MoveNext())
                {
                    var productMsg = call.ResponseStream.Current;

                    // Convert productMsg to Product model and add to the list
                    var product = new Product
                    {
                        Id = productMsg.Id,
                        Name = productMsg.Name,
                        Price = productMsg.Price,
                        Qty = productMsg.Qty,
                        Category = productMsg.Category
                    };

                    productList.Add(product);
                }
            }

            return productList;
        }
    }
}
