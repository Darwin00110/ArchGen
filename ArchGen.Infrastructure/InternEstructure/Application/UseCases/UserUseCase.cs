
namespace Application;
using Domain;
public class UserUseCase : IUserUseCase
{
    private readonly IUserRepository _repo;
    public UserUseCase(IUserRepository repo)
    {
        _repo = repo;
    }
    public async Task<bool> CreateUser(CreateUserRequest request)
    {
        var verifyUser = await _repo.VerifyUserExists_WithEmail(request.Email);
        if (verifyUser)
        {
            throw new UseCaseException('Usuario ja existe.');
        }
        var DataUser = new User
        {
            ID = Guid.NewGuid(),
            Email = request.Email,
            Nome = request.Nome,
            Telefone = request.Telefone ?? throw new UseCaseException('Telefone é obrigatório'),
            Senha = request.Senha
        };
        DataUser.Validate_Nome();
        DataUser.Validate_Telefone();
        DataUser.Validate_Email();
        var result = await _repo.CreateUser(DataUser);
        return result;
    }
    public async Task<ReadUserResponse> ReadUser(Guid ID)
    {
        var verifyUser = await _repo.VerifyUserExists_WithID(ID);
        if (!verifyUser)
        {
            throw new UseCaseException('Usuario não existe.');
        }
        var GetData = await _repo.GetDataUser_WithID(ID);
        return new ReadUserResponse
        {
          Email = GetData.Email,
          Nome = GetData.Nome,
          Telefone = GetData.Telefone  
        };
    }
    public async Task<bool> UpdateUser(Guid ID, UpdateUserRequest request)
    {
        var verifyUser = await _repo.VerifyUserExists_WithID(ID);
        if (!verifyUser)
        {
            throw new UseCaseException('Usuario não existe.');
        }
        var DataUser = new User
        {
          Email = request.Email,
          Nome = request.Nome,
          Telefone = request.Telefone,
          Senha = request.Senha  
        };
        DataUser.Validate_Email();
        DataUser.Validate_Nome();
        DataUser.Validate_Telefone();
        var result = await _repo.UpdateUser(DataUser);
        return result;
    }
    public async Task<bool> DeleteUser(Guid ID)
    {
        var verifyUser = await _repo.VerifyUserExists_WithID(ID);
        if (!verifyUser)
        {
            throw new UseCaseException('Usuario não existe.');
        }
        var result = await _repo.DeleteUser(ID);
        return result;
    }
    public async Task<string> Exec()
    {
        return 'Ta funcionando.';
    }
    
}
