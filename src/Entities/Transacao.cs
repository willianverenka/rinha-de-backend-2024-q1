using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rinha_de_backend_2024_q1.Entities
{
    [Table("transacoes")]
    public class Transacao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("cliente_id")]
        public int ClienteId { get; set; }
        [Column("valor")]
        public int Valor { get; set; }
        [Column("tipo")]
        public char Tipo { get; set; }
        [Column("descricao")]
        public string Descricao { get; set; }
        [Column("realizada_em")]
        public DateTime RealizadaEm { get; set; }
    }
}
