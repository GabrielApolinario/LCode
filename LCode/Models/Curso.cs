using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCode.Models
{
    public class Curso
    {
        public int Curso_id { get; set; }

        public string Curso_nome { get; set; }

        public double Curso_valor { get; set; }

        public double Curso_duracao { get; set; }

        public string Curso_area { get; set; }

        public string Curso_descricao { get; set; }

        public int Curso_status { get; set; }

        public int Curso_categoria { get; set; }

    }
}