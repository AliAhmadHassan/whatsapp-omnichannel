using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class Chavewhatsapp:Base
    {
        public Chavewhatsapp()
        {
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUChavewhatsapp"
            , ProcedureInserir = "SPIChavewhatsapp"
            , ProcedureRemover = "SPDChavewhatsapp"
            , ProcedureListarTodos = "SPSChavewhatsapp"
            , ProcedureSelecionar = "SPSChavewhatsappByTelefoneOrcozol")]
		public string TelefoneOrcozol { get; set; }
		public string ChaveWhatsApp { get; set; }
		public string MaquinaVirtual { get; set; }
		public string IpMaquinaVirtual { get; set; }
		public string Carteira { get; set; }
		public Int16 StatusRoboEnvio { get; set; }
		public Int16 StatusRoboLeitura { get; set; }
		public DateTime DataHoraStatusEnvio { get; set; }
		public DateTime DataHoraStatusLeitura { get; set; }
    }
}
