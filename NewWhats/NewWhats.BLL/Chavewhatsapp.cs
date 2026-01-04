using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class Chavewhatsapp:IChavewhatsapp
    {
        public List<DTO.Chavewhatsapp> Select()
        {
            return new DAL.Chavewhatsapp().Select();
        }

        public DTO.Chavewhatsapp SelectById(int Id)
        {
            return new DAL.Chavewhatsapp().SelectById(Id);
        }

        public void Remover(DTO.Chavewhatsapp Entidade)
        {
            new DAL.Chavewhatsapp().Remover(Entidade);
        }

        public void Cadastro(DTO.Chavewhatsapp Entidade)
        {
            new DAL.Chavewhatsapp().Cadastro(Entidade);
        }
    }
}
