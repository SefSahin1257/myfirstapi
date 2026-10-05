using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase{
 private readonly IAuthService _authService;

 public AuthController(IAuthService authService){
  _authService = authService; 
 } 

 [HttpPost("register")]
 public async Task<IActionResult>Register(RegisterUserDto dto){
  var registeredUser =  await _authService.RegisterAsync(dto);
  return StatusCode(StatusCodes.Status201Created, registeredUser);
 }

 [HttpPost("login")]
 public async Task<IActionResult> Login(LoginUserDto dto){
  var loggedUser = await _authService.LoginAsync(dto);
  if(loggedUser == null) return Unauthorized();
  return Ok(loggedUser);
 }
}
