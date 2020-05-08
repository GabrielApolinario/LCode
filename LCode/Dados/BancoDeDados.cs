using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace LCode.Models
{
    public class BancoDeDados : IDisposable
    {
        private readonly MySqlConnection con;
        public BancoDeDados()
        {
            con = new MySqlConnection(ConfigurationManager.ConnectionStrings["BdConexao"].ConnectionString);      

        }
         
        public void ExecutaComando(string query)
        {
            var cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public MySqlDataReader RetornaComando(string query)
        {
            var cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            return cmd.ExecuteReader();
        }

        public void Dispose()
        {
            con.Dispose();
            con.Close();
        }
    }
}