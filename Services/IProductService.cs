public interface IProductService{
	Task<Product?> GetByIdAsync(int id);
	Task<List<Product>> GetAllAsync(int size, int pageSize);	
	Task<Product> CreateAsync(CreateProductDto dto);
	Task<Product?> UpdateAsync(int id, UpdateProductDto dto);
	Task<bool> DeleteAsync(int id);
}
