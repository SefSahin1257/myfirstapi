namespace ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
public interface ITokenService
{
    string GenerateToken(User user);
}