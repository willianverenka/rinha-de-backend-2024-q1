using System.Text.Json.Serialization;

namespace rinha_de_backend_2024_q1.Services;

public class UltimaTransacaoUniqueResponse
{
    [JsonPropertyName("valor")]
    public int Valor { get; set; }
    [JsonPropertyName("tipo")]
    public char Tipo { get; set; }
    [JsonPropertyName("descricao")]
    public string Descricao { get; set; }
    [JsonPropertyName("realizada_em")]
    public DateTime RealizadaEm { get; set; }

    public UltimaTransacaoUniqueResponse(int valor, char tipo, string descricao, DateTime realizadaEm)
    {
        Valor = valor;
        Tipo = tipo;
        Descricao = descricao;
        RealizadaEm = realizadaEm;
    }
}