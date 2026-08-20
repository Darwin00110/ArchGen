using ArchGen.Domain;

namespace ArchGen.Application;

public class ArchGenUseCase
{
    private readonly IArchGenRepository _repo;
    public ArchGenUseCase(IArchGenRepository archGenRepository)
    {
        _repo = archGenRepository;
    }
    
}
