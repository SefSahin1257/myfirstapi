using ProductApi.Domain.Entities;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Dtos;
using Microsoft.Extensions.Logging;
using ProductApi.Application.Exceptions;

namespace ProductApi.Application.Services;

public class ProductService : IProductService {
	private readonly IProductRepository _repository;
	private readonly ILogger<ProductService> _logger;
	private readonly ICacheService _cache;
	public ProductService(IProductRepository productRepository, ILogger<ProductService> logger, ICacheService cache){
		_repository = productRepository;
		_logger = logger;
		_cache = cache;
	}

	public async Task<Product> GetByIdAsync(int id)
	{
		var cacheKey = $"product_{id}";
		
		//1. Önce cache'e baka
		var cachedProduct = _cache.Get<Product>(cacheKey);
		if (cachedProduct != null)
		{
			_logger.LogInformation("Cache HIT: ProductId = {ProductId}", id);
			return cachedProduct;
		}
		
		//2. Cache'te yoksa veritabanından getir
		_logger.LogInformation("Cache MISS: ProductId = {ProductId}", id);

		var product = await _repository.GetByIdAsync(id);

		if (product == null)
		{
			throw new NotFoundException("Ürün bulunamadı.");
		}
		
		//3. Ürünü 5 dakika cache'te sakla
		_cache.Set(cacheKey, product, TimeSpan.FromMinutes(5));

		return product;
	}

	public async Task<PagedResultDto<Product>> GetAllAsync(int page, int pageSize, int? categoryId){
	if(page < 1){ 
		throw new ArgumentException("Page değeri 1 veya daha büyük olmalı.");
	}
	if(pageSize < 1 || pageSize > 100){ 
		throw new ArgumentException("PageSize değeri 1 ile 100 arasında olmalıdır.");
	}

	var products = await _repository.GetAllAsync(page, pageSize, categoryId);
	var totalCount = await _repository.CountAsync(categoryId);
	
	var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);  	

	return new PagedResultDto<Product>{
 	  Items = products,
	  Page = page,
	  PageSize = pageSize,
	  TotalCount = totalCount,
	  TotalPages = totalPages
	 };
	}
	
	public async Task<Product> CreateAsync(CreateProductDto dto)
	{
		var product = new Product(dto.Name, dto.Price)
		{
			CategoryId = dto.CategoryId
		};
	var createdProduct = await _repository.AddAsync(product);
	
	_logger.LogInformation(
	 "Ürün oluşturuldu. ProductId = {ProductId}, Name = {ProductName}",
	 createdProduct.Id,
	 createdProduct.Name
	);
	 return createdProduct;
	}

	public async Task<Product> UpdateAsync(int id, UpdateProductDto dto){
	var product = await _repository.GetByIdAsync(id);
	if(product == null){
 	 throw new NotFoundException("Ürün bulunamadı.");
	}

	product.Name = dto.Name;
	product.Price = dto.Price;
	
	await _repository.UpdateAsync(product);
	
	// Veritabanı güncellendikten sonra eski cache kaydını sil
	_cache.Remove($"product_{id}");
	
	return product;
	}

	public async Task DeleteAsync(int id){
		
	var product = await _repository.GetByIdAsync(id);
	if(product == null)
	{ 
		throw new NotFoundException("Ürün bulunamadı");
	}
	await _repository.DeleteAsync(product);
	
	// Silinen ürünün cache kaydını temizle 
	_cache.Remove($"product_{id}");
	}
}
