public interface IUserRepository{
 Task<User> AddAsync(User user);
 Task<bool> UsernameExistsAsync(string username); 
 Task<User?> GetByUsernameAsync(string username);
}
