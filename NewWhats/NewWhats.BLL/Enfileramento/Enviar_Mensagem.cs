using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL.Enfileramento
{
    public static class Enviar_Mensagem
    {
        public static void enfilera(DTO.MensagemEnvio mensagem, string telefone_orcozol)
        {
            DAL.Enfileramento.Enviar_Mensagem.enfilera(mensagem, telefone_orcozol);
        }

        public static List<DTO.MensagemEnvio> recolhe(string telefone_orcozol)
        {
            return DAL.Enfileramento.Enviar_Mensagem.recolhe(telefone_orcozol);
        }
    }
}
