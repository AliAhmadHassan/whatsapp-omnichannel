using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WWeb.UI.WebformMVC.Models.DTO
{
    public class MensagemDTO
    {
        public int Id { get; set; }

        public string TelefoneCliente { get; set; }

        public DateTime DataMensagem { get; set; }

        public DateTime DataInclusao { get; set; }

        public short Tipo { get; set; }

        public string Mensagem { get; set; }

        public bool Enviada { get; set; }

        public string Sequencial { get; set; }

        public int Usuario { get; set; }
    }
}
