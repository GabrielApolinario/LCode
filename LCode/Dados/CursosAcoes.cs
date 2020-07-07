using LCode.Models;
using LCode.ViewModels;
using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LCode.Dados
{
    public class CursosAcoes
    {
        BancoDeDados bd = new BancoDeDados();

        public List<Curso> PesquisarCursos(string pesquisa)
        {
            string query = String.Format("SELECT * FROM lc_curso WHERE curso_nome LIKE '%{0}%' OR curso_categoria LIKE '%{0}%';", pesquisa);
            MySqlCommand cmd = new MySqlCommand(query, bd.AbreConexao());
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
                    Curso_id = Convert.ToInt32(retorno["Curso_id"]),
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

        public List<Curso> ListarCursos()
        {

            var strQuery = "SELECT * FROM lc_Curso ORDER BY curso_categoria, curso_nome asc;";
            MySqlCommand cmd = new MySqlCommand(strQuery, bd.AbreConexao());
            var retorno = cmd.ExecuteReader();
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

        public CursoVideoModuloViewModel QueryDetalhesCurso(int curso_id)
        {

            //MySqlCommand cmd = new MySqlCommand("Curso_Completo", bd.AbreConexao());
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("proc_curso_id", curso_id);
            //var retorno = cmd.ExecuteReader();
            //return DetalhesCurso(retorno).FirstOrDefault();

            MySqlCommand cmd = new MySqlCommand("Select * from lc_curso where curso_id = @proc_curso_id", bd.AbreConexao());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("proc_curso_id", curso_id);
            var retorno = cmd.ExecuteReader();
            return DetalhesCurso(retorno).FirstOrDefault();

        }

        //Retorna lista de cursos cadastrados
        public List<CursoVideoModuloViewModel> DetalhesCurso(MySqlDataReader retorno)
        {
            var cursos = new List<CursoVideoModuloViewModel>();

            if (retorno.HasRows)
            {
                while (retorno.Read())
                {

                    var TempCurso = new CursoVideoModuloViewModel()

                    {
                        Curso_id = Convert.ToInt32(retorno["curso_id"]),
                        Curso_nome = retorno["curso_nome"].ToString(),
                        Curso_descricao = retorno["curso_descricao"].ToString(),
                        Curso_duracao = Convert.ToDouble(retorno["curso_duracao"]),
                        Curso_valor = Convert.ToDouble(retorno["curso_valor"]),

                    };


                    cursos.Add(TempCurso);
                }
            }
                retorno.Close();
                bd.FecharConexao();
                return cursos;

        }
        public List<CursoComprado> ValidarCursoComprado(int usu_id)
        {
            using (bd = new BancoDeDados())
            {
                var strQuery = string.Format("SELECT * FROM lc_cursoComprado WHERE cursoComprado_usuario = '{0}';", usu_id);
                var retorno = bd.RetornaComando(strQuery);
                return VerificarCompra(retorno);
            }

        }

        public List<CursoComprado> VerificarCompra(MySqlDataReader retorno)
        {
            var usuarios = new List<CursoComprado>();

            while (retorno.Read())
            {
                var TempUsuario = new CursoComprado()
                {
                    cursoComprado_usuario = Convert.ToInt32(retorno["cursoComprado_usuario"]),
                    cursoComprado = Convert.ToInt32(retorno["cursoComprado"]),
                };
                usuarios.Add(TempUsuario);
            }
            retorno.Close();
            return usuarios;
        }
    }
}
