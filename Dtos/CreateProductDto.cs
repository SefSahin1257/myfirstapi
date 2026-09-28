using System.ComponentModel.DataAnnotations;
public class CreateProductDto{
	[Required]
	public required string Name{get; set;}
	[Range(0.01, double.MaxValue)]
	public decimal Price{get; set;}
}
