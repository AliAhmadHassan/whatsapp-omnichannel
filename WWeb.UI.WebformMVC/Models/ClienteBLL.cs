using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WWeb.UI.WebformMVC.Models.DAL;
using WWeb.UI.WebformMVC.Models.DTO;

namespace WWeb.UI.WebformMVC.Models.BLL
{
    public class ClienteBLL
    {
        public void Cadastrar(ClienteDTO cliente)
        {
            new DAL.ClienteDAL().Cadastrar(cliente);
        }

        public ClienteDTO ObterPeloTelefone(string telefone)
        {
            return new DAL.ClienteDAL().ObterPeloTelefone(telefone);
        }

        public List<ClienteDTO> Listar()
        {
            return new DAL.ClienteDAL().Listar();
        }

        public List<ClienteStatusDTO> ListarComStatus()
        {
            return new DAL.ClienteDAL().ListarComStatus();
        }

        public List<ClienteDTO> ListarPeloTelefoneOrcozol(string telefone_orcozol)
        {
            return new DAL.ClienteDAL().ListarPeloTelefoneOrcozol(telefone_orcozol);
        }

        public List<ClienteDTO> ListarClientesOnlines(int qtd_clientes)
        {
            return new WWeb.UI.WebformMVC.Models.DAL.ClienteDAL().ListarClientesOnlines(qtd_clientes);
        }

        public List<ClienteDTO> ListarClientesOnlinesPeloUsuario(int usuario, string telefone_cliente)
        {
            return new WWeb.UI.WebformMVC.Models.DAL.ClienteDAL().ListarClientesOnlinesPeloUsuario(usuario, telefone_cliente);
        }

        public int QtdClienteXTelefoneOrcozol(string telefone_orcozol)
        {
            return new WWeb.UI.WebformMVC.Models.DAL.ClienteDAL().QtdClienteXTelefoneOrcozol(telefone_orcozol);
        }

        public void AtualizarVisualizacao(string telefone_cliente)
        {
            new WWeb.UI.WebformMVC.Models.DAL.ClienteDAL().AtualizarVisualizacao(telefone_cliente);
        }

        public List<ClienteHoraHoraDTO> ListarClientesHoraHora(DateTime inicio, DateTime fim)
        {
            return new ClienteDAL().ListarClientesHoraHora(inicio, fim);
        }
    }
}
