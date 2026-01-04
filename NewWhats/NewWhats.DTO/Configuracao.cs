using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class Configuracao:Base
    {
        public Configuracao()
        {
            Id = -1;
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUConfiguracao"
            , ProcedureInserir = "SPIConfiguracao"
            , ProcedureRemover = "SPDConfiguracao"
            , ProcedureListarTodos = "SPSConfiguracao"
            , ProcedureSelecionar = "SPSConfiguracaoById")]
		public int Id { get; set; }
		public string Descricao { get; set; }
		public string Valor { get; set; }
		public bool Bloqueada { get; set; }
    }
}
