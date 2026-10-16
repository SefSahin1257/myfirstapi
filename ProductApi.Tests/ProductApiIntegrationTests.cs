using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

public class ProductApiIntegrationTests{
 
 [Fact]
 public async Task GetProducts_WithAuthenticatedUser_ReturnSuccess(){
  
  //Arrange
  using var factory = new CustomWebApplicationFactory();
  using var client = factory.CreateClient();
  
  //Act
  var response = await client.GetAsync("/api/product");
  
  //Assert
  Assert.True(response.IsSuccessStatusCode,
	$"HTTP Status: {(int)response.StatusCode} - {response.StatusCode}");
 }

 [Fact]
 public async Task GetProducts_WithoutToken_ReturnsUnauthorized(){
  //Arrange
  using var factory = new WebApplicationFactory<Program>();
  using var client = factory.CreateClient();
  
  //Act
  var response = await client.GetAsync("/api/product");

  //Assert 
  Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
 }

 [Fact]
 public async Task CreateProduct_ThenGetProducts_ReturnsCreatedProduct()
 {
  using var factory = new CustomWebApplicationFactory();
  using var client = factory.CreateClient();

  var newProduct = new
  {
   Name = "Test Laptop",
   Price = 25000m
  };
  //Act - Ürünü oluştur
  var postResponse = await client.PostAsJsonAsync("/api/product", newProduct);
  
  //Assert - Ürün oluşturuldu mu? kontrolü yapılır
  Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
  
  //Act - Ürünleri listele
  var getResponse = await client.GetAsync("/api/product");
  
  //Assert - Listeleme başarılı mı?
  getResponse.EnsureSuccessStatusCode();

  var responseBody = await getResponse.Content.ReadAsStringAsync();

  Assert.Contains("Test Laptop", responseBody);
 }

 [Fact]
 public async Task CreateProduct_WithoutPermission_ReturnsForbidden()
 {
  //Arrange
  using var factory = new CustomWebApplicationFactory();
  using var client = factory.CreateClient();
  
  client.DefaultRequestHeaders.Add("X-Test-Role", "User");

  var newProduct = new
  {
   Name = "Test Telefon",
   Price = 15000m
  };
  
  //Act
  var response = await client.PostAsJsonAsync("/api/product", newProduct);
  
  //Assert
  Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
 }

 [Fact]
 public async Task CreateProduct_WithNegativePrice_ReturnsBadReques()
 {
  //Arrange
  using var factory = new CustomWebApplicationFactory();
  using var client = factory.CreateClient();

  var invalidPrice = new
  {
   Name = "Test Laptop",
   Price = -500m
  };

  var response = await client.PostAsJsonAsync("/api/product",invalidPrice);
  
  Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
 }
}
