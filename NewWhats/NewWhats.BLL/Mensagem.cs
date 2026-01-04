using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class Mensagem:IMensagem
    {
        public List<DTO.Mensagem> Select()
        {
            return new DAL.Mensagem().Select();
        }

        public DTO.Mensagem SelectById(int Id)
        {
            return new DAL.Mensagem().SelectById(Id);
        }

        public void Remover(DTO.Mensagem Entidade)
        {
            new DAL.Mensagem().Remover(Entidade);
        }

        public void Cadastro(DTO.Mensagem Entidade)
        {
            new DAL.Mensagem().Cadastro(Entidade);
        }

        public List<DTO.Mensagem> SelectByTelefoneCliente(string TelefoneCliente)
        {
            return new DAL.Mensagem().SelectByTelefoneCliente(TelefoneCliente);
        }
    }
}
