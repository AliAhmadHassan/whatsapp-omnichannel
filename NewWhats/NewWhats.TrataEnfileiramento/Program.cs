using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWhats.TrataEnfileiramento
{
    class Program
    {
        static System.Timers.Timer timer_checkout = new System.Timers.Timer(10000);
        static int contador_limpeza_console = 1;

        static string telefone_orcozol = ConfigurationManager.AppSettings["application.telefoneorcozol"];

        static void Main(string[] args)
        {
            timer_checkout.Elapsed += Timer_checkout_Elapsed;
            timer_checkout.Start();

            CabecalhoPrograma();

            GravaMensagemDB gravaMensagemDB = new GravaMensagemDB();
            EnviaMensagemFila enviaMensagemFila = new EnviaMensagemFila();

            while (true)
            {
                try
                {
                    gravaMensagemDB.GravaMensagem(telefone_orcozol);
                    enviaMensagemFila.Enfileira(telefone_orcozol);

                    System.Threading.Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    SaidaLogs("Falha na aplicação. A mensagem do sistema foi: " + ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }
        
        static void Timer_checkout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (contador_limpeza_console >= 5)
                {
                    Console.Clear();
                   
                    CabecalhoPrograma();
                    contador_limpeza_console = 1;
                }
                else
                    contador_limpeza_console++;

                SaidaLogs("Checkout do sistema: Atividade normal.");
            }
            catch (Exception ex)
            {
                timer_checkout.Dispose();
                timer_checkout = null;

                timer_checkout.Elapsed += Timer_checkout_Elapsed;
                timer_checkout.Start();

                SaidaLogs("Falha na aplicação. A mensagem do sistema foi: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        static void SaidaLogs(string msg)
        {
            Console.Out.WriteLine("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), msg.ToUpper());
        }

        static void CabecalhoPrograma()
        {
            SaidaLogs("*".PadLeft(100, '*'));
            SaidaLogs(string.Format("Serviço Trata Enfileiramento iniciado com sucesso. Carteira: {0}, Telefone: {1}", ConfigurationManager.AppSettings["application.carteira"], ConfigurationManager.AppSettings["application.telefoneorcozol"]));
            SaidaLogs("*".PadLeft(100, '*'));
        }
    }
}