public interface IProductService{
	Task<Product?> GetByIdAsync(int id);
	Task<List<Product>> GetAllAsync();	
	Task<Product> CreateAsync(CreateProductDto dto);
	Task<Product?> UpdateAsync(int id, UpdateProductDto dto);
	Task<bool> DeleteAsync(int id);
}
