using ProductApi.Application.Dtos;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Application.Exceptions;
namespace ProductApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
    {
        var usernameExists =
            await _userRepository.UsernameExistsAsync(dto.Username);

        if (usernameExists)
        {
            throw new ConflictException("Bu kullanıcı adı zaten kullanılıyor.");
        }

        var user = new User
        {
            Name = dto.Name,
            Username = dto.Username,
            PasswordHash = string.Empty
        };

        user.PasswordHash =
            _passwordService.HashPassword(user, dto.Password);

        await _userRepository.AddAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username
        };
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginUserDto dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);

        if (user == null)
            return null;

        var passwordIsValid =
            _passwordService.VerifyPassword(user, dto.Password);

        if (!passwordIsValid)
            return null;

        var loggedUser = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username
        };

        var token = _tokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            User = loggedUser
        };
    }
}