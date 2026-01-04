using System;
using System.Collections.Generic;

namespace NewWhats.DTO
{
    public class Cliente:Base
    {
        public Cliente()
        {
        }
        [AtributoBind(ChavePrimaria = true
            , ProcedureAlterar = "SPUCliente"
            , ProcedureInserir = "SPICliente"
            , ProcedureRemover = "SPDCliente"
            , ProcedureListarTodos = "SPSCliente"
            , ProcedureSelecionar = "SPSClienteByTelefoneCliente")]
		public string TelefoneCliente { get; set; }
		public string Situacao { get; set; }
		public byte[] UrlImagem { get; set; }
		public DateTime DataHoraRegistro { get; set; }
		public string NomeCliente { get; set; }
		public DateTime DataVisualizacao { get; set; }
		public string Cpf { get; set; }
		public int Status { get; set; }
        public bool Pendente { get; set; }
    }
}
