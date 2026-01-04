using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewWhats
{
    class Program
    {
        ///// <summary>
        ///// The main entry point for the application.
        ///// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}

        static System.Timers.Timer timer_checkout = new System.Timers.Timer(10000);
        static ChromeDriver driver = null;

        static void Main(string[] args)
        {
            Cabecalho();

            timer_checkout.Elapsed += Timer_checkout_Elapsed;
            timer_checkout.Start();

            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://web.whatsapp.com/");

            WhatsAppProcesss whatsAppProcess = new WhatsAppProcesss(driver);

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                if ((driver.FindElementsByClassName("intro-body").Count > 0) || (driver.FindElementsByClassName("pane-chat").Count > 0))
                    whatsAppProcess.IniciaProcesso();
            }
        }

        static void Cabecalho()
        {
            Console.Out.Write("\r");
            SaidaLogs("*".PadLeft(100, '*'));
            SaidaLogs(string.Format("Serviço de Leitura WHATS WEB iniciado com sucesso. Carteira: {0}, Telefone: {1}", ConfigurationManager.AppSettings["application.carteira"], ConfigurationManager.AppSettings["application.telefoneorcozol"]));
            SaidaLogs("*".PadLeft(100, '*'));
            Console.Out.Write("\r");
        }

        static int contador_limpeza = 1;

        static void Timer_checkout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (contador_limpeza > 5)
                {
                    contador_limpeza = 1;
                    Console.Clear();
                    Cabecalho();
                }
                else
                {
                    contador_limpeza++;
                }

                SaidaLogs("Checkout do sistema: Atividade normal.");

                if (driver.SessionId == null)
                {
                    SaidaLogs("Sessão finalizada.");
                    driver.Close();
                    driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                }
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
