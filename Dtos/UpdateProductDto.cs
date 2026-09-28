using System.ComponentModel.DataAnnotations;
public class UpdateProductDto{
	[Required]
	public required string Name{get; set;}
	[Range(0.01, double.MaxValue)]
	public decimal Price{get; set;}
}
