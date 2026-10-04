using ArchGen.Domain;

namespace ArchGen.Application;

public interface IMainTemplateService
{
    public Task<TemplateStructure> CreateEntityArchGen(string NomeProjeto, string PathProjeto, string TipoProjeto);
}

