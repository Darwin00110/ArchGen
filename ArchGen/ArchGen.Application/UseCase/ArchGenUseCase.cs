using ArchGen.Domain;

namespace ArchGen.Application;

public class ArchGenUseCase : IArchGenUseCase
{
    private readonly ITerminalService _terminal;
    private readonly IFileService _file;

    public ArchGenUseCase(ITerminalService terminal, IFileService fileService)
    {
        _terminal = terminal;
        _file = fileService;
    }

    public async Task<ResultadoComandoResponse> Exec(CreateCleanArchRequest request)
    {
        var created = await CreateDomain(request.NomeDoProjeto);

        return new ResultadoComandoResponse
        {
            Error = created ? string.Empty : "Falha ao criar a camada Domain.",
            Saida = created ? $"Camada Domain criada para {request.NomeDoProjeto}." : string.Empty
        };
    }

    public async Task<bool> CreateDomain(string NomeProjeto)
    {
        var pathDomain = Path.Combine(Environment.CurrentDirectory, $"{NomeProjeto}.Domain");
        if (!Path.Exists(pathDomain))
        {
            var createFolder = await _terminal.ExecutarComando(["dotnet", "new", "classlib", "-n", $"{NomeProjeto}.Domain"], Environment.CurrentDirectory);
            if (string.IsNullOrWhiteSpace(createFolder.Saida) && !string.IsNullOrWhiteSpace(createFolder.Error))
            {
                throw new UseCaseException($"Falha ao criar a camada Domain, tente novamente mais tarde, Error: {createFolder.Error}");
            }
        }

        var pathEntities = Path.Combine(pathDomain, "Entities");
        var pathExceptions = Path.Combine(pathDomain, "Exceptions");
        var pathEnums = Path.Combine(pathDomain, "Enums");
        var pathInterfaces = Path.Combine(pathDomain, "Interfaces");

        await _terminal.CreateFolder(pathEntities);
        await _terminal.CreateFolder(pathExceptions);
        await _terminal.CreateFolder(pathEnums);
        await _terminal.CreateFolder(pathInterfaces);

        var userContent = $@"using {NomeProjeto}.Domain.Exceptions;

namespace {NomeProjeto}.Domain.Entities;

public class User
{{
    public Guid ID {{ get; set; }}
    public string Nome {{ get; set; }} = string.Empty;
    public string Email {{ get; set; }} = string.Empty;
    public string Telefone {{ get; set; }} = string.Empty;

    public void ValidateNome()
    {{
        if (string.IsNullOrWhiteSpace(Nome))
        {{
            throw new DomainException(""Nome não pode estar vazio, campo obrigatorio."");
        }}
    }}

    public void ValidateEmail()
    {{
        if (string.IsNullOrWhiteSpace(Email))
        {{
            throw new DomainException(""Email não pode estar vazio, campo obrigatorio."");
        }}

        if (!Email.Contains(""@gmail.com"", StringComparison.OrdinalIgnoreCase))
        {{
            throw new DomainException(""Formato Invalido do Email, ex: (exemplo@gmail.com)"");
        }}
    }}

    public void ValidateTelefone()
    {{
        if (string.IsNullOrWhiteSpace(Telefone))
        {{
            throw new DomainException(""Telefone não pode estar vazio, campo obrigatorio."");
        }}
    }}
}}
";

        var domainExceptionContent = $@"namespace {NomeProjeto}.Domain.Exceptions;

public class DomainException : Exception
{{
    public DomainException(string message) : base(message)
    {{
    }}
}}
";

        var serviceExceptionContent = $@"namespace {NomeProjeto}.Domain.Exceptions;

public class ServiceException : Exception
{{
    public ServiceException(string message) : base(message)
    {{
    }}
}}
";

        var useCaseExceptionContent = $@"namespace {NomeProjeto}.Domain.Exceptions;

public class UseCaseException : Exception
{{
    public UseCaseException(string message) : base(message)
    {{
    }}
}}
";

        var infraExceptionContent = $@"namespace {NomeProjeto}.Domain.Exceptions;

public class InfraStructureException : Exception
{{
    public InfraStructureException(string message) : base(message)
    {{
    }}
}}
";

        var enumContent = $@"namespace {NomeProjeto}.Domain.Enums;

public enum OptionsStatusUsuario
{{
    Ativo,
    Inativo,
    Bloqueado
}}
";

        var interfaceContent = $@"namespace {NomeProjeto}.Domain.Interfaces;

public interface IDomainMarker
{{
}}
";

        await _file.WriteContent(userContent, Path.Combine(pathEntities, "User.cs"));
        await _file.WriteContent(domainExceptionContent, Path.Combine(pathExceptions, "DomainException.cs"));
        await _file.WriteContent(serviceExceptionContent, Path.Combine(pathExceptions, "ServiceException.cs"));
        await _file.WriteContent(useCaseExceptionContent, Path.Combine(pathExceptions, "UseCaseException.cs"));
        await _file.WriteContent(infraExceptionContent, Path.Combine(pathExceptions, "InfraStructureException.cs"));
        await _file.WriteContent(enumContent, Path.Combine(pathEnums, "OptionsStatusUsuario.cs"));
        await _file.WriteContent(interfaceContent, Path.Combine(pathInterfaces, "Interfaces.cs"));

        return true;
    }
}
