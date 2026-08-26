namespace ArchGen.Application;

public interface IArchGenService
{
    public void SetConfiguracaoDoProjeto(string nomeDoProjeto, string TipoProjeto);
    public Task<bool> VerifyInternStructure_Diretory(string path);
    public Task<bool> ExecInternStructure(string Path);
}

