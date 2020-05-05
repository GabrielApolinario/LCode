using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCode.Models
{
    public class Certificado
    {
        public int Certificado_id { get; set; }

        public int Certificado_curso_comprado { get; set; }

        public int Certificado_usuario { get; set; }

        public string Certificado_link { get; set; }
    }
}