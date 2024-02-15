namespace rinha_de_backend_2024_q1.Entities
{
    public class TransacaoRequest
    {
        public int Valor { get; set; }
        public char Tipo { get; set; }  
        public string? Descricao { get; set; }
    }
}
