using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase{
	private readonly AppDbContext _db;
	
	public ProductController(AppDbContext db){
	_db = db;
	}

	[HttpGet]
	public async Task<IActionResult> GetProducts(){
	var products = await _db.Products.ToListAsync();
	return Ok(products);
	}
	
	[HttpGet("{id}")]
	public async Task<IActionResult> GetProduct(int id){
	var product = await _db.Products.FindAsync(id);
	if(product == null){
		return NotFound();
	}
	return Ok(product);	
	}
	
	[HttpPost]
	public async Task<IActionResult> CreateProduct(CreateProductDto dto){
	var prd = new Product(dto.Name, dto.Price);
	_db.Products.Add(prd);
	await _db.SaveChangesAsync();
	return Created($"/api/product/{prd.Id}", prd);
	}
	
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto){
	var prd = await _db.Products.FindAsync(id);
	if(prd == null){
	return NotFound();
	}
	prd.Name = dto.Name;
	prd.Price = dto.Price;
	await _db.SaveChangesAsync();
	return Ok(prd);
	}	

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProduct(int id){
	var prd = await _db.Products.FindAsync(id);
	if(prd == null) return NotFound();
	_db.Products.Remove(prd);
	await _db.SaveChangesAsync();
	return NoContent();
	}
}
