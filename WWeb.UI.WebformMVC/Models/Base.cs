using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWeb.UI.WebformMVC.Models.DAL
{
    public class Base<T>
    {
        /// <summary>
        /// Retorna Toda a tabela
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <returns>Lista do tipo de Entidade informada</returns>
        public virtual List<T> Select()
        {

            Auxiliar.Atributos atributos = Auxiliar.RetornoAtributos<T>((T)Activator.CreateInstance(typeof(T)));

            List<T> LObjeto = new List<T>();

            using (SqlConnection Conn = new SqlConnection(Conexao.strConn))
            {
                using (SqlCommand cmd = new SqlCommand(atributos.NomeProcedureListarTodos, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        Conn.Open();
                        using (SqlDataReader Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LObjeto.Add(Auxiliar.RetornaDadosEntidade<T>(Dr));
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return LObjeto;
        }

        /// <summary>
        /// Perquisa o Registro pela Chave Primaria da Tabela
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor da Chave Primaria</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual T SelectPelaPK(int Id)
        {
            T _entidade = (T)Activator.CreateInstance(typeof(T));

            Auxiliar.Atributos atributos = Auxiliar.RetornoAtributos<T>((T)Activator.CreateInstance(typeof(T)));

            using (SqlConnection Conn = new SqlConnection(Conexao.strConn))
            {
                using (SqlCommand cmd = new SqlCommand(atributos.NomeProcedurePelaPK, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue(atributos.NomeChavePrimeria, Id);

                        Conn.Open();
                        using (SqlDataReader Dr = cmd.ExecuteReader())
                        {
                            if (Dr.Read())
                                _entidade = Auxiliar.RetornaDadosEntidade<T>(Dr);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return _entidade;
        }

        /// <summary>
        /// Perquisa o Registro pela Chave Primaria da Tabela
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor da Chave Primaria</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual T SelectPelaPK(long Id)
        {
            T _entidade = (T)Activator.CreateInstance(typeof(T));

            Auxiliar.Atributos atributos = Auxiliar.RetornoAtributos<T>((T)Activator.CreateInstance(typeof(T)));

            using (SqlConnection Conn = new SqlConnection(Conexao.strConn))
            {
                using (SqlCommand cmd = new SqlCommand(atributos.NomeProcedurePelaPK, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue(atributos.NomeChavePrimeria, Id);

                        Conn.Open();
                        using (SqlDataReader Dr = cmd.ExecuteReader())
                        {
                            if (Dr.Read())
                                _entidade = Auxiliar.RetornaDadosEntidade<T>(Dr);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return _entidade;
        }

        /// <summary>
        /// Entidade a ser Removida do Banco de Dados
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser Removido</param>
        public virtual void Remover(T Entidade)
        {
            Auxiliar.Atributos atributos = Auxiliar.RetornoAtributos<T>(Entidade);

            using (SqlConnection Conn = new SqlConnection(Conexao.strConn))
            {
                using (SqlCommand cmd = new SqlCommand(atributos.NomeProcedureRemover, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue(atributos.NomeChavePrimeria, (int)Entidade.GetType().GetProperty(atributos.NomeChavePrimeria).GetValue(Entidade, null));
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Metodo para inserir/alterar registro no Banco de Dados
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser inserido</param>
        //public virtual void Cadastro(T Entidade)
        //{
        //    Auxiliar.Atributos atributos = Auxiliar.RetornoAtributos<T>(Entidade);

        //    if (int.Parse(Entidade.GetType().GetProperty(atributos.NomeChavePrimeria).GetValue(Entidade, null).ToString()) == -1)
        //        Inserir(Entidade, atributos.NomeChavePrimeria, atributos.NomeProcedureInserir);
        //    else
        //        Alterar(Entidade, atributos.NomeProcedureAlterar);
        //}
        public int Cadastro(T Entidade)
        {
            var atributos = Auxiliar.RetornoAtributos<T>(Entidade);
            if (Convert.ToInt32(Entidade.GetType().GetProperty(atributos.NomeChavePrimeria).GetValue(Entidade, null)) == -1)
                return Inserir(ref Entidade, atributos.NomeChavePrimeria, atributos.NomeProcedureInserir);
            else
            {
                Alterar(Entidade, atributos.NomeProcedureAlterar);
                return 0;
            }
        }

        /// <summary>
        /// Insere registro Generico no Banco de dados
        /// 
        /// Autor: Ali Ahmad Hassan
        /// Data: 2013-04-09
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser inserido</param>
        /// <param name="CampoChave">Noma do Campo da Chave Primaria</param>
        /// <param name="NomeProcedure">Nome da Procedure para Inserir</param>
        //private void Inserir(T Entidade, string ChavePrimaria, string NomeProcedure)
        //{
        //    using (SqlConnection Conn = new SqlConnection(Conexao.strConn))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(NomeProcedure, Conn))
        //        {
        //            try
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //                foreach (SqlParameter Param in Auxiliar.GeraParametros<T>(Entidade))
        //                    cmd.Parameters.Add(Param);

        //                Conn.Open();
        //                Entidade.GetType().GetProperty(ChavePrimaria).SetValue(Entidade, int.Parse(cmd.ExecuteScalar().ToString()), null);
        //            }
        //            catch
        //            {
        //                throw ;
        //            }
        //        }
        //    }
        //}
        private int Inserir(ref T Entidade, string ChavePrimaria, string NomeProcedure)
        {
            object result = 0;
            using (var Conn = new SqlConnection(Conexao.strConn))
            {
                using (var cmd = new SqlCommand(NomeProcedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var parametros = Auxiliar.GeraParametros<T>(Entidade);

                        foreach (SqlParameter Param in parametros)
                            if (Param.ParameterName != "@Id")
                                cmd.Parameters.Add(Param);

                        Conn.Open();
                        result = cmd.ExecuteScalar();
                        Entidade.GetType().GetProperty(ChavePrimaria).SetValue(Entidade, Convert.ToInt32(result), null);
                    }
                    catch { throw; }
                }
            }
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Altera registro Generico no Banco de dados
        /// 
        /// Autor: Ali Ahmad Hassan
        /// Data: 2013-04-09
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser inserido</param>
        /// <param name="NomeProcedure">Nome da Procedure para Alterar</param>
        private void Alterar(T Entidade, string NomeProcedure)
        {
            using (SqlConnection Conn = new SqlConnection(Conexao.strConn))
            {
                using (SqlCommand cmd = new SqlCommand(NomeProcedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        foreach (SqlParameter Param in Auxiliar.GeraParametros<T>(Entidade))
                            cmd.Parameters.Add(Param);

                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
    }
}
