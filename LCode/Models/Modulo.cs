using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LCode.Models
{
    public class Modulo
    {
        public int mod_id { get; set; }

        public string mod_nome { get; set; }

        public string mod_desc { get; set; }

        public int mod_curso { get; set; }

        public int mod_qtd_video { get; set; }

        public string modulos_lista { get; set; }
    }
}