using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace NewWhats.DAL
{
    public class Conexao
    {
        private string ConnectionStringCore = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString; // "Data Source=192.168.21.94;Initial Catalog=NewWhatsDB;Persist Security Info=True;User ID=sa;Password=Admin357/";

        protected string strConn(DTO.Base.TipoConexao tipoConexao)
        {
            string ConnectionString = string.Empty;

            switch (tipoConexao)
            {
                case DTO.Base.TipoConexao.Core:
                    ConnectionString = ConnectionStringCore;
                    break;
            }

            return ConnectionString;
        }

    }
}
