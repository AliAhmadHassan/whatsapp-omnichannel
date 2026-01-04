using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Configuration;

namespace NewWhats
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (ChromeDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://web.whatsapp.com/");

                WhatsAppProcesss whatsAppProcess = new WhatsAppProcesss(driver, this);

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                                       
                    //Se o usuario Logou no WhatsApp
                    if ((driver.FindElementsByClassName("intro-body").Count > 0) || (driver.FindElementsByClassName("pane-chat").Count > 0))
                    {
                        //Começa a Processar
                        whatsAppProcess.IniciaProcesso();
                    }
                    Application.DoEvents();
                }
            }
        }

        public void doEvents()
        {
            Application.DoEvents();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Add("RegexBubble", "(.*)data-id=\"(?<NosEnviamos>.*)_(?<TelCliente>\\d{13})\\@(.*)(?<hh>\\d{1}|\\d{2})\\:(?<mm>\\d{1}|\\d{2})\\, (?<dd>\\d{1}|\\d{2})[\\/](?<MM>\\d{1}|\\d{2})[\\/](?<yyyy>\\d{4})(.*)\\+(?<DDI>\\d{2})\\s(?<DDD>\\d{2})\\s(?<TEL1>\\d{5})\\-(?<TEL2>\\d{4})(.*)(<span(.*?)>)(?<Texto>.*)(\\</span>)");
            config.Save(ConfigurationSaveMode.Minimal);*/
        }
    }
}
