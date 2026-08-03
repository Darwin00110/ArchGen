using ArchGen.Application;
using ArchGen.Infrastructure;

namespace ArchGen.Tests;

public class UserUseCaseTests
{
    [Fact]
    public async Task CreateDomain_Deve_Criar_Estrutura_Basica_Do_Dominio()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), $"archgen-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempRoot);

        var previousCurrentDirectory = Environment.CurrentDirectory;
        Environment.CurrentDirectory = tempRoot;

        try
        {
            var terminal = new FakeTerminalService();
            var fileService = new FileService();
            var useCase = new ArchGenUseCase(terminal, fileService);

            var result = await useCase.CreateDomain("ProjetoTeste");

            Assert.True(result);
            Assert.True(File.Exists(Path.Combine(tempRoot, "ProjetoTeste.Domain", "Entities", "User.cs")));
            Assert.True(File.Exists(Path.Combine(tempRoot, "ProjetoTeste.Domain", "Exceptions", "DomainException.cs")));
            Assert.True(File.Exists(Path.Combine(tempRoot, "ProjetoTeste.Domain", "Enums", "OptionsStatusUsuario.cs")));
        }
        finally
        {
            Environment.CurrentDirectory = previousCurrentDirectory;
            if (Directory.Exists(tempRoot))
            {
                Directory.Delete(tempRoot, recursive: true);
            }
        }
    }

    private sealed class FakeTerminalService : ITerminalService
    {
        public Task<ResultadoComandoResponse> ExecutarComando(string[] Comandos, string diretorioTrabalho = "")
        {
            var projectName = Comandos.Length > 0 ? Comandos[^1] : "ProjetoTeste.Domain";
            var root = string.IsNullOrWhiteSpace(diretorioTrabalho) ? Environment.CurrentDirectory : diretorioTrabalho;
            Directory.CreateDirectory(Path.Combine(root, projectName));

            return Task.FromResult(new ResultadoComandoResponse
            {
                Saida = "ok",
                Error = string.Empty
            });
        }

        public Task<bool> CreateFolder(string NomeDaPasta)
        {
            Directory.CreateDirectory(NomeDaPasta);
            return Task.FromResult(true);
        }
    }
}
