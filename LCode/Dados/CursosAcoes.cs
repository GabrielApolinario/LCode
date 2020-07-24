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
            string query = String.Format("SELECT * FROM lc_curso WHERE curso_nome LIKE '%{0}%' OR curso_categoria LIKE '%{0}%' AND curso_status = 1;", pesquisa);
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
                    Imagem_link = retorno["Curso_imagem"].ToString(),
                };
                cursosPesquisados.Add(TempCursos);
            }
            retorno.Close();
            bd.FecharConexao();
            return cursosPesquisados;

        }

        public List<Curso> ListarCursos()
        {

            var strQuery = "SELECT * FROM lc_Curso WHERE curso_status = 1 ORDER BY curso_categoria, curso_nome asc;";
            MySqlCommand cmd = new MySqlCommand(strQuery, bd.AbreConexao());
            var retorno = cmd.ExecuteReader();
            return ListaDeCursos(retorno);
        }

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
                    Curso_valor = Convert.ToDouble(retorno["curso_valor"]),
                    Imagem_link = retorno["curso_imagem"].ToString(),
                };
                cursos.Add(TempCurso);
            }
            retorno.Close();
            return cursos;
        }

        public CursoVideoModuloViewModel QueryDetalhesCurso(int curso_id)
        {

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

        public Video DetalhesVideo(int video_id)
        {
            MySqlCommand cmd = new MySqlCommand("Select * from lc_video where video_id = @video", bd.AbreConexao());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@video", video_id);
            var retorno = cmd.ExecuteReader();
            return RetornarVideo(retorno);
        }

        public Video RetornarVideo(MySqlDataReader retorno)
        {
                Video vid = new Video();
            if (retorno.HasRows)
            {
                retorno.Read();

                vid.video_id = Convert.ToInt32(retorno["video_id"]);
                vid.video_link = retorno["video_link"].ToString();
                vid.video_titulo = retorno["video_titulo"].ToString();
                vid.video_modulo = Convert.ToInt32(retorno["video_modulo"]);
                vid.video_descricao = retorno["video_descricao"].ToString();
            }
            retorno.Close();
            return vid;

        }

        public Video VideoPadrao(int curso_id)
        {
            MySqlCommand cmd = new MySqlCommand("Select * from lc_video where video_curso = @curso_id ORDER BY video_id LIMIT 1", bd.AbreConexao());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@curso_id", curso_id);
            var retorno = cmd.ExecuteReader();
            return RetornarVideoPadrao(retorno);
        }

        public Video RetornarVideoPadrao(MySqlDataReader retorno)
        {
            Video vid = new Video();
            if (retorno.HasRows)
            {
                retorno.Read();

                vid.video_id = Convert.ToInt32(retorno["video_id"]);
                vid.video_link = retorno["video_link"].ToString();
                vid.video_titulo = retorno["video_titulo"].ToString();
                vid.video_modulo = Convert.ToInt32(retorno["video_modulo"]);
                vid.video_descricao = retorno["video_descricao"].ToString();
            }
            retorno.Close();
            return vid;

        }

        public List<Curso> CursosPorCategoria(int categoria_id)
        {
            MySqlCommand cmd = new MySqlCommand("Select * from lc_curso where curso_categoria = @categoria_id AND curso_status = 1 ORDER BY curso_nome", bd.AbreConexao());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@categoria_id", categoria_id);
            var retorno = cmd.ExecuteReader();
            return GetCursosPorCategoria(retorno);
        }

        public List<Curso> GetCursosPorCategoria(MySqlDataReader retorno)
        {
            var cursos = new List<Curso>();

            while (retorno.Read())
            {
                var TempCurso = new Curso()
                {
                    Curso_id = Convert.ToInt32(retorno["curso_id"]),
                    Curso_nome = retorno["curso_nome"].ToString(),
                    Curso_descricao = retorno["curso_descricao"].ToString(),
                    Curso_valor = Convert.ToDouble(retorno["curso_valor"]),
                    Imagem_link = retorno["curso_imagem"].ToString(),
                };
                cursos.Add(TempCurso);
            }
            retorno.Close();
            return cursos;
        }

        public UsuarioCursoViewModel GetCertificado(int curso_id, int usu_id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM lc_certificado ct, lc_curso c, lc_usuarios u WHERE ct.certificado_curso_comprado = c.curso_id " +
                "AND ct.certificado_usuario = u.usu_id " +
                "AND ct.certificado_curso_comprado = @proc_curso_id " +
                "AND u.usu_id = @proc_usu_id; ", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@proc_curso_id", curso_id);
            cmd.Parameters.AddWithValue("@proc_usu_id", usu_id);
            var retorno = cmd.ExecuteReader();

            MySqlDataReader rt = retorno;
            UsuarioCursoViewModel uc = new UsuarioCursoViewModel();

            if (rt.HasRows)
            {
                rt.Read();
                uc.Curso_id = Convert.ToInt32(retorno["curso_id"]);
                uc.Curso_nome = retorno["curso_nome"].ToString();
                uc.Usu_nome = retorno["usu_nome"].ToString();
                uc.Curso_duracao = Convert.ToDouble(retorno["curso_duracao"]);                
            }

            bd.FecharConexao();
            return uc;
        }

        public void AddFavorito(int cursoComprado_id)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE lc_cursocomprado SET cursoComprado_favorito = 1 WHERE cursoComprado_id = @proc_cursoComprado_id", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@proc_cursoComprado_id", cursoComprado_id);
            cmd.ExecuteNonQuery();
            bd.FecharConexao();

        }

        public void RemoveFavorito(int cursoComprado_id)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE lc_cursocomprado SET cursoComprado_favorito = 0 WHERE cursoComprado_id = @proc_cursoComprado_id", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@proc_cursoComprado_id", cursoComprado_id);
            cmd.ExecuteNonQuery();
            bd.FecharConexao();
        }
    }
}
