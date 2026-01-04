using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class Telefones:Base
    {
        public Telefones()
        {
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUTelefones"
            , ProcedureInserir = "SPITelefones"
            , ProcedureRemover = "SPDTelefones"
            , ProcedureListarTodos = "SPSTelefones"
            , ProcedureSelecionar = "SPSTelefonesByTelefoneCliente, TelefoneOrcozol")]
		public string TelefoneCliente { get; set; }
		public string TelefoneOrcozol { get; set; }
    }
}
