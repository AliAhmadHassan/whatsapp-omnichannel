using NewWhats.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.BLL
{
    public class Cliente : ICliente
    {
        public List<DTO.Cliente> Select()
        {
            return new DAL.Cliente().Select();
        }

        public DTO.Cliente SelectById(int Id)
        {
            return new DAL.Cliente().SelectById(Id);
        }

        public void Remover(DTO.Cliente Entidade)
        {
            new DAL.Cliente().Remover(Entidade);
        }

        public void Cadastro(DTO.Cliente Entidade)
        {
            new DAL.Cliente().Cadastro(Entidade);
        }

        public List<DTO.Cliente> SelectByStatus(int Status)
        {
            return new DAL.Cliente().SelectByStatus(Status);
        }

        public DTO.Cliente SelectPeloTelefone(string telefone)
        {
            return new DAL.Cliente().SelectPeloTelefone(telefone);
        }

        public void CadastrarCliente(DTO.Cliente cliente)
        {
            new DAL.Cliente().CadastrarCliente(cliente);
        }
    }
}
