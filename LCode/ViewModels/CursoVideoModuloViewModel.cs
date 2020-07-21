using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCode.ViewModels
{
    public class CursoVideoModuloViewModel
    {
        [DisplayName("Identificação")]
        public int Curso_id { get; set; }

        [DisplayName("Nome")]
        public string Curso_nome { get; set; }

        [DisplayName("Valor")]
        public double Curso_valor { get; set; }

        [DisplayName("Duração")]
        public double Curso_duracao { get; set; }

        [DisplayName("Descrição")]
        public string Curso_descricao { get; set; }

        [DisplayName("Status")]
        public int Curso_status { get; set; }

        [DisplayName("Categoria")]
        public int Curso_categoria { get; set; }

        public int mod_id { get; set; }

        public string mod_nome { get; set; }

        public string mod_desc { get; set; }

        public int mod_curso { get; set; }

        public int mod_qtd_video { get; set; }

        public int video_id { get; set; }

        [DisplayName("Título")]
        public string video_titulo { get; set; }

        [DisplayName("Descrição da aula")]
        public string video_descricao { get; set; }

        public int video_modulo { get; set; }

        [DisplayName("Curso que receberá a aula")]
        public int video_curso { get; set; }

        public string video_link { get; set; }

        [Required]
        public HttpPostedFileBase video { get; set; }
    }
}