using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rinha_de_backend_2024_q1.Entities
{
    [Table("saldos")]
    public class Saldo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("cliente_id")]
        public int ClienteId { get; set; }
        [Column("valor")]
        public int Valor { get; set; }
    }
}
