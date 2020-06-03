using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;

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
    }
}