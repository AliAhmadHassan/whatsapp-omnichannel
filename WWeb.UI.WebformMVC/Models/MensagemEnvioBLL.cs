using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWeb.UI.WebformMVC.Models.BLL
{
    public class MensagemEnvioBLL
    {
        public List<DTO.MensagemEnvioDTO> Select()
        {
            return new DAL.MensagemEnvioDAL().Select();
        }

        public void Remover(DTO.MensagemEnvioDTO Entidade)
        {
            new DAL.MensagemEnvioDAL().Remover(Entidade);
        }

        public void Cadastro(DTO.MensagemEnvioDTO Entidade)
        {
            new DAL.MensagemEnvioDAL().Cadastro(Entidade);
        }
    }
}
