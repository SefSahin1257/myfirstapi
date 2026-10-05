using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase{
	private readonly IProductService _productService;
	
	public ProductController(IProductService productService){
	_productService = productService;
	}
	
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10){
	var products = await _productService.GetAllAsync(page, pageSize);
	var dtoProducts = products.Select(ToDto).ToList();
	return Ok(dtoProducts);
	}
	
	[HttpGet("{id}")]
	public async Task<IActionResult> GetProduct(int id){
	var product = await _productService.GetByIdAsync(id);
	if(product == null) return NotFound();
	return Ok(ToDto(product));	
	}
	
	[HttpPost]
	public async Task<IActionResult> CreateProduct(CreateProductDto dto){
	  var prd = await _productService.CreateAsync(dto);
	  return Created($"/api/product/{prd.Id}", ToDto(prd));	
	}
	
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto){
	var product = await _productService.UpdateAsync(id,dto);
	if(product == null) return NotFound();
	return Ok(ToDto(product));
	}	

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProduct(int id){
	var prd = await _productService.DeleteAsync(id);
	if(!prd) return NotFound();
	
	return NoContent();
	}
	
	private static ProductDto ToDto(Product product){
	 return new ProductDto{
	  Id = product.Id,
	  Name = product.Name,
	  Price = product.Price
	  };
	}
	
}
