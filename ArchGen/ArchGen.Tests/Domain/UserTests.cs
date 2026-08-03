using ArchGen.Domain;

namespace ArchGen.Tests;

public class UserTests
{
    [Fact]
    public void ArchGenEntity_Deve_Permitir_Definir_Propriedades()
    {
        var entity = new ArchGenEntity
        {
            ID = Guid.NewGuid(),
            NomeProjeto = "MeuProjeto",
            TipoDoProjeto = "API"
        };

        Assert.Equal("MeuProjeto", entity.NomeProjeto);
        Assert.Equal("API", entity.TipoDoProjeto);
        Assert.NotEqual(Guid.Empty, entity.ID);
    }
}
