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

namespace LCode.Models
{
    public class BancoDeDados : IDisposable
    {
        public MySqlConnection con;

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

        public void InsereCurso(Curso c, int user_id)
        {

            MySqlCommand cmd = new MySqlCommand("Insere_Curso", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("proc_curso_nome", c.Curso_nome);
            cmd.Parameters.AddWithValue("proc_curso_descricao", c.Curso_descricao);
            cmd.Parameters.AddWithValue("proc_curso_duracao", c.Curso_duracao);
            cmd.Parameters.AddWithValue("proc_curso_valor", c.Curso_valor);
            cmd.Parameters.AddWithValue("proc_curso_categoria", c.Curso_categoria);
            cmd.Parameters.AddWithValue("proc_usu_id", user_id);
            con.Open();

            cmd.ExecuteNonQuery();

        }

        public static List<SelectListItem> PopulaCurso()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["BdConexao"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = " SELECT curso_id, curso_nome FROM lc_curso";
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
                                Value = sdr["curso_id"].ToString()
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

        }
    }
}