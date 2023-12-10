using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeVendas.Classes
{
    public class Produto
    {
        public int? Id { get; set; }

        public string? Nome { get; set; }

        public float? Preco { get; set; }

        public string? Descricao { get; set; }
    }
}
