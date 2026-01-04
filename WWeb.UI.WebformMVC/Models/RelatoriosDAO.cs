using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WWeb.UI.WebformMVC.Models.DTO;
namespace WWeb.UI.WebformMVC.Models.DAL
{
    public class RelatoriosDAL : Base<RelatorioAcionamentoDTO>
    {
        public List<RelatorioAcionamentoDTO> ListarComStatus(DateTime inicio, DateTime fim)
        {
            return AuxConsultas<RelatorioAcionamentoDTO>.Lista("SPSClientePorData", new SqlParameter[] { new SqlParameter("@Inicio", inicio), new SqlParameter("@Fim", fim) });
        }
    }
}