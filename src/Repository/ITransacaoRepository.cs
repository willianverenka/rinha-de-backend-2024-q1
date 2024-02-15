using rinha_de_backend_2024_q1.Entities;
using rinha_de_backend_2024_q1.Services;

namespace rinha_de_backend_2024_q1.Repository;

public interface ITransacaoRepository
{
    Task<IEnumerable<UltimaTransacaoUniqueResponse>> GetLastTransacoesByClienteId(int id);
    Task AddTransacao(TransacaoRequest transacaoRequest, int id);
}