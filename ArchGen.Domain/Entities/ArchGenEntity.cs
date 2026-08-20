namespace ArchGen.Domain;

public class ArchGenEntity
{
    public Guid ID {get; set;}
    public required string NomeProjeto {get; set;}
    public required string TipoDoProjeto {get; set;}
    public string? PathDomain {get; set;}
    public string? PathApplication {get; set;}
    public string? PathInfrastructure {get; set;}
    public string? PathTests {get; set;}

    public void Validar_TipoDoProjeto()
    {
        if(NomeProjeto == string.Empty)
        {
            throw new Exception("Nome do projeto é obrigatório.");
        }
        TipoDoProjeto = TipoDoProjeto.ToUpper();
        if (!TipoDoProjeto.Equals("API") || !TipoDoProjeto.Equals("CONSOLE"))
        {
            throw new Exception("Tipo de projeto inválido. Deve ser 'API' ou 'CONSOLE'.");
        }
    }
}
