using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WWeb.UI.WebformMVC.Models
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 22/12/2015
    /// </summary>
    public class MensagemPredefinidaDTO : WWeb.UI.WebformMVC.Models.DAL.BaseDTO
    {
        public MensagemPredefinidaDTO()
        {
            Id = -1;
        }

        [AtributoBind(ChavePrimaria = true
       , ProcedureAlterar = "SPUmensagempredefinida"
       , ProcedureInserir = "SPImensagempredefinida"
       , ProcedureRemover = "SPDmensagempredefinida"
       , ProcedureListarTodos = "SPSmensagempredefinida"
       , ProcedureSelecionar = "SPSmensagempredefinidaPelaPK")]
        public int Id { get; set; }

        public string Mensagem { get; set; }

        public bool Bloqueada { get; set; }
    }
}