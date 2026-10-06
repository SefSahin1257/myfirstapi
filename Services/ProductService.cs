public class ProductService : IProductService {
	private readonly IProductRepository _repository;
	private readonly ILogger<ProductService> _logger;
	public ProductService(IProductRepository productRepository, ILogger<ProductService> logger){
		_repository = productRepository;
		_logger = logger; 
	}

	public async Task<Product> GetByIdAsync(int id){
         var prd =  await _repository.GetByIdAsync(id);
	 if(prd == null){
    	  throw new NotFoundException("Ürün bulunamadı.");
         }
 	 return prd; 
	}

	public async Task<PagedResultDto<Product>> GetAllAsync(int page, int pageSize){
	if(page < 1){
          throw new ArgumentException("Page değeri 1 veya daha büyük olmalı.");
	}
	if(pageSize < 1 || pageSize > 100){
	  throw new ArgumentException("PageSize değeri 1 ile 100 arasında olmalıdır.");
	}

	var products = await _repository.GetAllAsync(page, pageSize);
	var totalCount = await _repository.CountAsync();
	
	var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);  	

	return new PagedResultDto<Product>{
 	  Items = products,
	  Page = page,
	  PageSize = pageSize,
	  TotalCount = totalCount,
	  TotalPages = totalPages
	 };
	}
	
	public async Task<Product> CreateAsync(CreateProductDto dto){
	var product = new Product(dto.Name, dto.Price);
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
	return product;
	}

	public async Task DeleteAsync(int id){
	var product = await _repository.GetByIdAsync(id);
	if(product == null){
	 throw new NotFoundException("Ürün bulunamadı");
	}
	await _repository.DeleteAsync(product);
	}
}
