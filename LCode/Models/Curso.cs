using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCode.Models
{
    public class Curso
    {

        public List<SelectListItem> Categorias { get; set; }

        [DisplayName("Identificação")]
        public int Curso_id { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O nome do curso deve ser preenchido")]
        public string Curso_nome { get; set; }

        [DisplayName("Valor")]
        [Required(ErrorMessage = "O Valor do curso deve ser preenchido")]
        public double Curso_valor { get; set; }

        [DisplayName("Duração")]
        [Required(ErrorMessage = "A duração do curso deve ser preenchida")]
        public double Curso_duracao { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Insira uma descrição do curso")]
        public string Curso_descricao { get; set; }

        [DisplayName("Status")]
        public int Curso_status { get; set; }

        [DisplayName("Categoria")]
        [Required(ErrorMessage = "Selecione uma categoria para o curso")]
        public int Curso_categoria { get; set; }

    }
}