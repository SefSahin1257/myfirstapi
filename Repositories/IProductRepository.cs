 public interface IProductRepository{
	Task<Product?> GetByIdAsync(int id);
	Task<List<Product>> GetAllAsync(int page, int pageSize);
	Task<Product> AddAsync(Product product);
	Task UpdateAsync(Product product);
	Task DeleteAsync(Product product);
	Task<int> CountAsync();
 }
