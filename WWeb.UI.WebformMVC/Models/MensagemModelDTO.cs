using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WWeb.UI.WebformMVC.Models.DTO
{
    [Serializable()]
    public class MensagemModelDTO : DTO.MensagemDTO
    {
        public string NomeCliente { get; set; }
    }
}
