using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rinha_de_backend_2024_q1.Entities
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("limite")]
        public int Limite { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
    }
}
