using Testing.Data;
namespace Testing.Business;

public class TurnoService
{
    private readonly TurnoRepository _repository;

    public TurnoService(TurnoRepository repository)
    {
        _repository = repository;
    }
}
