namespace ProductApi.Application.Dtos;
using System.ComponentModel.DataAnnotations;

public class LoginResponseDto{
 public required string Token{get; set;}
 public required UserDto User{get; set;}
}
