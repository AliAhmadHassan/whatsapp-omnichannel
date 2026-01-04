using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class MensagemPredefinida:Base
    {
        public MensagemPredefinida()
        {
            Id = -1;
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUMensagemPredefinida"
            , ProcedureInserir = "SPIMensagemPredefinida"
            , ProcedureRemover = "SPDMensagemPredefinida"
            , ProcedureListarTodos = "SPSMensagemPredefinida"
            , ProcedureSelecionar = "SPSMensagemPredefinidaById")]
		public int Id { get; set; }
		public string Mensagem { get; set; }
		public bool Bloqueada { get; set; }
    }
}
