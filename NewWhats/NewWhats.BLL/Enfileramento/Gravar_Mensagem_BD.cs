using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL.Enfileramento
{
    public static class Gravar_Mensagem_BD
    {
        public static void enfilera(DTO.Mensagem mensagem, string telefone_orcozol)
        {
            DAL.Enfileramento.Gravar_Mensagem_BD.enfilera(mensagem, telefone_orcozol);
        }

        public static List<DTO.Mensagem> recolhe(string telefone_orcozol)
        {
            return DAL.Enfileramento.Gravar_Mensagem_BD.recolhe(telefone_orcozol);
        }

        public static void EnfileraMensagem(DTO.Mensagem mensagem, string telefone_orcozol, MessageQueueTransaction MessageQueueLeituraTransacao)
        {
            DAL.Enfileramento.Gravar_Mensagem_BD.EnfileraMensagem(mensagem, telefone_orcozol, MessageQueueLeituraTransacao);
        }

        public static DTO.Mensagem ProximaMensagem(string telefone_orcozol, MessageQueueTransaction MessageQueueLeituraTransacao)
        {
            return DAL.Enfileramento.Gravar_Mensagem_BD.ProximaMensagem(telefone_orcozol, MessageQueueLeituraTransacao);
        }
    }
}
