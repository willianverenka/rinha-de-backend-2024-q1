using rinha_de_backend_2024_q1.Entities;
using rinha_de_backend_2024_q1.Exceptions;
using rinha_de_backend_2024_q1.Repository;

namespace rinha_de_backend_2024_q1.Services;

public class TransacaoService : ITransacaoService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ISaldoRepository _saldoRepository;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ILogger<TransacaoService> _logger;

    public TransacaoService(IClienteRepository clienteRepository, ISaldoRepository saldoRepository,
        ITransacaoRepository transacaoRepository, ILogger<TransacaoService> logger)
    {
        _clienteRepository = clienteRepository;
        _saldoRepository = saldoRepository;
        _transacaoRepository = transacaoRepository;
        _logger = logger;
    }

    public async Task<Result<TransacaoResponse, Exception>> InsertTransacao(TransacaoRequest transacaoRequest, int id)
    {
        Cliente? cliente;
        Saldo? saldo;
        
        try
        {
            cliente = await _clienteRepository.GetClienteById(id);
            saldo = await _saldoRepository.GetSaldoByClienteId(id);
        }
        catch (Exception)
        {
            return new Result<TransacaoResponse, Exception>(new DatabaseOperationException());
        }

        if (IsQueryInvalida(cliente, saldo, transacaoRequest))
            return new Result<TransacaoResponse, Exception>(new NotFoundException());
        if (IsRequestInvalido(transacaoRequest))
            return new Result<TransacaoResponse, Exception>(new InvalidRequestException());
        
        int saldoFinal = saldo.Valor - transacaoRequest.Valor;
        bool isCredito = transacaoRequest.Tipo == 'c', excedeLimite = saldoFinal < cliente.Limite * -1;

        if (IsTransacaoInvalida(isCredito, excedeLimite))
            return new Result<TransacaoResponse, Exception>(new InvalidRequestException());
        
        saldo.Valor = saldoFinal;

        try
        {
            await _transacaoRepository.AddTransacao(transacaoRequest, id);
        }
        catch (Exception)
        {
            return new Result<TransacaoResponse, Exception>(new DatabaseOperationException());
        }
        _logger.LogInformation("hit sucesso monad");
        return new Result<TransacaoResponse, Exception>(new TransacaoResponse(cliente.Limite, saldo.Valor));
    }

    private bool IsQueryInvalida(Cliente? cliente, Saldo? saldo, TransacaoRequest transacaoRequest) =>
        cliente is null || saldo is null;
        
    private bool IsRequestInvalido(TransacaoRequest transacaoRequest) => 
        !(transacaoRequest.Tipo is 'c' or 'd') || 
        transacaoRequest.Descricao.Length > 10 || 
        transacaoRequest.Descricao is null or "";
    
    private bool IsTransacaoInvalida(bool isCredito, bool excedeLimite) => isCredito && excedeLimite;

}