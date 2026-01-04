using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class Situacao:Base
    {
        public Situacao()
        {
            Id = -1;
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUSituacao"
            , ProcedureInserir = "SPISituacao"
            , ProcedureRemover = "SPDSituacao"
            , ProcedureListarTodos = "SPSSituacao"
            , ProcedureSelecionar = "SPSSituacaoById")]
		public int Id { get; set; }
		public string Descricao { get; set; }
		public bool Bloqueado { get; set; }
    }
}
