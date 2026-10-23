namespace ProductApi.Application.Dtos;
using System.ComponentModel.DataAnnotations;

public class ProductDto{
	public int Id{get; set;}
	public required string Name{get; set;}
	public decimal Price{get; set;}

	public int CategoryId { get; set; }

	public string CategoryName { get; set; } = string.Empty;
}
