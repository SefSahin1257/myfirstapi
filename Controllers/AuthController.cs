using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Dtos;
using FluentValidation;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase{
 private readonly IAuthService _authService;
 private readonly IValidator<RegisterUserDto> _registerValidtor;
 private readonly IValidator<LoginUserDto> _loginValidator;

 public AuthController(IAuthService authService, 
  IValidator<RegisterUserDto> registerValidator, 
  IValidator<LoginUserDto> loginValidator)
 {
  _authService = authService;
  _registerValidtor = registerValidator;
  _loginValidator = loginValidator;
 } 

 [HttpPost("register")]
 public async Task<IActionResult>Register(RegisterUserDto dto)
 {
  var validationResult = await _registerValidtor.ValidateAsync(dto);

  if (!validationResult.IsValid)
  {
   foreach (var error in validationResult.Errors)
   {
    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
   }
   return ValidationProblem(ModelState);
  }
  
  var registeredUser =  await _authService.RegisterAsync(dto);
  
  return StatusCode(StatusCodes.Status201Created, registeredUser);
  
 }

 [HttpPost("login")]
 public async Task<IActionResult> Login(LoginUserDto dto)
 {
  var validationResult = await _loginValidator.ValidateAsync(dto);

  if (!validationResult.IsValid)
  {
   foreach (var error in validationResult.Errors)
   {
    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
   }

   return ValidationProblem(ModelState);
  }
  
  var loggedUser = await _authService.LoginAsync(dto);
  
  if(loggedUser == null) return Unauthorized();
  return Ok(loggedUser);
 }

 [Authorize]
 [HttpGet("me")]
 public IActionResult Me(){
  var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
  var username = User.FindFirst(ClaimTypes.Name)?.Value;
  var role = User.FindFirst(ClaimTypes.Role)?.Value;
  var permission = User.FindFirst("Permission")?.Value;
  return Ok(new {
    userId, username, role, permission
   }
  );
 }
}
