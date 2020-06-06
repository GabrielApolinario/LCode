using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCode.Models
{
    public class Video
    {
        public List<SelectListItem> Cursos { get; set; }

        public int video_id { get; set; }

        [Required]
        [DisplayName("Título")]
        public string video_titulo {get;set;}

        [DisplayName("Título")]
        public string video_descricao { get; set; }

        [Required]
        public string video_link { get; set; }
        
        [Required]
        [DisplayName("Curso que receberá a aula")]
        public int video_curso { get; set; }
       
        [Required]
        public HttpPostedFileBase video { get; set; }
    }
}