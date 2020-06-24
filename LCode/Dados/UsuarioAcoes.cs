using LCode.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace LCode.Dados
{
    public class UsuarioAcoes
    {
        BancoDeDados bd = new BancoDeDados();


        public Usuarios GetUsuarios(int id_usu)
        {
            MySqlCommand cmd = new MySqlCommand("Select * from lc_usuarios WHERE usu_id = @proc_usu_id;", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@proc_usu_id", id_usu);
            cmd.CommandType = CommandType.Text;
            var retorno = cmd.ExecuteReader();

            var usuarioDetalhe = new List<Usuarios>();

            if (retorno.HasRows)
            {
                    retorno.Read();
                    var tempUsuario = new Usuarios()
                    {
                        Usu_nome = retorno["usu_nome"].ToString(),
                        Usu_sobrenome = retorno["usu_sobrenome"].ToString(),
                        Usu_senha = retorno["usu_senha"].ToString(),
                        Usu_empresa = retorno["usu_empresa"].ToString(),
                        Usu_data_nasc = Convert.ToDateTime(retorno["usu_data_nasc"]),
                        Usu_cpf_ou_cnpj = retorno["usu_cpf_or_cnpj"].ToString(),
                        Usu_email = retorno["usu_email"].ToString(),
                    };
                    usuarioDetalhe.Add(tempUsuario);
            }
            retorno.Close();
            bd.FecharConexao();
            return usuarioDetalhe.FirstOrDefault();
        }

        public void EditaUsuario(Usuarios u)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE lc_usuarios SET usu_nome = @proc_nome, usu_sobrenome = @proc_sobrenome, usu_senha = @proc_senha, usu_empresa = @proc_empresa," +
                " usu_data_nasc = @proc_data_nasc;", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@proc_nome", u.Usu_nome);
            cmd.Parameters.AddWithValue("@proc_sobrenome", u.Usu_sobrenome);
            cmd.Parameters.AddWithValue("@proc_senha", u.Usu_senha);
            cmd.Parameters.AddWithValue("@proc_empresa", u.Usu_empresa);
            cmd.Parameters.AddWithValue("@proc_data_nasc", u.Usu_data_nasc);

            cmd.ExecuteNonQuery();
            bd.FecharConexao();
        }
    

    }
}