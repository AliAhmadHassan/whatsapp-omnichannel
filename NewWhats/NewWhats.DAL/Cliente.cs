using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.DAL
{
    public class Cliente : Base<DTO.Cliente>
    {
        public List<DTO.Cliente> SelectByStatus(int Status)
        {
            return AuxConsultas<DTO.Cliente>.Lista("SPSClienteByStatus", strConn(DTO.Base.TipoConexao.Core), new SqlParameter("@Status", Status));
        }

        public DTO.Cliente SelectPeloTelefone(string telefone)
        {
            return AuxConsultas<DTO.Cliente>.Entidade("SPSClientePeloTelefone", strConn(DTO.Base.TipoConexao.Core), new SqlParameter("@TelefoneCliente", telefone));
        }

        public void CadastrarCliente(DTO.Cliente cliente)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                string query = @"insert into cliente(TelefoneCliente,Situacao,DataHoraRegistro, Pendente) values(@TelefoneCliente,@Situacao, @DataHoraRegistro, @Pendente)";
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TelefoneCliente", cliente.TelefoneCliente);
                    cmd.Parameters.AddWithValue("@Situacao", cliente.Situacao);
                    cmd.Parameters.AddWithValue("@DataHoraRegistro", cliente.DataHoraRegistro);
                    cmd.Parameters.AddWithValue("@Pendente", cliente.Pendente);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}