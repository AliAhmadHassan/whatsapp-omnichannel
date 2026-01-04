using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class Acesso:Base
    {
        public Acesso()
        {
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUAcesso"
            , ProcedureInserir = "SPIAcesso"
            , ProcedureRemover = "SPDAcesso"
            , ProcedureListarTodos = "SPSAcesso"
            , ProcedureSelecionar = "SPSAcessoByDepartamento, Menu")]
		public int Departamento { get; set; }
		public int Menu { get; set; }
    }
}
