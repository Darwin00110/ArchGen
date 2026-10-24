using ArchGen.Domain.Entities;

namespace ArchGen.Domain.Interfaces;

public interface IDefaultTemplateProvider
{
    TemplateDefinition GetDefaultTemplate();
}
