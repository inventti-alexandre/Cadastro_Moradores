﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace Cadastro_Moradores_Condominio
{
    public class Visitante
    {
        #region Variaveis
        private int ID;
        private string Nome;
        private string Parentesco;
        private int IdResponsavel;
        public static int IDTeste;
        #endregion

        #region Construtores
        public Visitante()
        { }

        public Visitante(int pID, string pNome, string pParentesco)
        {
            this.ID = pID;
            this.Nome = pNome;
            this.Parentesco = pParentesco;
        }

        #endregion

        #region GETs e SETs
        public int Id
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string nome
        {
            get { return this.Nome; }
            set { this.Nome = value; }
        }

        public string parentesco
        {
            get { return this.Parentesco; }
            set { this.Parentesco = value; }
        }

        public int IDResponsavel
        {
            get { return this.IdResponsavel; }
            set { this.IdResponsavel = value; }
        }
        #endregion

        #region Constantes para o CRUD
        //Variaveis de conexao Add Referencia "System.Configurarion"
        public string strConexao = ConfigurationManager.ConnectionStrings["StringConexao"].ConnectionString;
        // Variaveis contantes para SQL para o CRUd
        public const string strInsert = "INSERT INTO Visitante Values(@ID, @nome, @Parentesco, @IDResponsavel)";
        public const string strDelete = "DELETE FROM Visitante where ID= @ID";
        public const string strUpdate = "UPDATE Visitante SET Nome=@nome, Parentesco=@Parentesco, IDResponsavel=@IDResponsavel WHERE ID=@Id";//ID=@ID,
        public const string strSelect = "SELECT v.ID, v.nome, v.parentesco, v.IdResponsavel FROM Visitante AS v INNER JOIN Morador AS m ON v.IdResponsavel = m.ID WHERE (v.IdResponsavel = @IdResponsavel)";
        public const string strSelectAll = "SELECT v.ID, v.nome, v.parentesco, v.IdResponsavel FROM Visitante AS v";
        public const string strSelectByName = "SELECT ID, nome, parentesco, IdResponsavel FROM Visitante AS v WHERE (nome LIKE @nome)";
        #endregion 

        #region Manipulaçao dos dados

        public void Salvar(int pID, string pNome, string pParentesco, int pIdResponsavel)
        {
            using (SqlConnection objConexao = new SqlConnection(strConexao))
            {
                using (SqlCommand objComando = new SqlCommand(strInsert, objConexao))
                {
                    objComando.Parameters.AddWithValue("@ID", pID);
                    objComando.Parameters.AddWithValue("@nome", pNome);
                    objComando.Parameters.AddWithValue("@Parentesco", pParentesco);
                    objComando.Parameters.AddWithValue("@IdResponsavel", pIdResponsavel);

                    objConexao.Open();
                    objComando.ExecuteNonQuery();
                    objConexao.Close();
                }
            }
        }

        public void Atualizar(int pID, string pNome, string pParentesco, int pIdResponsavel)
        {//int pID,
            using (SqlConnection objConexao = new SqlConnection(strConexao))
            {
                using (SqlCommand objComando = new SqlCommand(strUpdate, objConexao))
                {
                    objComando.Parameters.AddWithValue("@ID", pID);
                    objComando.Parameters.AddWithValue("@nome", pNome);
                    objComando.Parameters.AddWithValue("@Parentesco", pParentesco);
                    objComando.Parameters.AddWithValue("@IdResponsavel", pIdResponsavel);

                    objConexao.Open();
                    objComando.ExecuteNonQuery();
                    objConexao.Close();
                }
            }
        }

        public void Excluir(int Id)
        {
            using (SqlConnection objConexao = new SqlConnection(strConexao))
            {
                using (SqlCommand objComando = new SqlCommand(strDelete, objConexao))
                {
                    objComando.Parameters.AddWithValue("@ID", Id);

                    objConexao.Open();
                    objComando.ExecuteNonQuery();
                    objConexao.Close();
                }
            }
        }

        public List<Visitante> Selecionar()
        {
            List<Visitante> lstVisitantes = new List<Visitante>();

            using (SqlConnection objConexao = new SqlConnection(strConexao))
            {
                using (SqlCommand objComando = new SqlCommand(strSelect, objConexao))
                {
                    try
                    {
                        objComando.Parameters.AddWithValue("@IdResponsavel", Convert.ToString(IDTeste));
                        objConexao.Open();
                        SqlDataReader objDataReader = objComando.ExecuteReader();

                        if (objDataReader.HasRows)
                        {
                            while (objDataReader.Read())
                            {
                                Visitante objVisitante = new Visitante();
                                objVisitante.ID = Convert.ToInt32(objDataReader["ID"].ToString());
                                objVisitante.Nome = objDataReader["nome"].ToString();
                                objVisitante.Parentesco = objDataReader["Parentesco"].ToString();
                                objVisitante.IdResponsavel = Convert.ToInt32(objDataReader["IdResponsavel"].ToString());

                                lstVisitantes.Add(objVisitante);
                            }
                            objDataReader.Close();
                        }
                        objConexao.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro!" + ex.Message);
                       // throw;
                    }
                }
            }

            return lstVisitantes;
        }

        public List<Visitante> SelecionarTodos()
        {

            List<Visitante> lstVisitantes = new List<Visitante>();

            using (SqlConnection objConexao = new SqlConnection(strConexao))
            {
                using (SqlCommand objComando = new SqlCommand(strSelectAll, objConexao))
                {
                    try
                    {
                       objConexao.Open();
                        SqlDataReader objDataReader = objComando.ExecuteReader();

                        if (objDataReader.HasRows)
                        {
                            while (objDataReader.Read())
                            {
                                Visitante objVisitante = new Visitante();
                                objVisitante.ID = Convert.ToInt32(objDataReader["ID"].ToString());
                                objVisitante.Nome = objDataReader["nome"].ToString();
                                objVisitante.Parentesco = objDataReader["Parentesco"].ToString();
                                objVisitante.IdResponsavel = Convert.ToInt32(objDataReader["IdResponsavel"].ToString());

                                lstVisitantes.Add(objVisitante);
                            }
                            objDataReader.Close();
                        }
                        objConexao.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro!" + ex.Message);
                        //throw;
                    }
                }
            }

            return lstVisitantes;
        }

        public List<Visitante> SelecionarPorNome(string pNome)
        {

            List<Visitante> lstVisitantes = new List<Visitante>();

            using (SqlConnection objConexao = new SqlConnection(strConexao))
            {
                using (SqlCommand objComando = new SqlCommand(strSelectByName, objConexao))
                {
                    try
                    {
                        objComando.Parameters.AddWithValue("@Nome", "%"+pNome+"%");
                        objConexao.Open();
                        SqlDataReader objDataReader = objComando.ExecuteReader();

                        if (objDataReader.HasRows)
                        {
                            while (objDataReader.Read())
                            {
                                Visitante objVisitante = new Visitante();
                                objVisitante.ID = Convert.ToInt32(objDataReader["ID"].ToString());
                                objVisitante.Nome = objDataReader["nome"].ToString();
                                objVisitante.Parentesco = objDataReader["Parentesco"].ToString();
                                objVisitante.IdResponsavel = Convert.ToInt32(objDataReader["IdResponsavel"].ToString());

                                lstVisitantes.Add(objVisitante);
                            }
                            objDataReader.Close();
                        }
                        objConexao.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro!" + ex.Message);
                        //throw;
                    }
                }
            }

            return lstVisitantes;
        }
        #endregion
    }
}
