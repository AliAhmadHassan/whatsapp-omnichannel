using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWeb.UI.WebformMVC.Models.DAL
{
    public static class Conexao
    {
        public static string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["wweb"].ConnectionString;

    }
}
