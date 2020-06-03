using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace LCode.Models
{
    public class Usuarios
    {
        [DisplayName("Identificação")]
        public int Usu_id { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        public string Usu_nome { get; set; }

        [DisplayName("Sobrenome")]
        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        public string Usu_sobrenome { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O e-mail é obritarório")]
        public string Usu_email { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Usu_senha { get; set; }

        [DisplayName("Cpf ou Cnpj")]
        [Required(ErrorMessage = "O Cpf ou Cnpj é obrigatório")]
        public string Usu_cpf_ou_cnpj { get; set; }

        [DisplayName("Data de nascimento")]
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime Usu_data_nasc { get; set; }

        [DisplayName("Empresa (Se houver)")] 
        public string Usu_empresa { get; set; }

        [DisplayName("Data de integração")]
        public DateTime Usu_data_integracao { get; set; }

        [DisplayName("País")]
        [Required(ErrorMessage = "Escolher o país é obrigatório")]
        public int Usu_pais { get; set; }

        [DisplayName("Tipo de usuário")]
        [Required(ErrorMessage = "Escolher o tipo de usuário é obrigatório")]
        public string Usu_hierarquia { get; set; }
    }
}