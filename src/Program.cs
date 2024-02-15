using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using rinha_de_backend_2024_q1.Context;
using rinha_de_backend_2024_q1.Entities;
using rinha_de_backend_2024_q1.Exceptions;
using rinha_de_backend_2024_q1.Repository;
using rinha_de_backend_2024_q1.Services;

var builder = WebApplication.CreateBuilder(args);
bool containerizedApp = true;
string connectionString;
if (containerizedApp)
{
    var variablesDictionary = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
    connectionString = $"Host=db;Port=5432;Pooling=true;Minimum Pool Size=50;Maximum Pool Size=2000;Timeout=15;Database={variablesDictionary["POSTGRES_DB"]};UserId={variablesDictionary["POSTGRES_USER"]};Password={variablesDictionary["POSTGRES_PASSWORD"]};";
}
else
{
    connectionString = $"Host=localhost;Port=5432;Pooling=true;Database=postgres;UserId=postgres;Password=123;";
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ISaldoRepository, SaldoRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<IExtratoService, ExtratoService>();
builder.Logging.AddConsole();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapPost("/clientes/{id:int}/transacoes", async
(int id, [FromBody] TransacaoRequest transacao, ITransacaoService transacaoService, ILogger<Program> logger) =>
{
    var result = await transacaoService.InsertTransacao(transacao, id);
    return result.Match(
        transacaoResponse =>
        {
            logger.LogInformation($"RESULTADO OK | limite: {transacaoResponse.Limite}, saldo: {transacaoResponse.Saldo}");
            return Results.Json(transacaoResponse);
        },
        exception =>
        {
            switch (exception)
            {
                case NotFoundException:
                    logger.LogInformation(nameof(NotFoundException));
                    return Results.NotFound();
                case DatabaseOperationException:
                    logger.LogError(nameof(DatabaseOperationException));
                    return Results.StatusCode(503);
                case InvalidRequestException:
                    logger.LogInformation(nameof(UnprocessableEntityResult));
                    return Results.UnprocessableEntity();
                default:
                    logger.LogError("UNKNOWN");
                    return Results.Problem();
            }
        });
});

app.MapGet("/clientes/{id:int}/extrato", async (int id, IExtratoService _extratoService, ILogger<Program> _logger) =>
{
    var result = await _extratoService.GetExtratoByClienteId(id);
    return result.Match(
        extratoResponse =>
        {
            _logger.LogInformation("RESULTADO OK EXTRATO");
            return Results.Json(extratoResponse);
        },
        exception =>
        {
            switch (exception)
            {
                case NotFoundException:
                    _logger.LogInformation(nameof(NotFoundException));
                    return Results.NotFound();
                case DatabaseOperationException:
                    _logger.LogError(nameof(DatabaseOperationException));
                    return Results.StatusCode(503);
                default:
                    _logger.LogError("UNKNOWN");
                    return Results.Problem();
            }
        });
});

app.Run();