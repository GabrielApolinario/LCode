using LCode.Models;
using LCode.ViewModels;
using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
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
            string query = String.Format("SELECT * FROM lc_curso WHERE curso_nome LIKE '%{0}%';", pesquisa);
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
                        //mod_id = Convert.ToInt32(retorno["mod_id"]),
                        //mod_nome = retorno["mod_nome"].ToString(),
                        //mod_desc = retorno["mod_desc"].ToString(),

                    };


                    cursos.Add(TempCurso);
                }
            }
                retorno.Close();
                bd.FecharConexao();
                return cursos;

        }

        //public List<CursoVideoModuloViewModel> QueryDetalhesCurso(int curso_id)

        //{

        //    List<CursoVideoModuloViewModel> modulos = new List<CursoVideoModuloViewModel>();

        //    MySqlCommand cmd = new MySqlCommand("select * from lc_modulo WHERE mod_curso = @proc_curso_id", bd.AbreConexao());
        //    cmd.Parameters.AddWithValue("@proc_curso_id", curso_id);

        //    MySqlDataAdapter sd = new MySqlDataAdapter(cmd);

        //    DataTable dt = new DataTable();

        //    sd.Fill(dt);

        //    bd.FecharConexao();

        //    foreach (DataRow retorno in dt.Rows)
        //    {
        //        var TempCurso = new CursoVideoModuloViewModel()

        //        {
        //            mod_id = Convert.ToInt32(retorno["mod_id"]),
        //            mod_nome = retorno["mod_nome"].ToString(),
        //            mod_desc = retorno["mod_desc"].ToString(),
        //        };
        //        modulos.Add(TempCurso);

        //        MySqlCommand cmd_video = new MySqlCommand("select * from lc_video WHERE video_modulo = @proc_mod_id", bd.AbreConexao());
        //        cmd_video.Parameters.AddWithValue("@proc_mod_id", TempCurso.mod_id);

        //        MySqlDataAdapter sd_video = new MySqlDataAdapter(cmd_video);

        //        DataTable dt_video = new DataTable();

        //        sd_video.Fill(dt_video);
        //        foreach (DataRow retorno_video in dt_video.Rows)

        //        {
        //            modulos.Add(
        //            new CursoVideoModuloViewModel
        //            {
        //                video_titulo = retorno_video["video_titulo"].ToString(),
        //            });
        //        }

        //        return modulos;

        //    }
        //    return modulos;
        //}
    }
}
            //if (retorno.HasRows)
            //{
            //    while (retorno.Read())
            //    {

            //        var TempCurso = new CursoVideoModuloViewModel()

            //        {
            //            Curso_id = Convert.ToInt32(retorno["curso_id"]),
            //            Curso_nome = retorno["curso_nome"].ToString(),
            //            Curso_descricao = retorno["curso_descricao"].ToString(),
            //            Curso_duracao = Convert.ToDouble(retorno["curso_duracao"]),
            //            Curso_valor = Convert.ToDouble(retorno["curso_valor"]),

            //            mod_nome = retorno["mod_nome"].ToString(),
            //            mod_desc = retorno["mod_desc"].ToString(),

            //            video_titulo = retorno["video_titulo"].ToString(),
            //            video_id = Convert.ToInt32(retorno["video_id"]),
            //            video_link = retorno["video_link"].ToString(),
            //            video_modulo = Convert.ToInt32(retorno["video_modulo"]),
            //            video_curso = Convert.ToInt32(retorno["video_curso"]),
            //            video_descricao = retorno["video_descricao"].ToString(),
            //        };


            //        cursos.Add(TempCurso);
            //    }

            //}
            //retorno.Close();
            //bd.FecharConexao();
            //return cursos;
