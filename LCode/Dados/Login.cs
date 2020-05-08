using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCode.Models;
using LCode.Dados;
using MySql.Data.MySqlClient;
using System.Web.Mvc;

namespace LCode.Dados
{
    public class Login
    {
        private BancoDeDados bd;

        
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
    }
}