using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class Configuracao:IConfiguracao
    {
        public List<DTO.Configuracao> Select()
        {
            return new DAL.Configuracao().Select();
        }

        public DTO.Configuracao SelectById(int Id)
        {
            return new DAL.Configuracao().SelectById(Id);
        }

        public void Remover(DTO.Configuracao Entidade)
        {
            new DAL.Configuracao().Remover(Entidade);
        }

        public void Cadastro(DTO.Configuracao Entidade)
        {
            new DAL.Configuracao().Cadastro(Entidade);
        }
    }
}
