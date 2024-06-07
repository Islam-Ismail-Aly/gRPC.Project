using gRPC.API.Client.Services;
using gRPC.API.Server.Protos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly GrpcInventoryClientService _grpcClient;

    public ProductsController(GrpcInventoryClientService grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpPost]
    public async Task<IActionResult> AddOrUpdateProduct([FromBody] Product product)
    {
        var getResponse = await _grpcClient.GetProductByIdAsync(product.Id);

        if (getResponse.Status)
        {
            var updateResponse = await _grpcClient.UpdateProductAsync(product);

            if (updateResponse.Status)
                return Ok(updateResponse.Product);

            return BadRequest(updateResponse.Message);
        }
        else
        {
            var addResponse = await _grpcClient.AddProductAsync(product);

            if (addResponse.Status)
                return Created();

            return BadRequest(addResponse.Message);
        }
    }

    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var response = await _grpcClient.GetProductByIdAsync(id);

        if (response.Status)
            return Ok(response.Product);

        return NotFound(response.Message);
    }

    [HttpGet("GetListProducts")]
    public async Task<IActionResult> GetListProducts(ProductCriteriaMsg criteriaMsg)
    {
        var response = await _grpcClient.GetListProducts(criteriaMsg);

        if (response.Count > 0)
            return Ok(response);

        return NotFound(response);
    }

    [HttpPost("AddBulkProducts")]
    public async Task<IActionResult> AddBulkProduct(List<Product> products)
    {
        var response = await _grpcClient.AddBulkProduct(products);

        if (response.NumOfInsertedRows > 0)
            return Ok(response);

        return BadRequest(response);
    }
}
