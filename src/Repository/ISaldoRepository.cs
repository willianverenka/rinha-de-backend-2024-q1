using rinha_de_backend_2024_q1.Entities;

namespace rinha_de_backend_2024_q1.Repository;

public interface ISaldoRepository
{
   Task<Saldo?> GetSaldoByClienteId(int id);
}