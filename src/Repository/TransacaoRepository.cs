using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using rinha_de_backend_2024_q1.Context;
using rinha_de_backend_2024_q1.Entities;
using rinha_de_backend_2024_q1.Services;

namespace rinha_de_backend_2024_q1.Repository;

public class TransacaoRepository : ITransacaoRepository
{
    private AppDbContext _db;
    private ILogger<TransacaoRepository> _logger;

    public TransacaoRepository(AppDbContext db, ILogger<TransacaoRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<UltimaTransacaoUniqueResponse>> GetLastTransacoesByClienteId(int id)
    {
        var query = await _db.Transacoes
            .Where(t => t.ClienteId == id)
            .OrderByDescending(t => t.RealizadaEm)
            .Take(10)
            .ToListAsync();

        return query.Select(t => new UltimaTransacaoUniqueResponse(t.Valor, t.Tipo, t.Descricao, t.RealizadaEm));

    }

    public async Task AddTransacao(TransacaoRequest transacaoRequest, int id)
    {
        await _db.Transacoes
            .AddAsync(new Transacao()
            {
                ClienteId = id,
                Valor = transacaoRequest.Valor,
                Tipo = transacaoRequest.Tipo,
                Descricao = transacaoRequest.Descricao,
                RealizadaEm = DateTime.UtcNow
            });
        await _db.SaveChangesAsync();
    }
}