namespace ArchGen.Application;

public interface IInternSolutionGenerationService
{
    public Task SetPathInternSolution(string path);
    public Task SetNomeProjeto(string nomeProjeto = "");
    public void SetTipoDoProjeto(string tipodoprojeto);
    public Task VerifyFileSolution();
    public Task<bool> CreateSoluctionFiles();
}
