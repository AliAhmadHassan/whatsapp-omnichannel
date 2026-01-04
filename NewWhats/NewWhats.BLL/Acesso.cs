using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class Acesso:IAcesso
    {
        public List<DTO.Acesso> Select()
        {
            return new DAL.Acesso().Select();
        }

        public DTO.Acesso SelectById(int Id)
        {
            return new DAL.Acesso().SelectById(Id);
        }

        public void Remover(DTO.Acesso Entidade)
        {
            new DAL.Acesso().Remover(Entidade);
        }

        public void Cadastro(DTO.Acesso Entidade)
        {
            new DAL.Acesso().Cadastro(Entidade);
        }
    }
}
