
namespace Domain;

public interface IUserRepository
{
    public Task<bool> CreateUser(User user);
    public Task<bool> DeleteUser(Guid Id);
    public Task<bool> VerifyUserExists_WithEmail(string email);
    public Task<bool> VerifyUserExists_WithID(Guid ID);
    public Task<User> GetDataUser_WithID(Guid ID);
    public Task<bool> UpdateUser(User user);
}