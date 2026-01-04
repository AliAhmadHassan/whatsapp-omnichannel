using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class Telefones:ITelefones
    {
        public List<DTO.Telefones> Select()
        {
            return new DAL.Telefones().Select();
        }

        public DTO.Telefones SelectById(int Id)
        {
            return new DAL.Telefones().SelectById(Id);
        }

        public void Remover(DTO.Telefones Entidade)
        {
            new DAL.Telefones().Remover(Entidade);
        }

        public void Cadastro(DTO.Telefones Entidade)
        {
            new DAL.Telefones().Cadastro(Entidade);
        }

        public void CadastrarTelefone(DTO.Telefones Entidade)
        {
            new DAL.Telefones().CadastrarTelefone(Entidade);
        }

        public List<DTO.Telefones> SelectByTelefoneCliente(string TelefoneCliente)
        {
            return new DAL.Telefones().SelectByTelefoneCliente(TelefoneCliente);
        }
    }
}
