﻿using LCode.Models;
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

    }
}