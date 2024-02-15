using rinha_de_backend_2024_q1.Entities;

namespace rinha_de_backend_2024_q1.Services;

public interface ITransacaoService
{
    public Task<Result<TransacaoResponse, Exception>> InsertTransacao(TransacaoRequest transacaoRequest, int id);
}