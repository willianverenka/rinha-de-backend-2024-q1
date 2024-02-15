using System.Text.Json.Serialization;
using rinha_de_backend_2024_q1.Repository;

namespace rinha_de_backend_2024_q1.Services;

public class TransacaoResponse
{
    [JsonPropertyName("limite")]
    public int Limite { get; set; }
    [JsonPropertyName("saldo")]
    public int Saldo { get; set; }

    public TransacaoResponse(int limite, int saldo)
    {
        Limite = limite;
        Saldo = saldo;
    }
}