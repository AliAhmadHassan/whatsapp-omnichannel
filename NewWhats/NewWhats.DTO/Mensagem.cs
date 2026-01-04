using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class Mensagem:Base
    {
        public Mensagem()
        {
            Id = -1;
        }

        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUMensagem"
            , ProcedureInserir = "SPIMensagem"
            , ProcedureRemover = "SPDMensagem"
            , ProcedureListarTodos = "SPSMensagem"
            , ProcedureSelecionar = "SPSMensagemById")]
		public int Id { get; set; }
		public string TelefoneCliente { get; set; }
		public DateTime DataMensagem { get; set; }
		public DateTime DataInclusao { get; set; }
		public Int16 Tipo { get; set; }
		public string Msg { get; set; }
		public bool Enviada { get; set; }
		public string Sequencial { get; set; }
		public int Usuario { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} => {3}", DataMensagem, TelefoneCliente, Tipo, Msg);
        }
    }
}
