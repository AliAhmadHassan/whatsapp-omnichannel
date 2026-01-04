using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewWhats.DTO;
using System.Configuration;

namespace NewWhats.DAL
{
    public class Telefones : Base<DTO.Telefones>
    {
        public List<DTO.Telefones> SelectByTelefoneCliente(string TelefoneCliente)
        {
            return AuxConsultas<DTO.Telefones>.Lista("SPSTelefonesByTelefoneCliente", strConn(DTO.Base.TipoConexao.Core), new SqlParameter("@TelefoneCliente", TelefoneCliente));
        }

        public void CadastrarTelefone(DTO.Telefones entidade)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
            {
                string query = @"insert into Telefones(TelefoneCliente,TelefoneOrcozol) values(@TelefoneCliente,@TelefoneOrcozol)";
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("TelefoneCliente", entidade.TelefoneCliente);
                    cmd.Parameters.AddWithValue("TelefoneOrcozol", entidade.TelefoneOrcozol);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
