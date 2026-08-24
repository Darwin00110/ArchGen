
using System.ComponentModel.DataAnnotations;

namespace Application;

public class CreateUserRequest
{
    [Required(ErrorMessage = 'O nome é obrigatório.')]
    public required string Nome { get; set; }
    [Required(ErrorMessage = 'O email é obrigatório.')]
    [EmailAddress(ErrorMessage = 'O email fornecido não é válido.')]
    public required string Email { get; set; }
    public string? Telefone { get; set; }
    [Required(ErrorMessage = 'A senha é obrigatória.')]
    public required string Senha {get; set;}
}