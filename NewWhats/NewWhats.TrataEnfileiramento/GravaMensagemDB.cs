using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace NewWhats.TrataEnfileiramento
{
    public class GravaMensagemDB
    {
        BLL.Mensagem mensagemService = null;

        public GravaMensagemDB()
        {
            mensagemService = new BLL.Mensagem();
        }

        public void GravaMensagens(string telefone_orcozol)
        {
            var mensagens = BLL.Enfileramento.Gravar_Mensagem_BD.recolhe(telefone_orcozol);

            if (mensagens != null)
            {
                var serv_cliente = new BLL.Cliente();
                var serv_telefone = new BLL.Telefones();

                foreach (var mensagem in mensagens)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(mensagem.TelefoneCliente))
                            continue;

                        if (mensagem.TelefoneCliente.Length >= 12)
                            mensagem.TelefoneCliente = mensagem.TelefoneCliente.Remove(0, 2);

                        if (serv_cliente.SelectPeloTelefone(mensagem.TelefoneCliente).TelefoneCliente == null)
                        {
                            serv_cliente.CadastrarCliente(new DTO.Cliente()
                            {
                                TelefoneCliente = mensagem.TelefoneCliente,
                                Situacao = "offline",
                                DataHoraRegistro = DateTime.Now,
                                Cpf = "",
                                NomeCliente = "",
                                Status = 0,
                                Pendente = true
                            });

                            serv_telefone.CadastrarTelefone(new DTO.Telefones()
                            {
                                TelefoneCliente = mensagem.TelefoneCliente,
                                TelefoneOrcozol = ConfigurationManager.AppSettings["application.telefoneorcozol"]
                            });
                        }

                        mensagemService.Cadastro(mensagem);
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine("Falha na aplicação. Não foi possível processar as mensagens. O erro ocorrido foi:" + ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
            }
        }

        public void GravaMensagem(string telefone_orcozol)
        {
            MessageQueueTransaction trans = new MessageQueueTransaction();
            trans.Begin();

            var mensagem = BLL.Enfileramento.Gravar_Mensagem_BD.ProximaMensagem(telefone_orcozol, trans);
            do
            {
                if (mensagem == null)
                {
                    trans.Abort();
                    trans.Dispose();
                    return;
                }

                var serv_cliente = new BLL.Cliente();
                var serv_telefone = new BLL.Telefones();

                try
                {
                    if(string.IsNullOrEmpty(mensagem.TelefoneCliente))
                    {
                        trans.Commit();

                        trans = new MessageQueueTransaction();
                        trans.Begin();
                        mensagem = BLL.Enfileramento.Gravar_Mensagem_BD.ProximaMensagem(telefone_orcozol, trans);
                        continue;
                    }

                    if (mensagem.TelefoneCliente.Length >= 12)
                        mensagem.TelefoneCliente = mensagem.TelefoneCliente.Remove(0, 2);

                    if (serv_cliente.SelectPeloTelefone(mensagem.TelefoneCliente).TelefoneCliente == null)
                    {
                        serv_cliente.CadastrarCliente(new DTO.Cliente()
                        {
                            TelefoneCliente = mensagem.TelefoneCliente,
                            Situacao = "offline",
                            DataHoraRegistro = DateTime.Now,
                            Cpf = "",
                            NomeCliente = "",
                            Status = 0,
                            Pendente = true
                        });

                        serv_telefone.CadastrarTelefone(new DTO.Telefones()
                        {
                            TelefoneCliente = mensagem.TelefoneCliente,
                            TelefoneOrcozol = ConfigurationManager.AppSettings["application.telefoneorcozol"]
                        });
                    }

                    mensagemService.Cadastro(mensagem);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Abort();
                    Console.Out.WriteLine("Falha na aplicação. Não foi possível processar as mensagens. O erro ocorrido foi:" + ex.Message + Environment.NewLine + ex.StackTrace);
                }

                trans = new MessageQueueTransaction();
                trans.Begin();
                mensagem = BLL.Enfileramento.Gravar_Mensagem_BD.ProximaMensagem(telefone_orcozol, trans);
            } while (mensagem != null);

            #region MyRegion
            //if (mensagem != null)
            //{
            //    var serv_cliente = new BLL.Cliente();
            //    var serv_telefone = new BLL.Telefones();

            //    try
            //    {
            //        if (string.IsNullOrEmpty(mensagem.TelefoneCliente))
            //            return;

            //        if (mensagem.TelefoneCliente.Length >= 12)
            //            mensagem.TelefoneCliente = mensagem.TelefoneCliente.Remove(0, 2);

            //        if (serv_cliente.SelectPeloTelefone(mensagem.TelefoneCliente).TelefoneCliente == null)
            //        {
            //            serv_cliente.CadastrarCliente(new DTO.Cliente()
            //            {
            //                TelefoneCliente = mensagem.TelefoneCliente,
            //                Situacao = "offline",
            //                DataHoraRegistro = DateTime.Now,
            //                Cpf = "",
            //                NomeCliente = "",
            //                Status = 0,
            //                Pendente = true
            //            });

            //            serv_telefone.CadastrarTelefone(new DTO.Telefones()
            //            {
            //                TelefoneCliente = mensagem.TelefoneCliente,
            //                TelefoneOrcozol = ConfigurationManager.AppSettings["application.telefoneorcozol"]
            //            });
            //        }

            //        mensagemService.Cadastro(mensagem);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.Out.WriteLine("Falha na aplicação. Não foi possível processar as mensagens. O erro ocorrido foi:" + ex.Message + Environment.NewLine + ex.StackTrace);
            //    }

            //} 
            #endregion
        }

    }
}
