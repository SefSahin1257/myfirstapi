using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => 
options.UseSqlite("Data source=products.db"));

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


app.MapGet("/products",(AppDbContext db) =>
	{	
		return db.Products.ToList();
	}


);

app.MapGet("/products/{id}", (int id, AppDbContext db)=> 
	{
	Product? foundProduct = db.Products.FirstOrDefault(n =>n.Id == id);
	if(foundProduct == null){
		return Results.NotFound("Product not found!");
	}

	return Results.Ok(foundProduct);
	
	}
);

app.MapPost("/products", (Product product, AppDbContext db)=>{
	db.Products.Add(product);
	db.SaveChanges();
	return Results.Ok(product);
	
});

app.MapDelete("/products/{id}", (int id, AppDbContext db) =>
	{
	Product? foundProduct = db.Products.FirstOrDefault(p => p.Id == id);
	if(foundProduct == null){
		return Results.NotFound("Product not found");
	}
	db.Products.Remove(foundProduct);
	db.SaveChanges();
	return Results.NoContent();

});

app.MapPut("/products/{id}", (int id,Product updatedProduct, AppDbContext db)=>
	{
		Product? prd = db.Products.FirstOrDefault(p => p.Id == id);
		if(prd == null){
			return Results.NotFound("Product not found");	
		}
		prd.Name = updatedProduct.Name;
		prd.Price = updatedProduct.Price;
		db.SaveChanges();
		return Results.Ok(prd);
});

app.Run();
