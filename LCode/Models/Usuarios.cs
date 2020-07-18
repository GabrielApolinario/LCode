using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using LCode.Dados;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Mvc;

namespace LCode.Models
{

    public class Usuarios
    {

        public List<SelectListItem> Paises { get; set; }
        //public List<SelectListItem> Hierarquia { get; set; }


        [DisplayName("Identificação")]
        public int Usu_id { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        public string Usu_nome { get; set; }

        [DisplayName("Sobrenome")]
        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        public string Usu_sobrenome { get; set; }

        [DisplayName("E-mail")]
        //[RegularExpression(@"/ ^[a - z0 - 9.] +@[a-z0-9]+\.[a-z]+\.([a - z]+)?$/i", ErrorMessage = "Digite um e-mail válido" )]
        [Required(ErrorMessage = "O e-mail é obritarório")]
        public string Usu_email { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Usu_senha { get; set; }

        [DisplayName("Cpf ou Cnpj")]
        //[RegularExpression(@"/(^\d{3}\.\d{3}\.\d{3}\-\d{2}$)|(^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$)/", ErrorMessage ="Digite um CPF ou CNPJ válido")]
        [Required(ErrorMessage = "O Cpf ou Cnpj é obrigatório")]
        public string Usu_cpf_ou_cnpj { get; set; }

        [DisplayName("Data de nascimento")]
        [Required(ErrorMessage = "A data é obrigatória")]
        public DateTime Usu_data_nasc { get; set; }

        [DisplayName("Empresa (Se houver)")]
        public string Usu_empresa { get; set; }

        [DisplayName("Data de integração")]
        public DateTime Usu_data_integracao { get; set; }

        [DisplayName("País")]
        [Required(ErrorMessage = "O país é obrigatório")]
        public int Usu_pais { get; set; }

        [DisplayName("Tipo de usuário")]
        [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
        public string Usu_hierarquia { get; set; }

        
    }
}