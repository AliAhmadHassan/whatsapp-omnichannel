using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class Situacao:ISituacao
    {
        public List<DTO.Situacao> Select()
        {
            return new DAL.Situacao().Select();
        }

        public DTO.Situacao SelectById(int Id)
        {
            return new DAL.Situacao().SelectById(Id);
        }

        public void Remover(DTO.Situacao Entidade)
        {
            new DAL.Situacao().Remover(Entidade);
        }

        public void Cadastro(DTO.Situacao Entidade)
        {
            new DAL.Situacao().Cadastro(Entidade);
        }
    }
}
