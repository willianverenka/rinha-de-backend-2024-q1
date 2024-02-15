namespace rinha_de_backend_2024_q1.Services;

public interface IExtratoService
{
    public Task<Result<ExtratoResponse, Exception>> GetExtratoByClienteId(int id);
}