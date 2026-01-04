using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.DAL
{
    public class Mensagem:Base<DTO.Mensagem>
    {
        public List<DTO.Mensagem> SelectByTelefoneCliente(string TelefoneCliente)
        {
            return AuxConsultas<DTO.Mensagem>.Lista("SPSMensagemByTelefoneCliente", strConn(DTO.Base.TipoConexao.Core), new SqlParameter("@TelefoneCliente", TelefoneCliente));
        }
    }
}
