var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

List<Product> products = new List<Product>();
                Product fproduct = new Product(1, "iPhone 17 Pro Max", 1400m );
                Product sproduct = new Product(2, "MacBook Air", 1000m);
                products.Add(fproduct);
                products.Add(sproduct);


app.MapGet("/products",() =>
	{	
		return products;
	}


);

app.MapGet("/products/{id}", (int id)=> 
	{
	Product? foundProduct = products.FirstOrDefault(n =>n.Id == id);
	if(foundProduct == null){
		return Results.NotFound("Product not found!");
	}

	return Results.Ok(foundProduct);
	
	}
);

app.MapPost("/products", (Product product)=>{
	products.Add(product);
	return Results.Ok(product);
	
});

app.MapDelete("/products/{id}", (int id) =>
	{
	Product? foundProduct = products.FirstOrDefault(p => p.Id == id);
	if(foundProduct == null){
		return Results.NotFound("Product not found");
	}
	products.Remove(foundProduct);
	return Results.NoContent();

});

app.MapPut("/products/{id}", (int id, Product updatedProduct)=>
	{
		Product? prd = products.FirstOrDefault(p => p.Id == id);
		if(prd == null){
			return Results.NotFound("Product not found");	
		}
		prd.Name = updatedProduct.Name;
		prd.Price = updatedProduct.Price;
		
		return Results.Ok(prd);
});

app.Run();
