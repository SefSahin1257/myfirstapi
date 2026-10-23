using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using ProductApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

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
  // Arrange
  using var factory = new CustomWebApplicationFactory();
  using var client = factory.CreateClient();

  var newProduct = new
  {
   Name = "Test Laptop",
   Price = 25000m,
   CategoryId = 1
  };

  // Act - Ürünü oluştur
  var postResponse = await client.PostAsJsonAsync(
   "/api/product", newProduct);

  // Assert - Ürün oluşturuldu mu?
  var errorBody = await postResponse.Content.ReadAsStringAsync();

  Assert.True(
   postResponse.StatusCode == HttpStatusCode.Created,
   $"HTTP Status: {(int)postResponse.StatusCode}\nResponse: {errorBody}"
  );

  // Act - Ürünleri listele
  var getResponse = await client.GetAsync("/api/product");

  // Assert
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
   Price = 15000m,
   CategoryId = 1
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
 
 [Fact]
 public async Task Register_WithEmptyFields_ReturnsValidationErrors()
 {
  using var factory = new CustomWebApplicationFactory();
  using var client = factory.CreateClient();

  var response = await client.PostAsJsonAsync(
   "/api/Auth/register",
   new
   {
    Name = "",
    Username = "",
    Password = ""
   });

  Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

  var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

  Assert.NotNull(problem);

  Assert.Collection(
   problem.Errors["Name"],
   message => Assert.Equal("Ad boş olamaz.", message));

  Assert.Collection(
   problem.Errors["Username"],
   message => Assert.Equal("Kullanıcı adı boş olamaz.", message));

  Assert.Collection(
   problem.Errors["Password"],
   message => Assert.Equal("Şifre boş olamaz.", message));
 }
}
