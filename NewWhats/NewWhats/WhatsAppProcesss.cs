using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewWhats.DTO;
using System.Configuration;
using System.Messaging;

namespace NewWhats
{
    public class WhatsAppProcesss
    {
        ChromeDriver driver = null;
        Form1 form;
        string telefone_orcozol = ConfigurationManager.AppSettings["application.telefoneorcozol"];

        public WhatsAppProcesss(ChromeDriver driver, Form1 form)
        {
            this.driver = driver;
            this.form = form;
        }

        public WhatsAppProcesss(ChromeDriver driver)
        {
            this.driver = driver;
            this.form = null;
        }

        public void IniciaProcesso()
        {
            LeituraClientesAntigos();

            while (true)
            {
                try
                {
                    EnfileraAsMensagem(LerMensagensNovas());
                    EnviaMensagens(telefone_orcozol);

                    if (this.form != null)
                        form.doEvents();

                    System.Threading.Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("\n\nFalha na aplicação. A mensagem do sistema foi: {0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        public void LeituraClientesAntigos()
        {
            var primeiro_contatos = driver.FindElementsByClassName("chat-body").OrderBy(c => c.Location.Y).ToList().FirstOrDefault();
            var tamanho_primeiro_contatos = primeiro_contatos.Size.Height;

            string pattern = "\\+(\\d{2})\\s(\\d{2}|\\d{3})\\s(\\d{4}|\\d{5})\\-(\\d{4})";
            Regex reg = new Regex(pattern);
            var matches = reg.Match(primeiro_contatos.Text);
            string auxTel = matches.Value.Replace("+", "").Replace(" ", "").Replace("-", "");
            if (string.IsNullOrEmpty(auxTel))
                auxTel = primeiro_contatos.Text.Split('\n')[0];

            var telefone_primeiro_contrato = primeiro_contatos.Text;

            while (driver.Title != "WhatsApp")
            {
                var msg = driver.FindElementsByClassName("unread").FirstOrDefault();
                reg = new Regex(pattern);

                do
                {
                    if (msg == null)
                        break;

                    matches = reg.Match(msg.Text);
                    auxTel = matches.Value.Replace("+", "").Replace(" ", "").Replace("-", "");

                    if (string.IsNullOrEmpty(auxTel))
                        auxTel = msg.Text.Split('\n')[0];

                    if (!string.IsNullOrEmpty(auxTel))
                        LeituraMensagensAntigas(auxTel);

                    msg = driver.FindElementsByClassName("unread").FirstOrDefault();
                } while (msg != null);

                System.Threading.Thread.Sleep(250);
                if (driver.FindElementById("pane-side") != null)
                    driver.ExecuteScript("document.getElementById('pane-side').scrollTop+=" + tamanho_primeiro_contatos);
            }

            driver.ExecuteScript("document.getElementById('pane-side').scrollTop=0");
        }

        private void LeituraMensagensAntigas(string clientes)
        {
            encontraCliente(clientes);
            List<DTO.Mensagem> mensagens = PegaNovasMensagens();
            EnfileraAsMensagem(mensagens);
        }

        private void EnfileraAsMensagem(List<Mensagem> mensagens)
        {
            if (mensagens != null && mensagens.Count > 0)
            {
                mensagens = TrataRepetidos(mensagens);
                foreach (var item in mensagens)
                {
                    MessageQueueTransaction trans = new MessageQueueTransaction();
                    trans.Begin();

                    try
                    {
                        System.Diagnostics.Debug.WriteLine(item.ToString());
                        BLL.Enfileramento.Gravar_Mensagem_BD.EnfileraMensagem(item, telefone_orcozol, trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine("Falha ao enfileirar mensagem. O erro do sistema foi: " + ex.Message + Environment.NewLine + ex.StackTrace);
                        trans.Abort();
                    }
                }
            }
        }

        private void EnviaMensagens(string telefone_orcozol)
        {
            List<DTO.MensagemEnvio> mensagensParaEnvio = BLL.Enfileramento.Enviar_Mensagem.recolhe(telefone_orcozol);

            if (mensagensParaEnvio != null)
            {
                foreach (var mensagem in mensagensParaEnvio)
                {
                    encontraCliente(mensagem.TelefoneCliente);

                    var txt_envio_msg = driver.FindElementsByClassName("input").Where(c => c.TagName.Equals("div")).FirstOrDefault();
                    if (txt_envio_msg != null)
                    {
                        txt_envio_msg.Clear();
                        txt_envio_msg.SendKeys(mensagem.Mensagem);
                    }

                    var btn = driver.FindElementsByClassName("send-container").FirstOrDefault();
                    if(btn == null)
                        btn = driver.FindElementsByClassName("icon-send").FirstOrDefault();

                    if (btn != null)
                        btn.Click();

                    List<DTO.Mensagem> mensagens = PegaNovasMensagens();
                    EnfileraAsMensagem(mensagens);
                }
            }
        }

        private void encontraCliente(string telefoneCliente)
        {
            var procurarContato = driver.FindElementsByClassName("input-search").FirstOrDefault();
            procurarContato.Clear();
            procurarContato.SendKeys(telefoneCliente);
            procurarContato.SendKeys("\n");

            DateTime timeout = DateTime.Now.AddSeconds(10);
            while (true)
            {
                if (DateTime.Now > timeout)
                    break;

                string content = driver.PageSource;

                if ((content.Contains(string.Format("false_{0}@", telefoneCliente)) && content.Contains("input-container")) || (content.Contains(string.Format("false_55{0}@", telefoneCliente)) && content.Contains("input-container")))
                {
                    break;
                }

                System.Threading.Thread.Sleep(500);
            }
        }

        private List<Mensagem> TrataRepetidos(List<Mensagem> mensagens)
        {
            List<Mensagem> retorno = new List<Mensagem>();

            try
            {
                foreach (var item in mensagens)
                {
                    if (string.IsNullOrEmpty(item.Msg) || string.IsNullOrEmpty(item.TelefoneCliente))
                    {
                        Console.Out.WriteLine("[{0}] - [{1}] - Não foi possível tratar a mensagem por falta de dados obrigatórios.", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), "TratarRepetidos");
                        continue;
                    }

                    if (retorno.Where(c => c.TelefoneCliente.Equals(item.TelefoneCliente)
                     && c.DataMensagem.Equals(item.DataMensagem)
                     && c.Msg.Equals(item.Msg)).Count() == 0)
                        retorno.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("[{1}] - Falha ao tentar tratar mensagens repetidas. A mensagem do sistema foi: {0}", ex.Message + Environment.NewLine + ex.StackTrace, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"));
            }
            return retorno;
        }

        static int contador_leitura_contatos = 0;
        static DateTime timeout_leitura_contatos = new DateTime();
        static bool alternador_leitura = true;
        /// <summary>
        /// Verifica se existe mensagens novas para ler e clica.
        /// </summary>
        private List<DTO.Mensagem> LerMensagensNovas()
        {
            var novasMensagens = driver.FindElementsByClassName("unread");
            if (novasMensagens.Count == 0)
            {
                //var contatos = driver.FindElementsByClassName("chat-secondary").OrderBy(c => c.Location.Y).ToList();
                var contatos = driver.FindElementsByClassName("timestamp").OrderBy(c => c.Location.Y).ToList();
                if (timeout_leitura_contatos < DateTime.Now)
                {
                    try
                    {
                        contatos[Convert.ToInt32(alternador_leitura)].Click();
                        timeout_leitura_contatos = DateTime.Now.AddSeconds(30);
                        alternador_leitura = !alternador_leitura;
                    }
                    catch
                    {
                        Console.Out.WriteLine("[{0}] - O Controle de timeout de contatos apresentou um erro.", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"));
                    }
                }

                return null;
            }

            List<DTO.Mensagem> mensagens = new List<Mensagem>();
            foreach (var novaMensagem in novasMensagens)
            {
                //Pega o telefone do usuario, para que com esse ver se a mensagem carregou
                try
                {
                    string pattern = "\\+(\\d{2})\\s(\\d{2}|\\d{3})\\s(\\d{4}|\\d{5})\\-(\\d{4})";
                    Regex reg = new Regex(pattern);
                    var matches = reg.Match(novaMensagem.Text);
                    string auxTel = matches.Value.Replace("+", "").Replace(" ", "").Replace("-", "");
                    //auxTel = "+55 11 97144-6308".Replace("+", "").Replace(" ", "").Replace("-", "");

                    if (string.IsNullOrEmpty(auxTel))
                        continue;

                    mensagens.AddRange(PegaNovasMensagens());

                    //Clica no usuario para ler as mensagens novas
                    int count_tentativa = 0;
                    System.Threading.Thread.Sleep(250);
                    do
                    {
                        try
                        {
                            novaMensagem.Click();
                            break;
                        }
                        catch
                        {
                            count_tentativa++;
                            System.Threading.Thread.Sleep(250);
                            if (count_tentativa >= 3)
                                break;
                        }
                    } while (count_tentativa < 3);

                    if (count_tentativa >= 3)
                        continue;

                    DateTime timeout = DateTime.Now.AddSeconds(10);
                    bool carregado = false;
                    while (true)
                    {
                        string content = driver.PageSource;

                        if (content.Contains(string.Format("false_{0}@", auxTel)) && content.Contains("input-container"))
                        {
                            carregado = true;
                            break;
                        }

                        System.Threading.Thread.Sleep(500);
                        if (DateTime.Now > timeout)
                        {
                            carregado = false;
                            break;
                        }
                    }

                    if (!carregado)
                        continue;

                    mensagens.AddRange(PegaNovasMensagens());
                    //break;
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("Falha na aplicação. A mensagem do sistema foi: {0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return mensagens;
        }

        /// <summary>
        /// Le o HTML atraves das Classes bubble bubble-text e retorna a lista de Mensagens
        /// </summary>
        /// <returns>Lista de Mensagens</returns>
        private List<Mensagem> PegaNovasMensagens()
        {
            List<DTO.Mensagem> mensagens = new List<Mensagem>();
            //string pattern = "((<div class=\"bubble bubble-text\"(.*?)?>)(.*?)(</div>))";
            //string pattern = "<div class=\"bubble bubble-text\"(.*?)?>([^\"]*)</span></div>";
            //var x = System.Text.RegularExpressions.Regex.Matches(driver.PageSource, pattern);

            var testetemp = driver.FindElementsByClassName("bubble-text");

            DTO.Mensagem mensagem = null;

            foreach (var bubble in testetemp)
            {
                mensagem = BLL.Bubble.getMessage(bubble.GetAttribute("innerHTML"));
                string[] campos = bubble.Text.Split('\n');
                if (campos.Length > 1)
                    mensagem.Msg = campos[0].Replace("\r", string.Empty);
                else
                    mensagem.Msg = "[imagem]";

                mensagens.Add(mensagem);
            }

            return mensagens;
        }
    }
}
