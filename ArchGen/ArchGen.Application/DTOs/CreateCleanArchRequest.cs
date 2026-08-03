using System.ComponentModel.DataAnnotations;

namespace ArchGen.Application;

public class CreateCleanArchRequest
{
    [Required(ErrorMessage = "Nome do projeto não pode estar em branco, campo obrigatorio.")]
    public required string NomeDoProjeto {get; set;}
    [Required(ErrorMessage = "Nome do projeto não pode estar em branco, campo obrigatorio.")]
    public required string TipoDoProjeto {get; set;} 
}
