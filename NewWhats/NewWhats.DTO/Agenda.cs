using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class Agenda:Base
    {
        public Agenda()
        {
            AgendaId = -1;
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUAgenda"
            , ProcedureInserir = "SPIAgenda"
            , ProcedureRemover = "SPDAgenda"
            , ProcedureListarTodos = "SPSAgenda"
            , ProcedureSelecionar = "SPSAgendaByAgendaId")]
		public int AgendaId { get; set; }
		public int UsuarioId { get; set; }
		public DateTime Data { get; set; }
		public string Anotacao { get; set; }
    }
}
