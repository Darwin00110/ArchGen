namespace ArchGen.Application;

public interface IFileService
{
    public Task<bool> WriteContent(string Content, string PathFile);
    public Task<string> ObterHashAsync(string path); 
}
