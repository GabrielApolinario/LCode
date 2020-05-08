using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCode.Models
{
    public class Usuarios
    {
        public int Usu_id { get; set; }

        public string Usu_nome { get; set; }

        public string Usu_sobrenome { get; set; }

        public string Usu_email { get; set; }

        public string Usu_senha { get; set; }

        public string Usu_cpf_ou_cnpj { get; set; }

        public DateTime Usu_data_nasc { get; set; }

        public string Usu_empresa { get; set; }

        public DateTime Usu_data_integracao { get; set; }

        public int Usu_pais { get; set; }

        public string Usu_hierarquia { get; set; }
    }
}