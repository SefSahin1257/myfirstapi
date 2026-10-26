using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProductApi.Domain.Entities;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Dtos;
using FluentValidation;
using AutoMapper;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase{
	private readonly IProductService _productService;
	private readonly IValidator<CreateProductDto> _createValidator;
	private readonly IValidator<UpdateProductDto> _updateValidator;
	private readonly IMapper _mapper;

	public ProductController(IProductService productService,
		IValidator<CreateProductDto> createValidator,
		IValidator<UpdateProductDto> updateValidator,
		IMapper mapper)
	{
	_productService = productService;
	_createValidator = createValidator;
	_updateValidator = updateValidator;
	_mapper = mapper;
	}
	
	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10, int? categoryId = null){
	var result = await _productService.GetAllAsync(page, pageSize, categoryId);
        
	var response = new PagedResultDto<ProductDto>
	 {
 	  Items = _mapper.Map<List<ProductDto>>(result.Items),
	  Page = result.Page,
	  PageSize = result.PageSize,
	  TotalCount = result.TotalCount,
	  TotalPages = result.TotalPages
	 };
	 
  	return Ok(response);
	}
	
	
	[HttpGet("{id}")]
	public async Task<IActionResult> GetProduct(int id){
	var product = await _productService.GetByIdAsync(id);
	return Ok(_mapper.Map<ProductDto>(product));	
	}
	
	
	[Authorize(Policy = "CanManageProducts")]
	[HttpPost]
	public async Task<IActionResult> CreateProduct(CreateProductDto dto)
	{
		var validationResult = await _createValidator.ValidateAsync(dto);

		if (!validationResult.IsValid)
		{
			foreach (var error in validationResult.Errors )
			{
				ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}

			return ValidationProblem(ModelState);
		}

		var product = await _productService.CreateAsync(dto);

		return CreatedAtAction(
			nameof(GetProduct), 
			new { id = product.Id }, 
			_mapper.Map<ProductDto>(product));
	}
	
	
	[Authorize(Policy = "CanManageProducts")]
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
	{
		var validationResult = await _updateValidator.ValidateAsync(dto);

		if (!validationResult.IsValid)
		{
			foreach (var error in validationResult.Errors)
			{
				ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}

			return ValidationProblem(ModelState);
		}

		var product = await _productService.UpdateAsync(id, dto);

		return Ok(_mapper.Map<ProductDto>(product));
	}	
	
	
	[Authorize(Policy = "CanManageProducts")]
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProduct(int id){
	 await _productService.DeleteAsync(id);
	 return NoContent();
	}
	
}
