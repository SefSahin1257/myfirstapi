using Microsoft.EntityFrameworkCore;
 public class ProductRepository : IProductRepository{
 private readonly AppDbContext _db;

 public ProductRepository(AppDbContext db){
  _db = db;
 }
 
 public async Task<Product?> GetByIdAsync(int id){
  var product = await _db.Products.FindAsync(id);
  return product;
 }

 public async Task<List<Product>> GetAllAsync(int page, int pageSize){
  var products = await _db.Products
  .OrderBy(p => p.Id)
  .Skip((page - 1) * pageSize)
  .Take(pageSize)
  .ToListAsync();
  return products;
 }

 public async Task<Product> AddAsync(Product product){
  _db.Products.Add(product);
  await _db.SaveChangesAsync();
  return product;
 }
 
 public async Task UpdateAsync(Product product){
  await _db.SaveChangesAsync();
 }

 public async Task DeleteAsync(Product product){
  _db.Products.Remove(product);
  await _db.SaveChangesAsync();
 }
	
}
