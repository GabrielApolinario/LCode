using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using LCode.Models;
using Microsoft.Ajax.Utilities;

namespace LCode.Controllers
{
    public class VideoController : Controller
    {
        BancoDeDados bd = new BancoDeDados();
        // GET: Video
        [HttpGet]
        public ActionResult CadastraVideo(Nullable<int> curso_id, Nullable<int> mod_id, Nullable<int> mod_curso)
        {
            if (Session["Professor"] != null || Session["Adm"] != null)
            {
                try
                {
                    Video v = new Video();
                    v.Modulos = BancoDeDados.PopulaModulos(Convert.ToInt32(curso_id));
                    v.video_curso = Convert.ToInt32(Request.QueryString["curso_id"]);
                    v.video_modulo = Convert.ToInt32(mod_id);
                    return View(v);
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Autenticacao");
            }
        }

        [HttpPost]
        public ActionResult CadastraVideo(Video v)
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


                Video vid = new Video
                {
                    video_titulo = v.video_titulo,
                    video_descricao = v.video_descricao,
                    video_link = v.video_link,
                    video_curso = v.video_curso,
                    video_modulo = v.video_modulo,
                };

                bd.InsereVideo(vid);

                vid.video_titulo = "";
                vid.video_descricao = "";
                vid.video_link = "";

                ViewBag.CadVideo = "Vídeo cadastrado com sucesso!";

                string PrevUrl = Request.QueryString["prevUrl"].ToString();

                if (PrevUrl.Contains("/Professor/EditarCurso"))                
                {
                    TempData["VidCadastrado"] = "O vídeo foi cadastrado com sucesso!";
                    return RedirectToAction("EditarCurso", "Professor", new { curso_id = vid.video_curso});
                }

                    return View(vid);
            }

            catch
            {
                ViewBag.CadVideo = "Erro ao cadastrar o vídeo!";

                return View();
            }
        }

    }
}