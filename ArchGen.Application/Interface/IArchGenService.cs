namespace ArchGen.Application;

public interface IArchGenService
{
    public Task VerifyDomainFiles();
    public Task VerifyApplicationFiles();
    public Task VerifyInfrastructureFiles();
    public Task VerifyTestsFiles();
    public Task VerifySoluctionFiles();
    public void SetConfiguracaoDoProjeto(string nomeDoProjeto, string TipoProjeto);
    public Task VerifyInternDomainFiles();
    public Task VerifyInternApplicationFiles();
    public Task VerifyInternInfrastructureFiles();
    public Task VerifyInternTestsFiles();
}

