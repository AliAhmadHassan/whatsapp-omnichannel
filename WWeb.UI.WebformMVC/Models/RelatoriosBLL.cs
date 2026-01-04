using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WWeb.UI.WebformMVC.Models.DTO;
using WWeb.UI.WebformMVC.Models.DAL;

namespace WWeb.UI.WebformMVC.Models
{
    public class RelatoriosBLL
    {
        public List<RelatorioAcionamentoDTO> ListarComStatus(DateTime inicio, DateTime fim)
        {
            return new DAL.RelatoriosDAL().ListarComStatus(new DateTime(inicio.Year, inicio.Month, inicio.Day, 0, 0, 0), new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));
        }
    }
}