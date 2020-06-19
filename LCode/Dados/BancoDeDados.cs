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

namespace LCode.Models
{
    public class BancoDeDados : IDisposable
    {
        public MySqlConnection con;
        public int novo_curso_id;
        public int mod_id;

        public BancoDeDados()
        {
            con = new MySqlConnection(ConfigurationManager.ConnectionStrings["BdConexao"].ConnectionString);

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

        public List<Curso> ListarCursos()
        {
            
            var strQuery = "SELECT * FROM lc_Curso ORDER BY curso_categoria, curso_nome asc;";
            MySqlCommand cmd = new MySqlCommand(strQuery, con);
            var retorno = RetornaComando(strQuery);
            return ListaDeCursos(retorno);
        }

        //Retorna lista de cursos cadastrados
        public List<Curso> ListaDeCursos(MySqlDataReader retorno)
        {
            var cursos = new List<Curso>();

            while (retorno.Read())
            {
                var TempCurso = new Curso()
                {
                    Curso_id = Convert.ToInt32(retorno["curso_id"]),
                    Curso_nome = retorno["curso_nome"].ToString(),
                    Curso_descricao = retorno["curso_descricao"].ToString(),
                };
                cursos.Add(TempCurso);
            }
            retorno.Close();
            return cursos;
        }

        public void InsereUsuario(Usuarios u)
        {

            MySqlCommand cmd = new MySqlCommand("Insere_Usuario", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("proc_usu_email", u.Usu_email);
            cmd.Parameters.AddWithValue("proc_usu_nome", u.Usu_nome);
            cmd.Parameters.AddWithValue("proc_usu_sobrenome", u.Usu_sobrenome);
            cmd.Parameters.AddWithValue("proc_usu_senha", u.Usu_senha);
            cmd.Parameters.AddWithValue("proc_usu_hierarquia", u.Usu_hierarquia);
            cmd.Parameters.AddWithValue("proc_usu_empresa", u.Usu_empresa);
            cmd.Parameters.AddWithValue("proc_usu_pais", u.Usu_pais);
            cmd.Parameters.AddWithValue("proc_usu_data_nasc", u.Usu_data_nasc);
            cmd.Parameters.AddWithValue("proc_usu_cpf", u.Usu_cpf_ou_cnpj);

            con.Open();

            cmd.ExecuteNonQuery();

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
            cmd.Parameters.AddWithValue("proc_mod_qtd_video", m.mod_qtd_video);
            cmd.Parameters.Add("novo_mod_id", MySqlDbType.Int32);
            cmd.Parameters["novo_mod_id"].Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();

            mod_id = Convert.ToInt32(cmd.Parameters["novo_mod_id"].Value);

            con.Close();

            return mod_id;
        }

        public List<CursoVideoModuloViewModel> QueryDetalhesCurso(CursoVideoModuloViewModel cmv)
        {
            
            MySqlCommand cmd = new MySqlCommand("Curso_Completo", con);
            cmd.Parameters.AddWithValue("proc_curso_id", cmv.Curso_id); 
            var retorno = cmd.ExecuteReader();
            return DetalhesCurso(retorno);
        }

        //Retorna lista de cursos cadastrados
        public List<CursoVideoModuloViewModel> DetalhesCurso(MySqlDataReader retorno)
        {
            var cursos = new List<CursoVideoModuloViewModel>();

            while (retorno.Read())
            {
                var TempCurso = new CursoVideoModuloViewModel()
                {
                    Curso_id = Convert.ToInt32(retorno["curso_id"]),                    
                    Curso_nome = retorno["curso_nome"].ToString(),
                    Curso_descricao = retorno["curso_descricao"].ToString(),
                    Curso_duracao = Convert.ToDouble(retorno["curso_duracao"]),
                    Curso_valor = Convert.ToDouble(retorno["curso_valor"]),

                    mod_nome = retorno["mod_nome"].ToString(),
                    mod_desc = retorno["mod_desc"].ToString(),

                    video_titulo = retorno["video_titulo"].ToString(),
                    video_id = Convert.ToInt32(retorno["video_id"]),
                    video_link = retorno["video_link"].ToString(),
                    video_modulo = Convert.ToInt32(retorno["video_modulo"]),
                    video_curso = Convert.ToInt32(retorno["video_titulo"]),
                    video_descricao = retorno["video_descricao"].ToString(),                    
                };
                cursos.Add(TempCurso);
            }
            retorno.Close();
            return cursos;
        }
    }
}