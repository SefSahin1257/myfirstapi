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


app.MapGet("/products",async(AppDbContext db) =>
	{	
		return await db.Products.ToListAsync();
	}


);

app.MapGet("/products/{id}", async(int id, AppDbContext db)=> 
	{
	Product? foundProduct = await db.Products.FirstOrDefaultAsync(n =>n.Id == id);
	if(foundProduct == null){
		return Results.NotFound("Product not found!");
	}

	return Results.Ok(foundProduct);
	
	}
);

app.MapPost("/products", async(Product product, AppDbContext db)=>{
	db.Products.Add(product);
	await db.SaveChangesAsync();
	return Results.Created($"/products/{product.Id}",product);
	
});

app.MapDelete("/products/{id}",async (int id, AppDbContext db) =>
	{
	Product? foundProduct = await db.Products.FindAsync(id);
	if(foundProduct == null){
		return Results.NotFound("Product not found");
	}
	db.Products.Remove(foundProduct);
	await db.SaveChangesAsync();
	return Results.NoContent();

});

app.MapPut("/products/{id}", async(int id,Product updatedProduct, AppDbContext db)=>
	{
		Product? prd = await db.Products.FindAsync(id);
		if(prd == null){
			return Results.NotFound("Product not found");	
		}
		prd.Name = updatedProduct.Name;
		prd.Price = updatedProduct.Price;
		await db.SaveChangesAsync();
		return Results.Ok(prd);
});

app.Run();
