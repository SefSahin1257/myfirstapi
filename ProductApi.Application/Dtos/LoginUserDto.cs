using System.ComponentModel.DataAnnotations;
namespace ProductApi.Application.Dtos;

public class LoginUserDto{

 public required string Username{get; set;}
 
 public required string Password{get; set;}
}
