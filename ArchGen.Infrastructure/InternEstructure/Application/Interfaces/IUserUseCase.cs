
namespace Application;

public interface IUserUseCase
{
    public Task<bool> CreateUser(CreateUserRequest request);
    public Task<ReadUserResponse> ReadUser(Guid ID);
    public Task<bool> UpdateUser(Guid ID, UpdateUserRequest request);
    public Task<bool> DeleteUser(Guid ID);
    public Task<string> Exec();
}