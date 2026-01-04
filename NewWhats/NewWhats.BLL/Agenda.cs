using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class Agenda:IAgenda
    {
        public List<DTO.Agenda> Select()
        {
            return new DAL.Agenda().Select();
        }

        public DTO.Agenda SelectById(int Id)
        {
            return new DAL.Agenda().SelectById(Id);
        }

        public void Remover(DTO.Agenda Entidade)
        {
            new DAL.Agenda().Remover(Entidade);
        }

        public void Cadastro(DTO.Agenda Entidade)
        {
            new DAL.Agenda().Cadastro(Entidade);
        }
    }
}
