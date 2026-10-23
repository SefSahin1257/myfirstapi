namespace ProductApi.Application.Dtos;
using System.ComponentModel.DataAnnotations;
public class UserDto{
 public int Id {get; set;}
 public required string Name {get; set;}
 public required string Username {get; set;}
}
