using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration["Jwt:Key"];
if(jwtKey == null){
 throw new InvalidOperationException("JWT key bulunamadı.");
}
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   	.AddJwtBearer(options => 
	{
	 options.TokenValidationParameters = new TokenValidationParameters
	{
	 ValidateIssuer = true,
	 ValidIssuer = builder.Configuration["Jwt:Issuer"],
	 
	 ValidateAudience = true,
	 ValidAudience = builder.Configuration["Jwt:Audience"],

	 ValidateIssuerSigningKey = true,

	 IssuerSigningKey = new SymmetricSecurityKey(
		 Encoding.UTF8.GetBytes(jwtKey)
	  ),

	 ValidateLifetime = true
	 };
	});
builder.Services.AddDbContext<AppDbContext>(options => 
options.UseSqlite(
	builder.Configuration.GetConnectionString("DefaultConnection")
	)
);
builder.Services.AddAuthorization(options => {
  options.AddPolicy("AdminOnly", policy => {
 	policy.RequireRole("Admin");
   });
  
  options.AddPolicy("CanManageProducts", policy => {
   policy.RequireRole("Admin");
   policy.RequireClaim("Permission", "ManageProducts");
  });
 });
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<PasswordHasher<User>>();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
