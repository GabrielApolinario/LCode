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
using System.IO;

namespace LCode.Controllers
{
    public class CursoController : Controller
    {
        BancoDeDados bd = new BancoDeDados();
        CursosAcoes ca = new CursosAcoes();
        UsuarioAcoes ua = new UsuarioAcoes();
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
                string FileName = Path.GetFileNameWithoutExtension(c.Imagem.FileName);

                string FileExtension = Path.GetExtension(c.Imagem.FileName);

                FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + "-" + FileName.Trim() + FileExtension;

                string UploadPath = Server.MapPath("~/ImagensCurso/");

                c.Imagem_link = UploadPath + FileName;

                c.Imagem.SaveAs(c.Imagem_link);

                c.Imagem_link = "~/ImagensCurso/" + FileName;

                Curso curso = new Curso
                {
                    Curso_nome = c.Curso_nome,
                    Curso_descricao = c.Curso_descricao,
                    Curso_duracao = c.Curso_duracao,
                    Curso_valor = c.Curso_valor,
                    Curso_categoria = c.Curso_categoria,
                    Imagem_link = c.Imagem_link,
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

            //string PrevUrl = Request.UrlReferrer.ToString();

            //PrevUrl.Contains("EditarCurso")

            return View(m);

        }

        [HttpPost]
        public ActionResult AddModulo(Modulo m, int curso_id)
        {
            
            Modulo mod = new Modulo
            {
                mod_desc = m.mod_desc,
                mod_nome = m.mod_nome,
                mod_curso = curso_id,

            };

            bd.InsereModulo(mod);

            m.mod_id = bd.mod_id;
            string PrevUrl = Request.QueryString["prevUrl"].ToString();

            if(PrevUrl.Contains("/Professor/EditarCurso"))
            {
                TempData["ModCadastrado"] = "Módulo cadastrado com sucesso. Agora você pode cadastrar novas aulas neste módulo!";
                return RedirectToAction("EditarCurso", "Professor", new { curso_id = Request.QueryString["curso_id"] });
            }

            return RedirectToAction("CadastraVideo", "Video", new { curso_id = Request.QueryString["curso_id"], m.mod_id });
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

            Video vid = new Video();
            vid.video_link = ca.VideoPadrao(Convert.ToInt32(curso_id)).video_link;
            ViewData["videoPadrao"] = vid.video_link;
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

        public ActionResult AssistirCurso(Nullable<int> curso_id, Nullable<int> video_id)
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
       

        public ActionResult AssistirVideo(Nullable<int> curso_id, Nullable<int> video_id)
        {
            Video retorno = new Video();

            if (video_id == null)
            {
               retorno = ca.VideoPadrao(Convert.ToInt32(curso_id));

                return View(retorno);
            }

            retorno = ca.DetalhesVideo(Convert.ToInt32(video_id));

            return View(retorno);
        }

        public ActionResult DescricaoAula(Nullable<int> curso_id, Nullable<int> video_id)
        {
            Video retorno = new Video();

            if (video_id == null)
            {
                retorno = ca.VideoPadrao(Convert.ToInt32(curso_id));

                return View(retorno);
            }

            retorno = ca.DetalhesVideo(Convert.ToInt32(video_id));

            return View(retorno);
        }

        public ActionResult Certificado(int curso_id, int usu_id)
        {            
            var retorno = ca.GetCertificado(curso_id, usu_id);

            return View(retorno);
        }

        public ActionResult ImprimirCertificado(int curso_id)
        {
            var pdf = new Rotativa.ActionAsPdf("Certificado", new { curso_id, usu_id = Convert.ToInt32(Session["UsuId"]) });
            var orientacao = Rotativa.Options.Orientation.Landscape;
            pdf.PageOrientation = orientacao;
            return pdf;
        }

        public ActionResult CriarCurso()
        {
            return View();
        }

        public ActionResult CursoPorCategoria(int categoria_id)
        {

            var retorno = ca.CursosPorCategoria(categoria_id);

            return View(retorno);
        }

        public ActionResult AddFavorito(int cursoComprado_id)
        {
            ca.AddFavorito(cursoComprado_id);

            return RedirectToAction("MeusCursos", "Usuario");
        }

        public ActionResult RemoveFavorito(int cursoComprado_id)
        {
            ca.RemoveFavorito(cursoComprado_id);

            return RedirectToAction("MeusCursos", "Usuario");
        }

    }
}