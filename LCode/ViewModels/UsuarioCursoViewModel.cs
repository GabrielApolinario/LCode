using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCode.Models;

namespace LCode.ViewModels
{
    public class UsuarioCursoViewModel
    {
        public int Curso_id { get; set; }
        public string Curso_nome { get; set; }
        public double Curso_duracao { get; set; }

        public int Usu_id { get; set; }

        public string Usu_nome { get; set; }

        public string Hora_Certificado = DateTime.Now.ToString("dd/MMMM/yyyy");

    }
}