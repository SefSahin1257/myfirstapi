using AutoMapper;
using ProductApi.Application.Dtos;
using ProductApi.Application.Mappings;
using ProductApi.Domain.Entities;
using Xunit;

namespace ProductApi.Tests;

public class ProductMappingTests
{
    [Fact]
    public void Product_To_ProductDto_MapsCorrectly()
    {
        //Arrange: AutoMapper yapılandırması oluştur.
        var configuration = new MapperConfiguration(
            cfg => cfg.AddProfile<ProductMappingProfile>(),
            Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance);

        var mapper = configuration.CreateMapper();

        var product = new Product("MacBook Pro", 65000)
        {
            Id = 1,
            CategoryId = 1,
            Category = new Category
            {
                Id = 1,
                Name = "Bilgisayar"
            }
        };
        
        //Act: Product nesnesini ProductDto'ya dönüştür.
        var dto = mapper.Map<ProductDto>(product);
        
        //Assert: Alanların doğru eşlendiğini kontrol et
        
        Assert.Equal(1, dto.Id);
        Assert.Equal("MacBook Pro", dto.Name);
        Assert.Equal(65000m, dto.Price);
        Assert.Equal(1, dto.CategoryId);
        Assert.Equal("Bilgisayar", dto.CategoryName);
    }
}