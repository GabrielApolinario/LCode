using LCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCode.Controllers
{
    public class CursoController : Controller
    {
        BancoDeDados bd = new BancoDeDados();
        Curso c = new Curso();

        // GET: Curso
        public ActionResult CadastroCurso()
        {
            if (Session["Professor"] != null || Session["Adm"] != null)
            {
                Curso c = new Curso();           
                c.Categorias = BancoDeDados.PopulaCategorias();
                return View(c);

            }
            else
            {
                return RedirectToAction("Login", "Autenticacao");
            }
        }

        [HttpPost]
        public ActionResult CadastroCurso(Curso c)
        {

            if (ModelState.IsValid)
            {
                Curso curso = new Curso
                {
                    Curso_nome = c.Curso_nome,
                    Curso_descricao = c.Curso_descricao,
                    Curso_duracao = c.Curso_duracao,
                    Curso_valor = c.Curso_valor,
                    Curso_categoria = c.Curso_categoria,
                };
                
                bd.InsereCurso(curso, Convert.ToInt32(Session["UsuId"]));
                Video v = new Video();
                v.video_curso = bd.novo_curso_id;
                return RedirectToAction("CadastraVideo", "Video", new { curso_id = v.video_curso });
            }
            else
            {
                c.Categorias = BancoDeDados.PopulaCategorias();

                return View(c);
            }
        }
    }
}