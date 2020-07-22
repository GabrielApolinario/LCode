using LCode.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySqlX.XDevAPI;

namespace LCode.Dados
{
    public class UsuarioAcoes
    {
        BancoDeDados bd = new BancoDeDados();

        public void ValidaLogin(Usuarios u)
        {
            //Apos o forms estar criado, fazer validacao aq

            using (bd = new BancoDeDados())
            {
                string query = string.Format("SELECT * FROM lc_usuarios WHERE Usu_email = '{0}' AND Usu_Senha = '{1}' LIMIT 1;", u.Usu_email, u.Usu_senha);

                MySqlDataReader retorno;

                retorno = bd.RetornaComando(query);

                if (retorno.HasRows)
                {
                    while (retorno.Read())
                    {
                        {
                            u.Usu_id = Convert.ToInt32(retorno["usu_id"]);
                            u.Usu_nome = Convert.ToString(retorno["usu_nome"]);
                            u.Usu_email = Convert.ToString(retorno["usu_email"]);
                            u.Usu_senha = Convert.ToString(retorno["usu_senha"]);
                            u.Usu_hierarquia = Convert.ToString(retorno["usu_hierarquia"]);
                        }
                    }
                    bd.Dispose();
                }
                else
                {
                    u.Usu_email = null;
                    u.Usu_senha = null;
                    u.Usu_hierarquia = null;
                    bd.Dispose();
                }
            }

        }

        public Usuarios ValidarCadastro(Usuarios u)
        {
            using (bd = new BancoDeDados())
            {
                var strQuery = string.Format("SELECT * FROM lc_usuarios WHERE usu_email = '{0}' OR usu_cpf_or_cnpj = {1};", u.Usu_email, u.Usu_cpf_ou_cnpj);
                var retorno = bd.RetornaComando(strQuery);
                return VerificarCadastro(retorno).FirstOrDefault();
            }

        }

        public List<Usuarios> VerificarCadastro(MySqlDataReader retorno)
        {
            var usuarios = new List<Usuarios>();

            while (retorno.Read())
            {
                var TempUsuario = new Usuarios()
                {
                    Usu_email = retorno["usu_email"].ToString(),
                    Usu_cpf_ou_cnpj = retorno["usu_cpf_or_cnpj"].ToString(),
                };
                usuarios.Add(TempUsuario);
            }
            retorno.Close();
            return usuarios;
        }
    public void InsereUsuario(Usuarios u)
        {

            MySqlCommand cmd = new MySqlCommand("Insere_Usuario", bd.AbreConexao());
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

            cmd.ExecuteNonQuery();

        }

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
                        Usu_id = Convert.ToInt32(retorno["usu_id"]),
                        Usu_nome = retorno["usu_nome"].ToString(),
                        Usu_sobrenome = retorno["usu_sobrenome"].ToString(),
                        Usu_senha = retorno["usu_senha"].ToString(),
                        Usu_empresa = retorno["usu_empresa"].ToString(),
                        Usu_data_nasc = Convert.ToDateTime(retorno["usu_data_nasc"]),
                        Usu_cpf_ou_cnpj = retorno["usu_cpf_or_cnpj"].ToString(),
                        Usu_email = retorno["usu_email"].ToString(),
                        Imagem_link = retorno["usu_imagem"].ToString(),
                    };
                    usuarioDetalhe.Add(tempUsuario);
            }
            retorno.Close();
            bd.FecharConexao();
            return usuarioDetalhe.FirstOrDefault();
        }

        public void EditaUsuario(Usuarios u)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE lc_usuarios SET usu_nome = @proc_nome, usu_sobrenome = @proc_sobrenome, usu_senha = @proc_senha, usu_empresa = @proc_empresa, usu_imagem = @proc_imagem_link," +
                " usu_data_nasc = @proc_data_nasc WHERE usu_id = @proc_usu_id;", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@proc_nome", u.Usu_nome);
            cmd.Parameters.AddWithValue("@proc_usu_id", u.Usu_id); 
            cmd.Parameters.AddWithValue("@proc_sobrenome", u.Usu_sobrenome);
            cmd.Parameters.AddWithValue("@proc_senha", u.Usu_senha);
            cmd.Parameters.AddWithValue("@proc_empresa", u.Usu_empresa);
            cmd.Parameters.AddWithValue("@proc_data_nasc", u.Usu_data_nasc);
            cmd.Parameters.AddWithValue("@proc_imagem_link", u.Imagem_link);

            cmd.ExecuteNonQuery();
            bd.FecharConexao();
        }

        public List<Curso> GetMeusCursos(int usu_id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM lc_cursocomprado cc, lc_curso c WHERE cc.cursoComprado = c.curso_id AND cc.cursoComprado_usuario = @usu_id", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@usu_id", usu_id);
            var retorno = cmd.ExecuteReader();
            return MeusCursos(retorno);
        }

        public List<Curso> MeusCursos(MySqlDataReader retorno)
        {
            var cursosComprados = new List<Curso>();

            while (retorno.Read())
            {
                var TempCursos = new Curso()
                {
                    Curso_id = Convert.ToInt32(retorno["curso_id"]),
                    Curso_nome = retorno["curso_nome"].ToString(),
                    Curso_descricao = retorno["curso_descricao"].ToString(),
                    Curso_valor = Convert.ToDouble(retorno["curso_valor"]),
                    Imagem_link = retorno["curso_imagem"].ToString(),
                };
                cursosComprados.Add(TempCursos);
            }
            retorno.Close();
            return cursosComprados;
            
        }
    

    }
}