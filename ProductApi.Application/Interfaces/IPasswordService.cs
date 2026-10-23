using ProductApi.Domain.Entities;
namespace ProductApi.Application.Interfaces;

public interface IPasswordService
{
    string HashPassword(User user, string password);
    
    bool VerifyPassword(User user, string password);
}