using LCode.Models;
using LCode.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCode.Dados
{
    public class ProfessorAcoes
    {
        BancoDeDados bd = new BancoDeDados();
        public List<Curso> CursosCriados(int usu_id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT * FROM lc_curso, lc_digital, lc_categoria WHERE digital_prof = @usu_id AND curso_id = digital_nome AND curso_categoria = categoria_id;", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@usu_id", usu_id);
            var retorno = cmd.ExecuteReader();
            return GetCursosCriados(retorno);
        }

        public List<Curso> GetCursosCriados(MySqlDataReader retorno)
        {
            List <Curso> c = new List<Curso>();

            while (retorno.Read())
            {
                c.Add(new Curso
                {
                    Curso_id = Convert.ToInt32(retorno["curso_id"]),
                    Curso_nome = retorno["curso_nome"].ToString(),
                    Curso_descricao = retorno["curso_descricao"].ToString(),
                    Categoria_nome = retorno["categoria_nome"].ToString(),
                    Curso_status = Convert.ToInt32(retorno["curso_status"]),
                });
            }
            retorno.Close();
            bd.FecharConexao();
            return c;
        }

        public List<CursoVideoModuloViewModel> EditarCurso(int curso_id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT curso_id, curso_nome, curso_descricao, curso_status, video_id, video_id, video_titulo, video_descricao, video_link FROM lc_curso, lc_video" +
                " WHERE curso_id = video_curso and curso_id = @curso_id;"
                , bd.AbreConexao());
            cmd.Parameters.AddWithValue("@curso_id", curso_id);
            var retorno = cmd.ExecuteReader();
            return GetEditarCurso(retorno);
        }

        public List<CursoVideoModuloViewModel> GetEditarCurso(MySqlDataReader retorno)
        {
            List<CursoVideoModuloViewModel> c = new List<CursoVideoModuloViewModel>();

            while (retorno.Read())
            {
                c.Add(new CursoVideoModuloViewModel
                {
                    Curso_id = Convert.ToInt32(retorno["curso_id"]),
                    Curso_nome = retorno["curso_nome"].ToString(),
                    Curso_descricao = retorno["curso_descricao"].ToString(),                   
                    Curso_status = Convert.ToInt32(retorno["curso_status"]),
                    video_id = Convert.ToInt32(retorno["video_id"]),
                    video_titulo = retorno["video_titulo"].ToString(),
                    video_descricao = retorno["video_descricao"].ToString(),
                    video_link = retorno["video_link"].ToString(),
                });
            }
            retorno.Close();
            return c;
        }

        public void EditaVideo(CursoVideoModuloViewModel c)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE lc_video SET video_titulo = @video_titulo, video_descricao = @video_descricao, video_link = @video_link WHERE video_id = @video_id", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@video_titulo", c.video_titulo);
            cmd.Parameters.AddWithValue("@video_descricao", c.video_descricao);
            cmd.Parameters.AddWithValue("@video_link", c.video_link);
            cmd.Parameters.AddWithValue("@video_id", c.video_id);
            cmd.ExecuteNonQuery();
            bd.FecharConexao();
        }

        public CursoVideoModuloViewModel DetalhesVideo(int video_id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM lc_video WHERE video_id = @video_id", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@video_id", video_id);
            var retorno = cmd.ExecuteReader();
            return GetEditarVideo(retorno);
        }

        public CursoVideoModuloViewModel GetEditarVideo(MySqlDataReader retorno)
        {
            CursoVideoModuloViewModel v = new CursoVideoModuloViewModel();

            retorno.Read();

            v.video_id = Convert.ToInt32(retorno["video_id"]);
            v.video_titulo = retorno["video_titulo"].ToString();
            v.video_descricao = retorno["video_descricao"].ToString();
            v.video_link = retorno["video_link"].ToString();

            retorno.Close();
            bd.FecharConexao();
            return v;           
        }

        public void AtivarCurso(int curso_id)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE lc_curso SET curso_status = 1 WHERE curso_id = @curso_id", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@curso_id", curso_id);
            cmd.ExecuteNonQuery();
            bd.FecharConexao();
        }

        public void DesativarCurso(int curso_id)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE lc_curso SET curso_status = 0 WHERE curso_id = @curso_id", bd.AbreConexao());
            cmd.Parameters.AddWithValue("@curso_id", curso_id);
            cmd.ExecuteNonQuery();
            bd.FecharConexao();
        }
    }
}