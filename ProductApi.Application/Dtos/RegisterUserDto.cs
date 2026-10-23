namespace ProductApi.Application.Dtos;
using System.ComponentModel.DataAnnotations;

public class RegisterUserDto{
 
 public required string Name {get; set;}
 
 public required string Username {get; set;}
 
 public required string Password{get; set;}
} 
