using WWeb.UI.WebformMVC.Models.DTO;

namespace WWeb.UI.WebformMVC.Models.BLL
{
    public class TelefonesBLL
    {
        public void Cadastrar(TelefonesDTO telefone)
        {
            new DAL.TelefonesDAL().Cadastrar(telefone);
        }

        public TelefonesDTO OnterTelefoneOrcozol(string telefone_orcozol)
        {
            return new DAL.TelefonesDAL().OnterTelefoneOrcozol(telefone_orcozol);
        }

        public TelefonesDTO OnterTelefoneCliente(string telefone_cliente)
        {
            return new DAL.TelefonesDAL().OnterTelefoneCliente(telefone_cliente);
        }
    }
}
