using System.Text.Json.Serialization;
using rinha_de_backend_2024_q1.Entities;

namespace rinha_de_backend_2024_q1.Services;

public class ExtratoResponse
{
    [JsonPropertyName("saldo")]
    public SaldoResponse Saldo { get; set; }
    [JsonPropertyName("ultimas_transacoes")]
    public IEnumerable<UltimaTransacaoUniqueResponse> UltimasTransacoes { get; set; }

    public ExtratoResponse(SaldoResponse saldo, IEnumerable<UltimaTransacaoUniqueResponse> ultimasTransacoes)
    {
        Saldo = saldo;
        UltimasTransacoes = ultimasTransacoes;
    }
}