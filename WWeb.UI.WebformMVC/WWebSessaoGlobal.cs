using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WWeb.UI.WebformMVC
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 06/01/2016
    /// </summary>
    public static class WWebSessaoGlobal
    {
        public static object LockThread = new object();

        public static List<ClientesOnLines> ClientesEmNegociacao = new List<ClientesOnLines>();

        public static List<UsuarioOnLine> UsuariosOnLines = new List<UsuarioOnLine>();

        public static void RemoverCliente(ClientesOnLines cliente)
        {
            try
            {
                Monitor.Enter(LockThread);
                var cli = ClientesEmNegociacao.Where(c => c.ClienteConversando.TelefoneCliente.Equals(cliente.ClienteConversando.TelefoneCliente)).FirstOrDefault();
                if (cli != null)
                    ClientesEmNegociacao.Remove(cli);
            }
            catch { }
            finally
            {
                Monitor.Exit(LockThread);
            }
        }

        public static void LimparSessoes()
        {
            ClientesEmNegociacao.Clear();
        }
    }

    public class ClientesOnLines
    {
        public Models.Cliente ClienteConversando { get; set; }

        public Tb_Usuario UsuarioConversando { get; set; }
    }

    public class UsuarioOnLine
    {
        public string SessionID { get; set; }

        public Tb_Usuario Usuario { get; set; }
    }
}