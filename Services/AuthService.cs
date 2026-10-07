using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthService : IAuthService{
 private readonly IUserRepository _userRepository;
 private readonly PasswordHasher<User> _passwordHasher; 
 private readonly IConfiguration _configuration;

 public AuthService(IUserRepository userRepository, PasswordHasher<User> passwordHasher, IConfiguration configuration){
  _userRepository = userRepository;
  _passwordHasher = passwordHasher;
  _configuration = configuration;
 }
 public async Task<UserDto> RegisterAsync(RegisterUserDto dto){
  var usernameExists = await _userRepository.UsernameExistsAsync(dto.Username);
 
  if(usernameExists){
   throw new ConflictException("Bu kullanıcı adı zaten kullanılıyor.");
  }
  var user = new User{
    Name = dto.Name,
    Username = dto.Username,
    PasswordHash = string.Empty
   };
   
  user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
  await _userRepository.AddAsync(user);
  
  var registeredUser = new UserDto{
   Id = user.Id,
   Name = user.Name,
   Username = user.Username
  };
  return registeredUser;

  }

  public async Task<LoginResponseDto?> LoginAsync(LoginUserDto dto){
  var user = await _userRepository.GetByUsernameAsync(dto.Username);
  if(user == null) return null;
  var result = _passwordHasher.VerifyHashedPassword(
   user,
   user.PasswordHash,
   dto.Password   
  ); 
  if(result == PasswordVerificationResult.Failed){
   return null;
  }
  var loggedUser = new UserDto{
   Id = user.Id,
   Name = user.Name,
   Username = user.Username
  };
   
  var token = GenerateJwtToken(user);   
  
  return new LoginResponseDto{
   Token = token,
   User = loggedUser
  };
 }

 private string GenerateJwtToken(User user){  
  var claims = new List<Claim>{
   new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
   new Claim(ClaimTypes.Name, user.Username),
   new Claim(ClaimTypes.Role, user.Role)
  };
  if(user.Role == "Admin"){
   // claims.Add(new Claim("Permission", "ManageProducts"));}

   var claim = new Claim("Permission", "ManageProducts");
   claims.Add(claim);
  }
 
  var jwtKey = _configuration["Jwt:Key"];
  if(jwtKey == null){
   throw new InvalidOperationException("JWT Key bulunamadı.");
  }
   
  var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
  var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

  var token = new JwtSecurityToken(
   issuer: _configuration["Jwt:Issuer"],
   audience: _configuration["Jwt:Audience"],
   claims: claims,
   expires: DateTime.UtcNow.AddHours(1),
   signingCredentials: credentials
  );
  
  return new JwtSecurityTokenHandler().WriteToken(token);  

 }

}
