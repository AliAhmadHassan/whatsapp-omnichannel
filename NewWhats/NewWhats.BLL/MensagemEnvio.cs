using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class MensagemEnvio:IMensagemEnvio
    {
        public List<DTO.MensagemEnvio> Select()
        {
            return new DAL.MensagemEnvio().Select();
        }

        public DTO.MensagemEnvio SelectById(int Id)
        {
            return new DAL.MensagemEnvio().SelectById(Id);
        }

        public List<DTO.MensagemEnvio> SelectByIdTelefoneOrcozol(string TelefoneOrcozol)
        {
            return new DAL.MensagemEnvio().SelectByIdTelefoneOrcozol(TelefoneOrcozol);
        }

        public void Remover(DTO.MensagemEnvio Entidade)
        {
            new DAL.MensagemEnvio().Remover(Entidade);
        }

        public void Cadastro(DTO.MensagemEnvio Entidade)
        {
            new DAL.MensagemEnvio().Cadastro(Entidade);
        }
    }
}
