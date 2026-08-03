using System.ComponentModel.DataAnnotations;

namespace ArchGen.Application;

public class ResultadoComandoResponse
{
    [Required(ErrorMessage = "Saida está em branco, campo Obrigatorio")]
    public required string Saida {get; set;}
    [Required(ErrorMessage = "Error está em branco, campo Obrigatorio")]
    public required string Error {get; set;} 
}
