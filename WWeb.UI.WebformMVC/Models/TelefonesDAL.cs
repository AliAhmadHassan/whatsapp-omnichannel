using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using WWeb.UI.WebformMVC.Models.DTO;

namespace WWeb.UI.WebformMVC.Models.DAL
{
    public class TelefonesDAL : Base<TelefonesDTO>
    {
        public void Cadastrar(TelefonesDTO telefone)
        {
            using (SqlConnection ctx = new SqlConnection(Conexao.strConn))
            {
                string query = @"insert into telefones(TelefoneCliente,TelefoneOrcozol) values(@TelefoneCliente,@TelefoneOrcozol)";
                ctx.Open();
                using (SqlCommand cmd = new SqlCommand(query, ctx))
                {
                    cmd.Parameters.AddWithValue("@TelefoneCliente", telefone.TelefoneCliente);
                    cmd.Parameters.AddWithValue("@TelefoneOrcozol", telefone.TelefoneOrcozol);

                    var resultado = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                ctx.Close();
            }
        }

        public TelefonesDTO OnterTelefoneCliente(string telefone_cliente)
        {
            return AuxConsultas<TelefonesDTO>.Entidade("SPSTelefonePeloCliente", new SqlParameter("@TelefoneCliente", telefone_cliente));
        }

        public TelefonesDTO OnterTelefoneOrcozol(string telefone_orcozol)
        {
            return AuxConsultas<TelefonesDTO>.Entidade("SPSTelefonePeloNumeroOrcozol", new SqlParameter("@TelefoneCliente", telefone_orcozol));
        }
    }
}
