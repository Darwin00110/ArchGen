namespace ArchGen.Application;

public interface IArchGenService
{
    public Task<bool> VerifySoluctionFiles();
    public void SetConfiguracaoDoProjeto(string nomeDoProjeto, string TipoProjeto);
    public Task<bool> VerifyInternTestsFiles();
    public Task<bool> VerifyInternConsoleFiles();
    public Task<bool> VerifyInternAPIFiles();
    public Task<bool> VerifyInternStructure_Diretory();
    public Task<bool> ExecInternStructure();
}

