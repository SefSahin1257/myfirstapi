using Microsoft.EntityFrameworkCore;
public class UserRepository : IUserRepository{
 private readonly AppDbContext _db;
 public UserRepository(AppDbContext db){
  _db = db;
 
 }
 
 public async Task<bool> UsernameExistsAsync(string username){
  return await _db.Users.AnyAsync(u => u.Username == username);
 }
 
 public async Task<User> AddAsync(User user){
  _db.Users.Add(user);
  await _db.SaveChangesAsync();
  return user;
 }

 public async Task<User?> GetByUsernameAsync(string username){
  return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
 }
}
