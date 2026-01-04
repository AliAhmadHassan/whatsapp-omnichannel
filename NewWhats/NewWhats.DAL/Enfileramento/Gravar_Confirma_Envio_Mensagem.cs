using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.DAL.Enfileramento
{
    public static class Gravar_Confirma_Envio_Mensagem
    {
        public static void enfilera(DTO.MensagemEnvio mensagem, string telefone_orcozol)
        {
            System.Messaging.Message msg = new System.Messaging.Message();
            msg.Body = mensagem;
            MessageQueue msgQ = new MessageQueue(string.Format(".\\private$\\{0}_NewWhats_Gravar_Confirma_Envio_Mensagem", telefone_orcozol));
            msgQ.Send(msg);
        }

        public static List<DTO.MensagemEnvio> recolhe(string telefone_orcozol)
        {
            List<DTO.MensagemEnvio> mensagens = null;
            using (MessageQueue messageQueue = new MessageQueue(string.Format(".\\private$\\{0}_NewWhats_Gravar_Confirma_Envio_Mensagem", telefone_orcozol)))
            {
                System.Messaging.Message[] messages = messageQueue.GetAllMessages();
                foreach (System.Messaging.Message message in messages)
                {
                    if (mensagens == null)
                        mensagens = new List<DTO.MensagemEnvio>();

                    DTO.MensagemEnvio mensagem = new DTO.MensagemEnvio();
                    Object o = new Object();
                    System.Type[] arrTypes = new System.Type[2];
                    arrTypes[0] = mensagem.GetType();
                    arrTypes[1] = o.GetType();
                    message.Formatter = new XmlMessageFormatter(arrTypes);
                    mensagem = ((DTO.MensagemEnvio)message.Body);

                    mensagens.Add(mensagem);
                    messageQueue.ReceiveById(message.Id);
                }
            }

            return mensagens;
        }
    }
}
