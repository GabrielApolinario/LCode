using LCode.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCode.Dados
{
    public class CursosAcoes
    {
        BancoDeDados bd = new BancoDeDados();

        public List<Curso> PesquisarCursos(string pesquisa)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM lc_cursoComprado WHERE curso_nome LIKE '%@pesquisa%'", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@pesquisa", pesquisa);
            var retorno = cmd.ExecuteReader();
            return GetCursos(retorno);

        }

        public List<Curso> GetCursos(MySqlDataReader retorno)
        {
            var cursosPesquisados = new List<Curso>();


            while (retorno.Read())
            {
                var TempCursos = new Curso()
                {
                    Curso_nome = retorno["Curso_nome"].ToString(),
                    Curso_descricao = retorno["Curso_descricao"].ToString(),
                    Curso_valor = Convert.ToDouble(retorno["Curso_valor"]),                   
                };
                cursosPesquisados.Add(TempCursos);
            }
            retorno.Close();
            bd.FecharConexao();
            return cursosPesquisados;

        }
    }
}