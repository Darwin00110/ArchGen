namespace ArchGen.Domain;

public interface IArchGenRepository
{
    public Task<bool> VerifyExists_OnThe_Databank(string NomeProjeto);
}
