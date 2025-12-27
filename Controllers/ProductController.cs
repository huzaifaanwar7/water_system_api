using Microsoft.AspNetCore.Mvc;
using GBS.Service;

[Route("api/[controller]")]
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
}
