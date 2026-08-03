namespace ArchGen.Application;

public interface IArchGenUseCase
{
    public Task<ResultadoComandoResponse> Exec(CreateCleanArchRequest request);
    public Task<bool> CreateDomain(string NomeProjeto);
}
