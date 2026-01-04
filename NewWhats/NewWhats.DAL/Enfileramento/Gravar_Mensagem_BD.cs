using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.DAL.Enfileramento
{
    public static class Gravar_Mensagem_BD
    {
        public static void enfilera(DTO.Mensagem mensagem, string telefone_orcozol)
        {
            System.Messaging.Message msg = new System.Messaging.Message();
            msg.Body = mensagem;
            MessageQueue msgQ = new MessageQueue(string.Format(".\\private$\\{0}_NewWhats_Gravar_Mensagem_BD", telefone_orcozol));
            msgQ.Send(msg);
        }

        public static void EnfileraMensagem(DTO.Mensagem mensagem, string telefone_orcozol, MessageQueueTransaction MessageQueueLeituraTransacao)
        {
            System.Messaging.Message msg = new System.Messaging.Message();
            msg.Body = mensagem;
            MessageQueue msgQ = new MessageQueue(string.Format(".\\private$\\{0}_NewWhats_Gravar_Mensagem_BD", telefone_orcozol));
            msgQ.Send(msg, MessageQueueLeituraTransacao);
        }

        public static List<DTO.Mensagem> recolhe(string telefone_orcozol)
        {
            List<DTO.Mensagem> mensagens = null;
            using (MessageQueue messageQueue = new MessageQueue(string.Format(".\\private$\\{0}_NewWhats_Gravar_Mensagem_BD", telefone_orcozol)))
            {
                System.Messaging.Message[] messages = messageQueue.GetAllMessages();
                foreach (System.Messaging.Message message in messages)
                {
                    if (mensagens == null)
                        mensagens = new List<DTO.Mensagem>();

                    DTO.Mensagem mensagem = new DTO.Mensagem();
                    Object o = new Object();
                    System.Type[] arrTypes = new System.Type[2];
                    arrTypes[0] = mensagem.GetType();
                    arrTypes[1] = o.GetType();
                    message.Formatter = new XmlMessageFormatter(arrTypes);
                    mensagem = ((DTO.Mensagem)message.Body);

                    mensagens.Add(mensagem);
                    messageQueue.ReceiveById(message.Id);
                }
            }
            return mensagens;
        }

        public static MessageQueue MessageQueueLeitura = null;
        public static DTO.Mensagem ProximaMensagem(string telefone_orcozol, MessageQueueTransaction MessageQueueLeituraTransacao)
        {
            if (MessageQueueLeitura == null)
                MessageQueueLeitura = new MessageQueue(string.Format(".\\private$\\{0}_NewWhats_Gravar_Mensagem_BD", telefone_orcozol));

            DTO.Mensagem mensagem = new DTO.Mensagem();
            try
            {
                var message = MessageQueueLeitura.Receive(new TimeSpan(0, 0, 1), MessageQueueLeituraTransacao);
                Object o = new Object();
                System.Type[] arrTypes = new System.Type[2];
                arrTypes[0] = mensagem.GetType();
                arrTypes[1] = o.GetType();
                message.Formatter = new XmlMessageFormatter(arrTypes);

                mensagem = ((DTO.Mensagem)message.Body);
            }
            catch (MessageQueueException msms_ex)
            {
                switch (msms_ex.ErrorCode)
                {
                    case -2147467259:
                        Console.Out.WriteLine("[{0}] - Verificação da fila: Nenhuma mensagem pendente.", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"));
                        return null;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Falha ao ler fila de mensagens. A mensagem de erro foi: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return mensagem;
        }
    }
}
