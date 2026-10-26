using Microsoft.Extensions.Logging;
using Moq; 
using ProductApi.Domain.Entities;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Services;
using ProductApi.Application.Dtos; 
using ProductApi.Application.Exceptions;

namespace ProductApi.Tests;


public class ProductServiceTests
{
    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, 101)]
    public async Task GetAllAsync_InvalidPagination_ThrowsArgumentException(int page, int pageSize)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        var loggerMock = new Mock<ILogger<ProductService>>();
        var cacheMock = new Mock<ICacheService>();

        var service = new ProductService(
            repositoryMock.Object,
            loggerMock.Object,
            cacheMock.Object
        );

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.GetAllAsync(page, pageSize, null)
        );
    }	    

    [Fact]
    public async Task GetAllAsync_ValidPage_ReturnsPagedResult()
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        var loggerMock = new Mock<ILogger<ProductService>>();
        var cacheMock = new Mock<ICacheService>();

	var products = new List<Product>
	{
  	 new Product("Laptop", 30000),
	 new Product("Mouse", 1000)
	};
  
 	repositoryMock
		.Setup(r => r.GetAllAsync(1,10, null))
	 	.ReturnsAsync(products);
	
	repositoryMock
		.Setup(r => r.CountAsync(null))	
		.ReturnsAsync(2);
	
        var service = new ProductService(
            repositoryMock.Object,
            loggerMock.Object,
            cacheMock.Object
        );
	// Act + Assert
	var result = await service.GetAllAsync(1,10, null);
	
	//Assert
	Assert.Equal(2, result.Items.Count);
	Assert.Equal(1, result.Page);
 	Assert.Equal(10, result.PageSize);
	Assert.Equal(2, result.TotalCount);
	Assert.Equal(1, result.TotalPages);

	repositoryMock.Verify(
  		  r => r.GetAllAsync(1, 10, null),
		  Times.Once
		 );

	repositoryMock.Verify(
    		r => r.CountAsync(null),
    		Times.Once
		);	
    }

    [Fact]
    public async Task DeleteAsync_ProductNotFound_ThrowsNotFoundException(){
     var repositoryMock = new Mock<IProductRepository>();
     var loggerMock = new Mock<ILogger<ProductService>>();
     var cacheMock = new Mock<ICacheService>();
     
     repositoryMock
	.Setup(r => r.GetByIdAsync(999))
	.ReturnsAsync((Product?)null);

     var service = new ProductService(
	 repositoryMock.Object,
	 loggerMock.Object,
	 cacheMock.Object);

     
    //Act + Assert
    await Assert.ThrowsAsync<NotFoundException>(
	 () => service.DeleteAsync(999));
    repositoryMock.Verify(
	 r => r.DeleteAsync(It.IsAny<Product>()),
	 Times.Never);
    }

   [Fact]
   public async Task DeleteAsync_ProductExists_DeletesProduct(){
    
    var repositoryMock = new Mock<IProductRepository>();
    var loggerMock = new Mock<ILogger<ProductService>>();
    var cacheMock = new Mock<ICacheService>();

    var product = new Product("Laptop", 30000);
    
    repositoryMock
	.Setup(r => r.GetByIdAsync(1))
	.ReturnsAsync(product);
    
    var service = new ProductService(
	 repositoryMock.Object,
	 loggerMock.Object,
	 cacheMock.Object
	);

    await service.DeleteAsync(1);
    repositoryMock.Verify(
	 r => r.DeleteAsync(product),
	 Times.Once);
   }

   [Fact]
   public async Task GetByIdAsync_ProductExists_ReturnsProduct(){
    var repositoryMock = new Mock<IProductRepository>();
    var loggerMock = new Mock<ILogger<ProductService>>();
    var cacheMock = new Mock<ICacheService>();

    var product = new Product("Laptop", 30000);

    repositoryMock
	.Setup(r => r.GetByIdAsync(1))
 	.ReturnsAsync(product);

    var service = new ProductService(
	 repositoryMock.Object,
	 loggerMock.Object,
	 cacheMock.Object
	 );
   
    var result = await service.GetByIdAsync(1);

    Assert.Same(product, result);
  
    repositoryMock.Verify(
 	 r => r.GetByIdAsync(1),
	 Times.Once);
   }

  [Fact]
  public async Task GetByIdAsync_ProductNotFound_ThrowsNotFoundException(){

   var repositoryMock = new Mock<IProductRepository>();
   var loggerMock = new Mock<ILogger<ProductService>>();
   var cacheMock = new Mock<ICacheService>();

   repositoryMock
	.Setup(r => r.GetByIdAsync(999))
  	.ReturnsAsync((Product?)null);
   
   var service = new ProductService(
   	repositoryMock.Object,
	loggerMock.Object,
    cacheMock.Object
    );

  await Assert.ThrowsAsync<NotFoundException>(
	() => service.GetByIdAsync(999));

  repositoryMock.Verify(
	r => r.GetByIdAsync(999),
	Times.Once);
 }

 [Fact]
 public async Task CreateAsync_ValidDto_CreatesProduct(){
  var repositoryMock = new Mock<IProductRepository>();
  var loggerMock = new Mock<ILogger<ProductService>>();
  var cacheMock = new Mock<ICacheService>();

  var dto = new CreateProductDto{
    Name = "Laptop",	
    Price = 30000
   };

  repositoryMock
	.Setup(r => r.AddAsync(It.IsAny<Product>()))
	.ReturnsAsync((Product p) => p);

  var service = new ProductService(
	repositoryMock.Object,
	loggerMock.Object,
	cacheMock.Object
	);

  var result = await service.CreateAsync(dto);

  Assert.Equal("Laptop", result.Name);
  Assert.Equal(30000, result.Price);

  repositoryMock.Verify(
	r => r.AddAsync(
	 It.Is<Product>(p => p.Name == "Laptop" && p.Price == 30000)
	), Times.Once);
 }  

 [Fact]
 public async Task UpdateAsync_ProductNotFound_ThrowsNotFoundException(){
  var repositoryMock = new Mock<IProductRepository>();
  var loggerMock = new Mock<ILogger<ProductService>>();
  var cacheMock = new Mock<ICacheService>();

  var dto = new UpdateProductDto{
   Name = "Yeni Laptop",
   Price = 35000
  };
   
  repositoryMock
	.Setup(r => r.GetByIdAsync(999))
	.ReturnsAsync((Product?)null);

  var service = new ProductService(
	repositoryMock.Object,
	loggerMock.Object,
	cacheMock.Object
	);

 await Assert.ThrowsAsync<NotFoundException>(
	() => service.UpdateAsync(999, dto));

 repositoryMock.Verify(
	r => r.UpdateAsync(It.IsAny<Product>()),
	Times.Never);
 }

 [Fact]
 public async Task UpdateAsync_ProductExists_UpdatesProduct(){
  var repositoryMock = new Mock<IProductRepository>();
  var loggerMock = new Mock<ILogger<ProductService>>();
  var cacheMock = new Mock<ICacheService>();

  var product = new Product("Laptop", 30000);
  
  var dto = new UpdateProductDto{
	Name = "MacBook",
 	Price = 45000
  };

  repositoryMock
	.Setup(r => r.GetByIdAsync(1))
	.ReturnsAsync(product);

  var service = new ProductService(
	repositoryMock.Object,
	loggerMock.Object,
	cacheMock.Object
	);

  var result = await service.UpdateAsync(1, dto);

  Assert.Equal("MacBook", result.Name);
  Assert.Equal(45000, result.Price);
  Assert.Same(product, result);

  repositoryMock.Verify(
	r => r.UpdateAsync(product),
	Times.Once);
 }

 [Fact]
 public async Task GetAllAsync_WithCategoryId_FiltersProducts()
 {
	 var repositoryMock = new Mock<IProductRepository>();
	 var loggerMock = new Mock<ILogger<ProductService>>();
	 var cacheMock = new Mock<ICacheService>();

	 var products = new List<Product>
	 {
		 new Product("iPhone 16", 55000) { CategoryId = 2 },
		 new Product("Samsung S24", 40000) { CategoryId = 2 }

	 };

	 repositoryMock
		 .Setup(r => r.GetAllAsync(1, 10,2))
		 .ReturnsAsync(products);

	 repositoryMock
		 .Setup(r => r.CountAsync(2))
		 .ReturnsAsync(2);

	 var service = new ProductService(
		 repositoryMock.Object,
		 loggerMock.Object,
		 cacheMock.Object
		 );

	 var result = await service.GetAllAsync(1, 10, 2);
	 
	 Assert.Equal(2, result.Items.Count);
	 Assert.Equal(2, result.TotalCount);
	 Assert.All(result.Items, p => Assert.Equal(2, p.CategoryId));

	 repositoryMock.Verify(
		 r => r.GetAllAsync(1,10,2), Times.Once);
	 
	 repositoryMock.Verify(
		 r => r.CountAsync(2), Times.Once);
 }

 [Fact]
 public async Task GetByIdAsync_CacheMiss_GetsFromRepository()
 {
	 //Arrange
	 var repositoryMock = new Mock<IProductRepository>();
	 var loggerMock = new Mock<ILogger<ProductService>>();
	 var cacheMock = new Mock<ICacheService>();

	 var product = new Product("MacBook Pro", 65000)
	 {
		 Id = 1,
		 CategoryId = 1
	 };
	 
	 //Cache boş: Get<Product> null dönecek
	 cacheMock
		 .Setup(c => c.Get<Product>("product_1"))
		 .Returns((Product?)null);

	 repositoryMock
		 .Setup(r => r.GetByIdAsync(1))
		 .ReturnsAsync(product);

	 var service = new ProductService(
		 repositoryMock.Object,
		 loggerMock.Object,
		 cacheMock.Object);
	 
	 //Act
	 var result = await service.GetByIdAsync(1);
	 
	 //Assert
	 Assert.Equal("MacBook Pro", result.Name);
	 
	 repositoryMock
		 .Verify(r => r.GetByIdAsync(1),
			 Times.Once);
	 
	 cacheMock
		 .Verify(c => c.Set(
			 "product_1",
			 product,
			 TimeSpan.FromMinutes(5)),
			 Times.Once);
	}

 [Fact]
 public async Task GetByIdAsync_CacheHit_DoeNotCallRepository()
 {
	 var repositoryMock = new Mock<IProductRepository>();
	 var loggerMock = new Mock<ILogger<ProductService>>();
	 var cacheMock = new Mock<ICacheService>();

	 var cachedProduct = new Product("MacBook Pro", 65000)
	 {
		 Id = 1,
		 CategoryId = 1
	 };
	 
	 //Ürün cache'te var
	 cacheMock
		 .Setup(r => r.Get<Product>("product_1"))
		 .Returns(cachedProduct);

	 var service = new ProductService(
		 repositoryMock.Object,
		 loggerMock.Object,
		 cacheMock.Object);
	 
	 //Act
	 var result = await service.GetByIdAsync(1);
	 
	 //Assert
	 Assert.Equal("MacBook Pro", result.Name);
	 
	 repositoryMock
		 .Verify(r => r.GetByIdAsync(It.IsAny<int>()),
			 Times.Never);

 }

 [Fact]
 public async Task UpdateAsync_RemoveProductFromCache()
 {
	 //Arrange
	 var repositoryMock = new Mock<IProductRepository>();
	 var loggerMock = new Mock<ILogger<ProductService>>();
	 var cacheMock = new Mock<ICacheService>();

	 var product = new Product("MacBook Pro", 65000)
	 {
		 Id = 1,
		 CategoryId = 1
	 };

	 var dto = new UpdateProductDto
	 {
		Name = "MacBook Pro M4",
		Price = 70000
	 };

	 repositoryMock
		 .Setup(r => r.GetByIdAsync(1))
		 .ReturnsAsync(product);

	 var service = new ProductService(
		 repositoryMock.Object,
		 loggerMock.Object,
		 cacheMock.Object
		 );
	 
	 // Act
	 await service.UpdateAsync(1, dto);
	 
	 // Assert
	 repositoryMock
		 .Verify(r => r.UpdateAsync(product),
			 Times.Once);
	 
	 cacheMock
		 .Verify(c => c.Remove("product_1"),
			 Times.Once);
 }

 [Fact]
 public async Task DeleteAsync_RemovesProductFromCache()
 {
	 // Arrange
	 var repositoryMock = new Mock<IProductRepository>();
	 var loggerMock = new Mock<ILogger<ProductService>>();
	 var cacheMock = new Mock<ICacheService>();

	 var product = new Product("MacBook Pro", 65000)
	 {
		Id = 1,
		CategoryId = 1
	 };

	 repositoryMock
		 .Setup(r => r.GetByIdAsync(1))
		 .ReturnsAsync(product);

	 var service = new ProductService(
		 repositoryMock.Object,
		 loggerMock.Object,
		 cacheMock.Object
		 );
	 
	 // Act
	 await service.DeleteAsync(1);
	 
	 // Assert
	 repositoryMock
		 .Verify(r => r.DeleteAsync(product),
			 Times.Once);
	 
	 cacheMock
		 .Verify(c => c.Remove("product_1"),
			 Times.Once);

 }
}

