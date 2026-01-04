using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL.Enfileramento
{
    public static class Gravar_Confirma_Envio_Mensagem
    {
        public static void enfilera(DTO.MensagemEnvio mensagem, string telefone_orcozl)
        {
            DAL.Enfileramento.Gravar_Confirma_Envio_Mensagem.enfilera(mensagem, telefone_orcozl);
        }

        public static List<DTO.MensagemEnvio> recolhe(string telefone_orcozl)
        {
            return DAL.Enfileramento.Gravar_Confirma_Envio_Mensagem.recolhe(telefone_orcozl);
        }
    }
}
