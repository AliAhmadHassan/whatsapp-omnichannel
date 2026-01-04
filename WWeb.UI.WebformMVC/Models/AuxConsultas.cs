using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWeb.UI.WebformMVC.Models.DAL
{
    public static  class AuxConsultas<T>
    {
        public static List<T> Lista(string NomeProcedure, params SqlParameter[] parametros)
        {
            List<T> LObjeto = new List<T>();

            using (SqlConnection Conn = new SqlConnection(Conexao.strConn))
            {
                using (SqlCommand cmd = new SqlCommand(NomeProcedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        foreach (SqlParameter parametro in parametros)
                            cmd.Parameters.Add(parametro);

                        Conn.Open();
                        using (SqlDataReader Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LObjeto.Add(Auxiliar.RetornaDadosEntidade<T>(Dr));
                    }
                    catch
                    {
                        throw ;
                    }
                }
            }
            return LObjeto;
        }

        public static T Entidade(string NomeProcedure, params SqlParameter[] parametros)
        {

            T Objeto = (T)Activator.CreateInstance(typeof(T));

            using (SqlConnection Conn = new SqlConnection(Conexao.strConn))
            {
                using (SqlCommand cmd = new SqlCommand(NomeProcedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        foreach (SqlParameter parametro in parametros)
                            cmd.Parameters.Add(parametro);

                        Conn.Open();
                        using (SqlDataReader Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                Objeto = Auxiliar.RetornaDadosEntidade<T>(Dr);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return Objeto;
        }
    }
}
