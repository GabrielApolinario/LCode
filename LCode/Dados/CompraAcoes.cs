using LCode.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LCode.Dados
{
    public class CompraAcoes
    {
        BancoDeDados bd = new BancoDeDados();
       

        public Curso CursoCarrinho(int curso_id)
        {

            MySqlCommand cmd = new MySqlCommand("Select * from lc_curso where curso_id = @proc_curso_id", bd.AbreConexao());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("proc_curso_id", curso_id);
            var retorno = cmd.ExecuteReader();
            return DetalhesCurso(retorno).FirstOrDefault();

        }

        //Retorna lista de cursos cadastrados
        public List<Curso> DetalhesCurso(MySqlDataReader retorno)
        {
            var cursos = new List<Curso>();

            if (retorno.HasRows)
            {
                while (retorno.Read())
                {
                    var TempCurso = new Curso()
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

        public void CompraCurso(int curso_id, int usu_id, string formaPagamento)
        {
            string command = string.Format("INSERT INTO lc_compra (compra_curso, compra_data, compra_hora, compra_usu, compra_status, compra_pag_forma) VALUES ({0},'{1}','{2}',{3},'{4}','{5}')",
               curso_id, DateTime.Today.ToString("yyyyMMdd"),DateTime.Now.TimeOfDay ,usu_id, "PAGO", formaPagamento);
            
            MySqlCommand cmd = new MySqlCommand(command, bd.AbreConexao());
            cmd.ExecuteNonQuery();
            bd.FecharConexao();
        }

        public CursoComprado CursoJaComprado(int usu_id, int curso_id)
        {
            using (bd = new BancoDeDados())
            {
                var strQuery = string.Format("SELECT * FROM lc_cursoComprado WHERE cursoComprado_usuario = '{0}' AND cursoComprado = {1};", usu_id, curso_id);
                var retorno = bd.RetornaComando(strQuery);
                return VerificarCompra(retorno);
            }

        }

        public CursoComprado VerificarCompra(MySqlDataReader retorno)
        {
            CursoComprado cursoComprado = new CursoComprado();

            if (retorno.HasRows)
            {
                retorno.Read();
                cursoComprado.cursoComprado_usuario = Convert.ToInt32(retorno["cursoComprado_usuario"]);
                cursoComprado.cursoComprado = Convert.ToInt32(retorno["cursoComprado"]);
                retorno.Close();
                return cursoComprado;
            }
            else
            {
                retorno.Close();
                return null;
            }
        }
    }
}