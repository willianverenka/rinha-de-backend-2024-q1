using rinha_de_backend_2024_q1.Entities;

namespace rinha_de_backend_2024_q1.Repository;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> GetAllClientes();
    Task<Cliente?> GetClienteById(int id);
}