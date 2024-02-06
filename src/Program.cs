using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rinha_de_backend_2024_q1.Context;
using rinha_de_backend_2024_q1.Entities;

var builder = WebApplication.CreateBuilder(args);
var variablesDictionary = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql($"Host=localhost;Port=5432;Pooling=true;Database={variablesDictionary["POSTGRES_DB"]};UserId={variablesDictionary["POSTGRES_USER"]};Password={variablesDictionary["POSTGRES_PASSWORD"]};"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/clientes", async (AppDbContext db) =>
{
    var query = await db.Clientes.FromSql($"SELECT * FROM clientes").ToListAsync();
    return Results.Json(query);
});

app.MapPost("/clientes/{id:int}/transacoes", async (int id, [FromBody]TransacaoRequest transacao, AppDbContext db) =>
{
    var saldoCliente = await db.Saldos.FirstOrDefaultAsync(t => t.ClienteId == id);
    var cliente = await db.Clientes.FindAsync(id);
    if(saldoCliente == null || cliente == null) return Results.NotFound();
    switch(transacao.Tipo)
    {
        case 'c':
            saldoCliente.Valor -= transacao.Valor;
            break;
        case 'd':
            if (cliente.Limite > transacao.Valor) return Results.BadRequest("O valor da transacao excede o limite.");
            saldoCliente.Valor += transacao.Valor;
            break;
        default:
            return Results.BadRequest();
    }

    await db.Transacoes.AddAsync(new Transacao()
    {
        ClienteId = id,
        Descricao = transacao.Descricao,
        Valor = transacao.Valor,
        Tipo = transacao.Tipo,
        RealizadaEm = DateTime.UtcNow
    });

    await db.SaveChangesAsync();

    var response = new
    {
        limite = cliente.Limite,
        saldo = saldoCliente.Valor
    };

    return Results.Ok(response);
});

app.MapGet("/clientes/{id:int}/extrato", async (int id, AppDbContext db) =>
{
    var cliente = await db.Clientes.FindAsync(id);
    var saldoCliente = await db.Saldos.FirstOrDefaultAsync(t => t.ClienteId == id);
    if (saldoCliente == null || cliente == null) return Results.NotFound();

    var transacoes = await db.Transacoes
        .Where(t => t.ClienteId == id)
        .OrderByDescending(t => t.RealizadaEm)
        .Take(10)
        .Select(t => new
        {
            valor = t.Valor,
            tipo = t.Tipo,
            descricao = t.Descricao,
            realizada_em = t.RealizadaEm
        })
        .ToListAsync();

    var response = new
    {
        saldo = new
        {
            total = saldoCliente.Valor,
            data_extrato = DateTime.UtcNow,
            limite = cliente.Limite
        },
        ultimas_transacoes = transacoes
    };

    return Results.Ok(response);
});

app.Run();

