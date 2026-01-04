using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class MensagemEnvio:Base
    {
        public MensagemEnvio()
        {
            Id = -1;
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUMensagemEnvio"
            , ProcedureInserir = "SPIMensagemEnvio"
            , ProcedureRemover = "SPDMensagemEnvio"
            , ProcedureListarTodos = "SPSMensagemEnvio"
            , ProcedureSelecionar = "SPSMensagemEnvioById")]
		public int Id { get; set; }
		public DateTime Data { get; set; }
		public string Mensagem { get; set; }
		public int Usuario { get; set; }
		public bool Enviado { get; set; }
		public string TelefoneCliente { get; set; }
        public string TelefoneOrcozol { get; set; }
    }
}
