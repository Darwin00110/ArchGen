
using System.ComponentModel.DataAnnotations;

namespace Application;

public class UpdateUserRequest
{
    public Guid? ID {get; set;}
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email fornecido não é válido.")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "A senha é obrigatória.")]
    public required string Senha {get; set;}
    public required string Telefone {get; set;}
    public required string Nome {get; set;}
}