using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.TrataEnfileiramento
{
    
    public class EnviaMensagemFila
    {
        BLL.MensagemEnvio mensagemEnvioService = null;

        public EnviaMensagemFila()
        {
            mensagemEnvioService = new BLL.MensagemEnvio();
        }

        public void Enfileira(string telefone_orcozol)
        {
            foreach (var item in mensagemEnvioService.SelectByIdTelefoneOrcozol(telefone_orcozol))
            {
                BLL.Enfileramento.Enviar_Mensagem.enfilera(item, telefone_orcozol);

                //item.Enviado = true;
                //mensagemEnvioService.Cadastro(item);

                //BLL.Enfileramento.Enviar_Mensagem.enfilera(item, telefone_orcozol);
            }
        }

        public void Enfileira(string telefone_orcozol,  MessageQueueTransaction MessageQueueLeituraTransacao)
        {
            foreach (var item in mensagemEnvioService.SelectByIdTelefoneOrcozol(telefone_orcozol))
            {
                BLL.Enfileramento.Enviar_Mensagem.enfilera(item, telefone_orcozol);

                //item.Enviado = true;
                //mensagemEnvioService.Cadastro(item);

                //BLL.Enfileramento.Enviar_Mensagem.enfilera(item, telefone_orcozol);
            }
        }
    }
}
