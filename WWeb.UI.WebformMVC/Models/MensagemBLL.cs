using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WWeb.UI.WebformMVC.Models.DTO;

namespace WWeb.UI.WebformMVC.Models.BLL
{
    public class MensagemBLL
    {
        public void Cadastrar(DTO.MensagemDTO mensagem)
        {
            new DAL.MensagemDAL().Cadastrar(mensagem);
        }

        public List<MensagemDTO> ObterMensagensCliente(string telefone_cliente)
        {
            return new DAL.MensagemDAL().ObterMensagensCliente(telefone_cliente);
        }

        public List<MensagemModelDTO> ObterMensagensNaoEnviadas(string telefone_orcozol)
        {
            return new DAL.MensagemDAL().ObterMensagensNaoEnviadas(telefone_orcozol);
        }

        public void AtualizarStatusEnviada(int id_mensagem)
        {
            new DAL.MensagemDAL().AtualizarStatusEnviada(id_mensagem);
        }

        public void AtualizarMensagemEnviada(string telefone_cliente)
        {
            new DAL.MensagemDAL().AtualizarMensagemEnviada(telefone_cliente);
        }

        public List<MensagemUsuarioDashboardDTO> UsuarioXMensagens(DateTime inicio, DateTime fim)
        {
            return new DAL.MensagemDAL().UsuarioXMensagens(inicio, fim);
        }

        public List<MensagemPeriodoDashboardDTO> MensagemXPeriodo(DateTime inicio, DateTime fim)
        {
            return new DAL.MensagemDAL().MensagemXPeriodo(inicio, fim);
        }

        public List<MensagemHoraHoraDTO> MensagemHoraHora(DateTime inicio, DateTime fim)
        {
            return new DAL.MensagemDAL().MensagemHoraHora(inicio, fim);
        }
    }
}
