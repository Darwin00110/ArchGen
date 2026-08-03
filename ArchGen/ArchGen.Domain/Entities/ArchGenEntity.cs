namespace ArchGen.Domain;

public class ArchGenEntity
{
    public Guid ID {get; set;}
    public required string NomeProjeto {get; set;}
    public required string TipoDoProjeto {get; set;}
    
}
