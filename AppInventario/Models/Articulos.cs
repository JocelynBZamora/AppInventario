using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInventario.Models
{
    public class Articulos
    {
        public int id {  get; set; }
        public string descripcion { get; set; } = string.Empty;
        public decimal precio { get; set; }
        public int existencia { get; set; }
    }
}
