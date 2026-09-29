using Microsoft.EntityFrameworkCore;
public class ProductService : IProductService {
	private readonly AppDbContext _db;
	public ProductService(AppDbContext db){
		_db = db;
	}

	public async Task<Product?> GetByIdAsync(int id){
	var prdId = await _db.Products.FindAsync(id);
	return prdId;
	}

	public async Task<List<Product>> GetAllAsync(){
	var products = await _db.Products.ToListAsync();
	return products;
	}
	
	public async Task<Product> CreateAsync(CreateProductDto dto){
	var product = new Product(dto.Name, dto.Price);
	_db.Products.Add(product);
	await _db.SaveChangesAsync();
	return product;
	}

	public async Task<Product?> UpdateAsync(int id, UpdateProductDto dto){
	var product = await GetByIdAsync(id);
	if(product == null) return null;
	product.Name = dto.Name;
	product.Price = dto.Price;
        await _db.SaveChangesAsync();
	return product;
	}

	public async Task<bool> DeleteAsync(int id){
	var product = await GetByIdAsync(id);
	if(product == null) return false;
	_db.Products.Remove(product);
	await _db.SaveChangesAsync();
	return true;
	}
}
