using System.Collections;
using rinha_de_backend_2024_q1.Entities;
using rinha_de_backend_2024_q1.Exceptions;
using rinha_de_backend_2024_q1.Repository;

namespace rinha_de_backend_2024_q1.Services;

public class ExtratoService : IExtratoService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ISaldoRepository _saldoRepository;

    public ExtratoService(IClienteRepository clienteRepository, ITransacaoRepository transacaoRepository,
        ISaldoRepository saldoRepository)
    {
        _clienteRepository = clienteRepository;
        _transacaoRepository = transacaoRepository;
        _saldoRepository = saldoRepository;
    }
    public async Task<Result<ExtratoResponse, Exception>> GetExtratoByClienteId(int id)
    {
        Cliente? cliente;
        Saldo? saldo;
        IEnumerable<UltimaTransacaoUniqueResponse> transacoes;

        try
        {
            cliente = await _clienteRepository.GetClienteById(id);
            saldo = await _saldoRepository.GetSaldoByClienteId(id);
            transacoes = await _transacaoRepository.GetLastTransacoesByClienteId(id);
        }
        catch (Exception)
        {
            return new Result<ExtratoResponse, Exception>(new DatabaseOperationException());
        }

        if (IsQueryInvalida(cliente, saldo))
            return new Result<ExtratoResponse, Exception>(new NotFoundException());

        return new Result<ExtratoResponse, Exception>(new ExtratoResponse(new SaldoResponse(saldo.Valor, cliente.Limite), transacoes));
    }

    private bool IsQueryInvalida(Cliente? cliente, Saldo? saldo) =>
        cliente is null || saldo is null;
}
