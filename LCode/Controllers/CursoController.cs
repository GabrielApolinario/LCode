using LCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCode.ViewModels;
using System.Configuration;
using MySql.Data.MySqlClient;
using LCode.Dados;

namespace LCode.Controllers
{
    public class CursoController : Controller
    {
        BancoDeDados bd = new BancoDeDados();
        CursosAcoes ca = new CursosAcoes();

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
                Modulo m = new Modulo();
                m.mod_curso = bd.novo_curso_id;
                return RedirectToAction("AddModulo", "Curso", new { curso_id = m.mod_curso });
            }
            else
            {
                c.Categorias = BancoDeDados.PopulaCategorias();

                return View(c);
            }
        }

        public ActionResult AddModulo(Nullable<int> curso_id)
        {
            Modulo m = new Modulo();
            GetModulos(Convert.ToInt32(curso_id));
            m.modulos_lista = Request["modulos"];
            m.mod_curso = Convert.ToInt32(curso_id);
            return View(m);

        }

        [HttpPost]
        public ActionResult AddModulo(Modulo m)
        {
            Modulo mod = new Modulo
            {
                mod_desc = m.mod_desc,
                mod_nome = m.mod_nome,
                mod_curso = m.mod_curso,
                mod_qtd_video = m.mod_qtd_video,

            };

            bd.InsereModulo(mod);

            m.mod_id = bd.mod_id;
            return RedirectToAction("CadastraVideo", "Video", new { curso_id = m.mod_curso, m.mod_id });
        }

        public ActionResult Cursos()
        {
            var retorno = ca.ListarCursos();

            return View(retorno);
        }

        
        public ActionResult DetalhesCurso(Nullable <int> curso_id)
        {
            var retorno = ca.QueryDetalhesCurso(Convert.ToInt32(curso_id));

            ViewData["modulos"] = bd.QueryModulos(Convert.ToInt32(curso_id));
            ViewData["videos"] = bd.QueryVideos(Convert.ToInt32(curso_id));
            return View(retorno);
        }
       
        public void GetModulos(int id_curso)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["BdConexao"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {

                string query = string.Format("SELECT * FROM lc_modulo WHERE mod_curso = {0};", id_curso);
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["mod_nome"].ToString(),
                                Value = sdr["mod_curso"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            ViewBag.modulos = new SelectList(items, "Value", "Text");
        }

        public ActionResult PesquisarCursos(FormCollection frm)
        {
            if(frm["pesquisar"] != null)
            {
                try
                {
                    var pesquisa = frm["pesquisar"];
                    var retorno = ca.PesquisarCursos(pesquisa);

                    if(retorno.Count == 0)
                    {
                        ViewBag.curso = "Não foi encontrado nenhum curso";
                    }
                    return View(retorno);
                }
                catch 
                {
                    ViewBag.curso = "Ocorreu um erro ao pesquisar o curso";
                    return View();
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AssistirCurso(int curso_id)
        {
            
            var retorno = ca.ValidarCursoComprado(Convert.ToInt32(Session["UsuId"]));
            
            if (retorno != null)
            {
                var curso = ca.QueryDetalhesCurso(Convert.ToInt32(curso_id));

                ViewData["modulos"] = bd.QueryModulos(Convert.ToInt32(curso_id));
                ViewData["videos"] = bd.QueryVideos(Convert.ToInt32(curso_id));

                return View(curso);
            }

            return View();           
        }
            
            
    }
}