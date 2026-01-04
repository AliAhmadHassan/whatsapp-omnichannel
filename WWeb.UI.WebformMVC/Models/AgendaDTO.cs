using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WWeb.UI.WebformMVC.Models.DTO
{
    public class AgendaDTO
    {
        [Display(Name = "Id")]
        public int AgendaId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Usuário")]
        [DataType(DataType.Text)]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Anotação")]
        [DataType(DataType.Text)]
        public string Anotacao { get; set; }
    }
}