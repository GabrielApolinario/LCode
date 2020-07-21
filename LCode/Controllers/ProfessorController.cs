using LCode.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCode.ViewModels;
using System.IO;

namespace LCode.Controllers
{
    public class ProfessorController : Controller
    {

        ProfessorAcoes pa = new ProfessorAcoes();
        // GET: Professor
        public ActionResult CursosCriados()
        {
            if (Session["Adm"] != null || Session["Professor"] != null)
            {
                var retorno = pa.CursosCriados(Convert.ToInt32(Session["UsuId"]));
                return View(retorno);
            }
                TempData["loginCursosCriados"] = "É obrigatório realizar o login para acessar os cursos criados por você!";
                return RedirectToAction("Login", "Autenticacao");


        }

        [HttpGet]
        public ActionResult EditarCurso(int curso_id)
        {
            var retorno = pa.EditarCurso(curso_id);

            return View(retorno);
        }

        public ActionResult EditarVideo(int curso_id, int video_id)
        {
            var retorno = pa.DetalhesVideo(video_id);

            return View(retorno);
        }

        [HttpPost]
        public ActionResult EditarVideo(int video_id, CursoVideoModuloViewModel v)
        {
            try
            {
                string FileName = Path.GetFileNameWithoutExtension(v.video.FileName);

                string FileExtension = Path.GetExtension(v.video.FileName);

                string final = "";

                foreach (char s in FileName.ToCharArray())
                {
                    if (s != ' ')
                        final += s;
                }

                FileName = final;

                FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + "-" + FileName + FileExtension;

                string UploadPath = Server.MapPath("~/Videos/");

                v.video_link = UploadPath + FileName;

                v.video.SaveAs(v.video_link);

                v.video_link = "~/Videos/" + FileName;
            }
            catch
            {
                var vidPadrao = pa.DetalhesVideo(video_id);
                v.video_link = vidPadrao.video_link;                
            }

            pa.EditaVideo(v);

            return View();
        }


    }
}