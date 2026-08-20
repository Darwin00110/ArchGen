namespace Domain;
public class User
{
    public Guid ID {get; set;}
    public required string Nome {get; set;}
    public required string Telefone {get; set;}
    public required string Senha {get; set;}
    public required string Email {get; set;}
    public void Validate_Nome ()
    {
        if(string.IsNullOrEmpty(Nome))
        {
            throw new DomainException("O nome não pode ser nulo ou vazio.");
        }
    }
    public void Validate_Telefone ()
    {
        if(string.IsNullOrEmpty(Telefone))
        {
            throw new DomainException("O telefone não pode ser nulo ou vazio.");
        }
    }
    public void Validate_Email ()
    {
        if(string.IsNullOrEmpty(Email))
        {
            throw new DomainException("O email não pode ser nulo ou vazio.");
        }
        if (!Email.Contains("@gmail.com"))
        {
            throw new DomainException("Formato invalido de email, ex: (exemplo@gmail.com)");
        }
    }
}
