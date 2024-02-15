using System.Collections;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using rinha_de_backend_2024_q1.Context;
using rinha_de_backend_2024_q1.Entities;

namespace rinha_de_backend_2024_q1.Repository;

public class ClienteRepository : IClienteRepository
{
    private AppDbContext _db;
    private ILogger<ClienteRepository> _logger;

    public ClienteRepository(AppDbContext db, ILogger<ClienteRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Cliente>> GetAllClientes()
    {
        return await _db.Clientes.FromSql($"SELECT * FROM clientes;").ToListAsync();
    }

    public async Task<Cliente?> GetClienteById(int id)
    {
        return await _db.Clientes.FindAsync(id);
    }
}