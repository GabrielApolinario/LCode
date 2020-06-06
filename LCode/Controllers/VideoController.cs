using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCode.Models;

namespace LCode.Controllers
{
    public class VideoController : Controller
    {
        BancoDeDados bd = new BancoDeDados();
        // GET: Video
        public ActionResult CadastraVideo()
        {
            Video v = new Video();
            v.Cursos = BancoDeDados.PopulaCurso();
            return View(v);
        }

        [HttpPost]
        public ActionResult CadastraVideo(Video v)
        {
            string FileName = Path.GetFileNameWithoutExtension(v.video.FileName);

            string FileExtension = Path.GetExtension(v.video.FileName);

            FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + "-" + FileName.Trim() + FileExtension;

            string UploadPath = Server.MapPath("~/Videos/");

            v.video_link = UploadPath + FileName;

            v.video.SaveAs(v.video_link);

            v.video_link = "~/Videos/" + FileName;

            Video vid = new Video
            {
                video_titulo = v.video_titulo,
                video_descricao = v.video_descricao,
                video_link = v.video_link,
                video_curso = v.video_curso,

            };

            bd.InsereVideo(vid);

            return View();
        }
    }
}