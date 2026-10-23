using System.ComponentModel.DataAnnotations;
namespace ProductApi.Application.Dtos;

public class CreateProductDto{
	
	public required string Name{get; set;}
	
	public decimal Price{get; set;}
	
	public int CategoryId { get; set; }
}
