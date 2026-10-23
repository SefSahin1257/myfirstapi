using System.ComponentModel.DataAnnotations;
namespace ProductApi.Application.Dtos;

public class LoginUserDto{
 [Required]
 public required string Username{get; set;}
 [Required]
 [StringLength(20, MinimumLength = 6)]
 public required string Password{get; set;}
}
