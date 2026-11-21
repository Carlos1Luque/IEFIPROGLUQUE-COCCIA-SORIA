using Testing.Data;
namespace Testing.Business;

public class ClienteService
{
    private readonly ClienteRepository _repository;

    public ClienteService(ClienteRepository repository)
    {
        _repository = repository;
    }
}
