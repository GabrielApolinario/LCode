using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCode.Models;
using LCode.Dados;
using MySql.Data.MySqlClient;
using System.Web.Mvc;
using System.Data;
using Renci.SshNet.Security.Cryptography;

namespace LCode.Dados
{
    public class Login
    {
        //private BancoDeDados bd;

        BancoDeDados bd = new BancoDeDados();
        public void ValidaLogin(Usuarios u)
        {
            //Apos o forms estar criado, fazer validacao aq

            using (bd = new BancoDeDados())
            {
                string query = string.Format("SELECT usu_nome, usu_email, usu_senha, usu_hierarquia FROM lc_usuarios WHERE Usu_email = '{0}' AND Usu_Senha = '{1}' LIMIT 1;", u.Usu_email, u.Usu_senha);

                MySqlDataReader retorno;

                retorno = bd.RetornaComando(query);

                if (retorno.HasRows)
                {
                    while (retorno.Read())
                    {
                        {
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
                var strQuery = string.Format("SELECT * FROM lc_usuarios WHERE usu_email = '{0}';", u.Usu_email);
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
    }
}