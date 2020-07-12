using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCode.Models
{
    public class Compra
    {
        public int compra_id { get; set; }
        public int compra_curso { get; set; }

        public DateTime compra_hora { get; set; }

        public DateTime compra_data { get; set; }
        public int compra_usu { get; set; }

        public int compra_status { get; set; }

        public string compra_pag_forma { get; set; }

    }
}