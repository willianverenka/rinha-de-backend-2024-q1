using Microsoft.EntityFrameworkCore;
using Npgsql;
using rinha_de_backend_2024_q1.Context;
using rinha_de_backend_2024_q1.Entities;

namespace rinha_de_backend_2024_q1.Repository;

public class SaldoRepository : ISaldoRepository
{
    private AppDbContext _db;

    public SaldoRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Saldo?> GetSaldoByClienteId(int id)
    {
        return await _db.Saldos.FirstOrDefaultAsync(s => s.ClienteId == id);
    }
}