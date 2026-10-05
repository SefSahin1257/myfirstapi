public interface IAuthService{
 Task<UserDto> RegisterAsync(RegisterUserDto dto);
 Task<LoginResponseDto?> LoginAsync(LoginUserDto dto);
}
