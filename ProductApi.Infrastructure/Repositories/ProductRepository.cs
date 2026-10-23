using ProductApi.Domain.Entities;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductApi.Infrastructure.Repositories;

 public class ProductRepository : IProductRepository{
 private readonly AppDbContext _db;

 public ProductRepository(AppDbContext db){
  _db = db;
 }
 
 public async Task<Product?> GetByIdAsync(int id)
 {
  var product = await _db.Products
   .Include(p => p.Category)
   .FirstOrDefaultAsync(p => p.Id == id);
   return product;
 }

 public async Task<List<Product>> GetAllAsync(int page, int pageSize, int? categoryId)
 {
  var query = _db.Products
   .AsNoTracking()
   .Include(p => p.Category)
   .AsQueryable();

  if (categoryId.HasValue)
  {
   query = query.Where(p => p.CategoryId == categoryId.Value);
  }

  var products = await query
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

 public async Task<int> CountAsync(int? categoryId)
 {
  var query = _db.Products.AsQueryable();

  if (categoryId.HasValue)
  {
   query = query.Where(p => p.CategoryId == categoryId.Value);
  }
  return await query.CountAsync();
 }	
}
