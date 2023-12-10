using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeVendas.Classes
{
    public class Venda
    {
        public string? Anotacao { get; set; }
        public DateTime? Data { get; set; }
        public string? Cliente { get; set; }
        public enum Pagamento
        {
            Pix,
            Dinheiro,
            Debito,
            Credito,
        }
    }
}
