using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCode.Models
{
    public class Livros
    {
        public int Livros_id { get; set; }

        public string Livros_nome { get; set; }

        public string Livros_descricao { get; set; }

        public string Livros_link { get; set; }

        public int Livros_status { get; set; }

        public int Livros_curso { get; set; }
    }
}