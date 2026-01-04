using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class MensagemPredefinida:IMensagemPredefinida
    {
        public List<DTO.MensagemPredefinida> Select()
        {
            return new DAL.MensagemPredefinida().Select();
        }

        public DTO.MensagemPredefinida SelectById(int Id)
        {
            return new DAL.MensagemPredefinida().SelectById(Id);
        }

        public void Remover(DTO.MensagemPredefinida Entidade)
        {
            new DAL.MensagemPredefinida().Remover(Entidade);
        }

        public void Cadastro(DTO.MensagemPredefinida Entidade)
        {
            new DAL.MensagemPredefinida().Cadastro(Entidade);
        }
    }
}
