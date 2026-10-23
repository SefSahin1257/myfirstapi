using ProductApi.Domain.Entities;
namespace ProductApi.Application.Interfaces;
public interface IUserRepository{
 Task<User> AddAsync(User user);
 Task<bool> UsernameExistsAsync(string username); 
 Task<User?> GetByUsernameAsync(string username);
}
