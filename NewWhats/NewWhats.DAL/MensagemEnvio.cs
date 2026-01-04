using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewWhats.DTO;

namespace NewWhats.DAL
{
    public class MensagemEnvio : Base<DTO.MensagemEnvio>
    {
        public List<DTO.MensagemEnvio> SelectByIdTelefoneOrcozol(string telefoneOrcozol)
        {
            return AuxConsultas<DTO.MensagemEnvio>.Lista("SPSMensagemEnvioByTelefoneOrcozol", strConn(DTO.Base.TipoConexao.Core), new SqlParameter("@TelefoneOrcozol", telefoneOrcozol));
        }
    }
}
