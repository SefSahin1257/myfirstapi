using ProductApi.Domain.Entities;
namespace ProductApi.Application.Interfaces;
using ProductApi.Application.Dtos;
public interface IProductService{
	Task<Product> GetByIdAsync(int id);
	Task<PagedResultDto<Product>> GetAllAsync(int size, int pageSize, int? categoryId);	
	Task<Product> CreateAsync(CreateProductDto dto);
	Task<Product> UpdateAsync(int id, UpdateProductDto dto);
	Task DeleteAsync(int id);
}
