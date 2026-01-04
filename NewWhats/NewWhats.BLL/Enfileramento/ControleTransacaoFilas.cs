using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;

namespace NewWhats.BLL.Enfileramento
{
    public class ControleTransacaoFilas
    {
        public static MessageQueueTransaction IniciarTransacao()
        {
            MessageQueueTransaction msg_trans = new MessageQueueTransaction();
            msg_trans.Begin();
            return msg_trans;
        }

        public static void ConfirmarTransacao(MessageQueueTransaction transacao)
        {
            transacao.Commit();
        }

        public static void CancelarTransacao(MessageQueueTransaction transacao)
        {
            transacao.Abort();
        }
    }
}
