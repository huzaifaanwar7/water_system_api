using Microsoft.AspNetCore.Mvc;
using GBS.Service;
using GBS.Model;
using Microsoft.AspNetCore.Http.HttpResults;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("ProductList")]
    public async Task<IActionResult> ProductList()
    {
        var list = await _productService.GetProductList();
        return Ok(list);
    }

    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }
    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] ProductVM model)
    {
        var id = await _productService.AddProduct(model);
        return Ok(new { Message = "Product Added Successfully", ProductId = id });
    }
    [HttpPut("UpdateProduct/{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductVM model)
    {
        var updated = await _productService.UpdateProduct(id, model);
        if (!updated) return NotFound();

        return Ok(new { Message = "Product updated successfully" });
    }
    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService.DeleteProduct(id);
        if (!deleted) return NotFound();

        return Ok(new { Message = "Product deleted successfully" });
    }

}
