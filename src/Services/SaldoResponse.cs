using System.Text.Json.Serialization;

namespace rinha_de_backend_2024_q1.Services;

public class SaldoResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("data_extrato")]
    public DateTime DataExtrato { get; set; }
    [JsonPropertyName("limite")]
    public int Limite { get; set; }

    public SaldoResponse(int total, int limite)
    {
        Total = total;
        DataExtrato = DateTime.UtcNow;
        Limite = limite;
    } 
}