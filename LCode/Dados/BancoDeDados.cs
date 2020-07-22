using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Renci.SshNet;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using LCode.Controllers;
using LCode.ViewModels;
using System.Drawing.Design;

namespace LCode.Models
{
    public class BancoDeDados : IDisposable
    {

        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["BdConexao"].ConnectionString);
        public int novo_curso_id;
        public int mod_id;

        public MySqlConnection AbreConexao()
        {
            con.Open();

            return con;
        }

        public MySqlConnection FecharConexao()
        {
            con.Close();
            con.Dispose();

            return con;
        }


        public void Dispose()
        {
            con.Dispose();
            con.Close();
        }

        public MySqlDataReader RetornaComando(string query)
        {

            var cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;

            con.Open();
            return cmd.ExecuteReader();
        }

 
        public int InsereCurso(Curso c, int user_id)
        {
            MySqlCommand cmd = new MySqlCommand("Insere_Curso", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("proc_curso_nome", c.Curso_nome);
            cmd.Parameters.AddWithValue("proc_curso_descricao", c.Curso_descricao);
            cmd.Parameters.AddWithValue("proc_curso_duracao", c.Curso_duracao);
            cmd.Parameters.AddWithValue("proc_curso_valor", c.Curso_valor);
            cmd.Parameters.AddWithValue("proc_curso_categoria", c.Curso_categoria);
            cmd.Parameters.AddWithValue("proc_curso_imagem", c.Imagem_link);
            cmd.Parameters.AddWithValue("proc_usu_id", user_id);
            cmd.Parameters.Add("novo_curso_id", MySqlDbType.Int32);
            cmd.Parameters["novo_curso_id"].Direction = ParameterDirection.Output;
            con.Open();

            cmd.ExecuteNonQuery();
            novo_curso_id = Convert.ToInt32(cmd.Parameters["novo_curso_id"].Value);
            con.Dispose();

            return novo_curso_id;
        }

        public static List<SelectListItem> PopulaCurso(int id_usu)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["BdConexao"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {

                string query = string.Format("SELECT c.curso_nome, d.digital_prof, u.usu_nome, d.digital_nome "
                + "FROM lc_curso c, lc_digital d, lc_usuarios u "
                + "WHERE d.digital_prof = u.usu_id "
                + "AND d.digital_nome = c.curso_id "
                + "AND u.usu_id = {0};", id_usu);
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
                                Text = sdr["curso_nome"].ToString(),
                                Value = sdr["digital_nome"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulaCategorias()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["BdConexao"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = " SELECT categoria_id, categoria_nome FROM lc_categoria";
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
                                Text = sdr["categoria_nome"].ToString(),
                                Value = sdr["categoria_id"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulaPais()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["BdConexao"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = " SELECT pais_id, pais_nome FROM lc_pais";
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
                                Text = sdr["pais_nome"].ToString(),
                                Value = sdr["pais_id"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        public void InsereVideo(Video v)
        {
            string query = string.Format("Insert into lc_Video (video_titulo, video_descricao, video_curso, video_link, video_modulo, video_status) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')"
                , v.video_titulo, v.video_descricao, v.video_curso, v.video_link, v.video_modulo, 1);

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            cmd.ExecuteNonQuery();
        }

        public int InsereModulo(Modulo m)
        {
            string query = string.Format("Insere_Modulo");

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("proc_mod_curso", m.mod_curso);
            cmd.Parameters.AddWithValue("proc_mod_nome", m.mod_nome);
            cmd.Parameters.AddWithValue("proc_mod_desc", m.mod_desc);
            cmd.Parameters.Add("novo_mod_id", MySqlDbType.Int32);
            cmd.Parameters["novo_mod_id"].Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();

            mod_id = Convert.ToInt32(cmd.Parameters["novo_mod_id"].Value);

            con.Close();

            return mod_id;
        }

        public List<CursoVideoModuloViewModel> QueryVideos(int curso_id)
        {

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM lc_video WHERE video_curso = @proc_curso_id ORDER BY video_id ASC;", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@proc_curso_id", curso_id);
            con.Open();
            var retorno = cmd.ExecuteReader();       
            return ListarVideos(retorno);

        }

        public List<CursoVideoModuloViewModel> ListarVideos(MySqlDataReader retorno)
        {
            var videosLista = new List<CursoVideoModuloViewModel>();

            if (retorno.HasRows)
            {
                while (retorno.Read())
                {
                    var tempVideos = new CursoVideoModuloViewModel()
                    {
                        video_id = Convert.ToInt32(retorno["video_id"]),
                        video_titulo = retorno["video_titulo"].ToString(),
                        video_modulo = Convert.ToInt32(retorno["video_modulo"]),
                        video_link = retorno["video_link"].ToString(),
                        video_descricao = retorno["video_descricao"].ToString(),                       
                        
                    };
                    videosLista.Add(tempVideos);
                }
            }
            retorno.Close();
            FecharConexao();
            return videosLista;

        }

        public List<CursoVideoModuloViewModel> QueryModulos(int curso_id)
        {

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM lc_modulo where mod_curso = @proc_curso_id ORDER BY mod_id ASC;", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@proc_curso_id", curso_id);
            con.Open();
            var retorno = cmd.ExecuteReader();
            return ListarModulos(retorno);

        }

        public List<CursoVideoModuloViewModel> ListarModulos(MySqlDataReader retorno)
        {
            var videosLista = new List<CursoVideoModuloViewModel>();

            if (retorno.HasRows)
            {
                while (retorno.Read())
                {
                    var tempVideos = new CursoVideoModuloViewModel()
                    {
                        mod_id = Convert.ToInt32(retorno["mod_id"]),
                        mod_nome = retorno["mod_nome"].ToString(),

                    };
                    videosLista.Add(tempVideos);
                }
            }
            retorno.Close();
            FecharConexao();
            return videosLista;

        }

        public List<Categoria> BuscaCategorias()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM lc_categoria", AbreConexao());
            var retorno = cmd.ExecuteReader();
            return MeusCursos(retorno);
        }

        public List<Categoria> MeusCursos(MySqlDataReader retorno)
        {
            var categorias = new List<Categoria>();

            while (retorno.Read())
            {
                var TempCategorias = new Categoria()
                {
                    Categoria_id = Convert.ToInt32(retorno["categoria_id"]),
                    Categoria_nome = retorno["categoria_nome"].ToString(),
                };
                categorias.Add(TempCategorias);
            }
            retorno.Close();
            return categorias;

        }

    }
}