using Microsoft.AspNetCore.Mvc;
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
	var result = await _productService.GetAllAsync(page, pageSize);
        
	var response = new PagedResultDto<ProductDto>
	 {
 	  Items = result.Items.Select(ToDto).ToList(),
	  Page = result.Page,
	  PageSize = result.PageSize,
	  TotalCount = result.TotalCount,
	  TotalPages = result.TotalPages
	 };
	 
  	return Ok(response);
	}
	
	[HttpGet("{id}")]
	public async Task<IActionResult> GetProduct(int id){
	var product = await _productService.GetByIdAsync(id);
	return Ok(ToDto(product));	
	}
	
	[HttpPost]
	public async Task<IActionResult> CreateProduct(CreateProductDto dto){
	  var prd = await _productService.CreateAsync(dto);
	  return CreatedAtAction(nameof(GetProduct),new { id = prd.Id }, ToDto(prd));	
	}
	
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto){
	var product = await _productService.UpdateAsync(id,dto);
	return Ok(ToDto(product));
	}	

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProduct(int id){
	 await _productService.DeleteAsync(id);
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
