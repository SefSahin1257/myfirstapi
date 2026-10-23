using System.ComponentModel.DataAnnotations;
namespace ProductApi.Application.Dtos;

public class CreateProductDto{
	[Required]
	public required string Name{get; set;}
	[Range(0.01, double.MaxValue)]
	public decimal Price{get; set;}

	[Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçmelisiniz.")]
	public int CategoryId { get; set; }
}
