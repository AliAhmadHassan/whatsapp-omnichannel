using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WWeb.UI.WebformMVC.Models;
using WWeb.UI.WebformMVC.Models.DTO;

namespace WWeb.UI.WebformMVC.Controllers
{
    public class BatePapoController : Controller
    {
        public ActionResult IndexMensagem()
        {
            return View();
        }

        public ActionResult ClienteNegociacao()
        {
            return View();
        }

        public ActionResult Index(string telefone_cliente, string nome_cliente, string telefone_orcozol, string cpf_cliente)
        {
            try
            {
                #region Verificar Atividade

                Tb_Usuario usu = (Session["Usuario"] as Tb_Usuario);

                var cliente_global = WWebSessaoGlobal.ClientesEmNegociacao.Where(c => c.ClienteConversando.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();
                if (cliente_global != null)
                    if (!cliente_global.UsuarioConversando.Us_ID.Equals(usu.Us_ID))
                        return RedirectToAction("ClienteNegociacao");

                int qtd_clientes = Session["ClientesOnlineQtd"] == null ? 2000 : Convert.ToInt32(Session["ClientesOnlineQtd"].ToString());
                List<ClienteDTO> clientes_onlines = new Models.BLL.ClienteBLL().ListarClientesOnlines(qtd_clientes);

                var cli = clientes_onlines.Where(c => c.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();
                if (cli == null)
                    return RedirectToAction("ClienteNegociacao");

                #endregion

                ViewBag.TelefoneCliente = telefone_cliente;
                ViewBag.NomeCliente = nome_cliente;
                ViewBag.TelefoneOrcozol = telefone_orcozol;
                ViewBag.CpfCliente = cpf_cliente;

                string url_crm = string.Empty;
                int sessao_timeout = 1000 * 120;
                int max_conversa = 0;

                #region Configurações
                using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
                {
                    var config = ctx.Configuracao.Where(c => c.Id.Equals(5)).FirstOrDefault();
                    if (config != null)
                        url_crm = config.Valor;

                    var config_timeout = ctx.Configuracao.Where(c => c.Id.Equals(2)).FirstOrDefault();
                    if (config_timeout != null)
                        sessao_timeout = Convert.ToInt32(config_timeout.Valor) * 60;

                    var config_conversa = ctx.Configuracao.Where(c => c.Id.Equals(1)).FirstOrDefault();
                    if (config_conversa != null)
                        max_conversa = Convert.ToInt32(config_conversa.Valor);

                }
                #endregion

                if (!string.IsNullOrEmpty(url_crm))
                    ViewBag.Crm = url_crm;

                ViewBag.Timeout = sessao_timeout;
                ViewBag.MaxConversa = max_conversa;
                List<Cliente> clientes = new List<Cliente>();

                ClientesOnLines cliente = new ClientesOnLines()
                {
                    ClienteConversando = new Cliente() { Cpf = cpf_cliente, TelefoneCliente = telefone_cliente, NomeCliente = nome_cliente },
                    UsuarioConversando = usu
                };

                using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new WWebEntities())
                {
                    WWeb.UI.WebformMVC.Models.Cliente _cli = ctx.Cliente.Where(c => c.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();
                    if (_cli != null)
                    {
                        cliente.ClienteConversando.UrlImagem = _cli.UrlImagem;
                        cliente.ClienteConversando.DataHoraRegistro = _cli.DataHoraRegistro;
                        cliente.ClienteConversando.DataVisualizacao = _cli.DataVisualizacao;
                        if (_cli.Status != null)
                            ViewBag.StatusCliente = _cli.Status;
                        else
                            ViewBag.StatusCliente = 0;
                    }
                }

                if (cliente.ClienteConversando.UrlImagem == null)
                    ViewBag.FotoCliente = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/img/perfil.jpg"));
                else
                    ViewBag.FotoCliente = cliente.ClienteConversando.UrlImagem;

                int total_conversa = WWebSessaoGlobal.ClientesEmNegociacao.Where(u => u.UsuarioConversando.Us_ID.Equals(usu.Us_ID)).ToList().Count;
                if (total_conversa < max_conversa)
                {
                    if (WWebSessaoGlobal.ClientesEmNegociacao.Where(c => c.ClienteConversando.TelefoneCliente.Equals(cliente.ClienteConversando.TelefoneCliente)).FirstOrDefault() == null)
                        WWebSessaoGlobal.ClientesEmNegociacao.Add(cliente);

                    Mensagens(telefone_cliente);
                    return View();
                }
                else
                {
                    foreach (var c in WWebSessaoGlobal.ClientesEmNegociacao)
                        if (!c.UsuarioConversando.Us_ID.Equals(usu.Us_ID))
                            return RedirectToAction("IndexMensagem");

                    return View();
                }
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Conversa(string telefone_cliente)
        {
            using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new WWebEntities())
            {
                Cliente cliente = ctx.Cliente.Where(c => c.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();

                if (cliente.UrlImagem == null)
                    ViewBag.FotoCliente = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/img/perfil.jpg"));
                else
                    ViewBag.FotoCliente = cliente.UrlImagem;

                ViewBag.NomeCliente = cliente.NomeCliente;
                ViewBag.CpfCliente = cliente.Cpf;

                if (cliente.Status != 0)
                {
                    ViewBag.StatusClienteId = cliente.Status;
                    ViewBag.StatusCliente = cliente.Situacao1.Descricao;
                }
                else
                    ViewBag.StatusCliente = "Nao Definido";
            }

            ViewBag.TelefoneCliente = telefone_cliente;
            ViewBag.Telefone = telefone_cliente;

            return View(new Models.BLL.MensagemBLL().ObterMensagensCliente(telefone_cliente));
        }

        public JsonResult Mensagens(string telefone_cliente)
        {
            var mensagens = new Models.BLL.MensagemBLL().ObterMensagensCliente(telefone_cliente);
            new Models.BLL.MensagemBLL().AtualizarMensagemEnviada(telefone_cliente);

            System.Web.Script.Serialization.JavaScriptSerializer jSearializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            string jsonString = jSearializer.Serialize(mensagens);
            return Json(jsonString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PublicarMensagen(string telefone_cliente, string mensagem, string telefone_orcozol)
        {
            try
            {
                int tipo = mensagem.Equals("Conversa Finalizada!") ? 3 : 1;
                using (Models.WWebEntities ctx = new Models.WWebEntities())
                {
                    if (mensagem.Equals("Conversa Expirada!"))
                    {
                        var msg = ctx.Mensagem.Where(m => m.TelefoneCliente.Equals(telefone_cliente) && m.Tipo.Equals(2)).Take(1).LastOrDefault();
                        if (msg != null)
                            msg.Enviada = false;

                        tipo = 4;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        bool enviada = tipo == 1 ? false : true;
                        switch (tipo)
                        {
                            case 1:
                            case 2:
                                new WWeb.UI.WebformMVC.Models.BLL.MensagemEnvioBLL().Cadastro(new MensagemEnvioDTO()
                                {
                                    Data = DateTime.Now,
                                    Enviado = enviada,
                                    Mensagem = mensagem,
                                    TelefoneCliente = telefone_cliente,
                                    TelefoneOrcozol = telefone_orcozol,
                                    Usuario = (Session["Usuario"] as Tb_Usuario).Us_ID
                                });
                                break;
                            case 3:
                                ctx.Mensagem.Add(new Models.Mensagem()
                                {
                                    DataInclusao = DateTime.Now,
                                    TelefoneCliente = telefone_cliente,
                                    DataMensagem = DateTime.Now,
                                    Mensagem1 = mensagem,
                                    Tipo = Convert.ToInt16(tipo),
                                    Enviada = enviada,
                                    Usuario = (Session["Usuario"] as Tb_Usuario).Us_ID
                                });
                                ctx.SaveChanges();

                                break;
                            default:
                                break;
                        }
                    }

                    #region MyRegion
                    //ctx.Mensagem.Add(new Models.Mensagem()
                    //{
                    //    DataInclusao = DateTime.Now,
                    //    TelefoneCliente = telefone_cliente,
                    //    DataMensagem = DateTime.Now,
                    //    Mensagem1 = mensagem,
                    //    Tipo = Convert.ToInt16(tipo),
                    //    Enviada = enviada,
                    //    Usuario = (Session["Usuario"] as Tb_Usuario).Us_ID
                    //});

                    //
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(new object());
        }

        public JsonResult AtualizarNomeCliente(string telefone_cliente, string nome_cliente, string cpf_cliente)
        {
            try
            {
                using (Models.WWebEntities ctx = new Models.WWebEntities())
                {
                    Models.Cliente cli = ctx.Cliente.Where(c => c.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();
                    if (cli == null)
                        return Json("Cliente não encontrado.");

                    cli.NomeCliente = nome_cliente;
                    cli.Cpf = cpf_cliente;

                    ctx.SaveChanges();

                    List<Telefone> tels = new List<Telefone>();
                    Telefone tel = new Telefone()
                    {
                        DDDTelefone = telefone_cliente.Substring(0, 2),
                        Qualidade = "5",
                        NumeroTelefone = telefone_cliente.Substring(2, telefone_cliente.Length - 2),
                        TipoTelefone = "Celular",
                        TratadoPor = "Whatsapp Web"
                    };

                    tels.Add(tel);

                    new CobnetAdapter().AtualizarTelefone(new Devedor() { CPFCNPJ = cpf_cliente.Replace(".", "").Replace("-", "").Replace("/", "").Trim().PadLeft(15, '0'), Telefones = tels });
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json("Nome do cliente atualizado com sucesso.");
        }

        public JsonResult RemoverConversa(string telefone_cliente)
        {
            RespostaAutenticacao resposta = new RespostaAutenticacao();

            try
            {
                var cliente_global = WWebSessaoGlobal.ClientesEmNegociacao.Where(c => c.ClienteConversando.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();
                if (cliente_global != null)
                    WWebSessaoGlobal.ClientesEmNegociacao.Remove(cliente_global);

                resposta.responseAutenticado = "1";
                resposta.responseErro = "0";
                resposta.responseMsg = "Cliente Removido com sucesso.";
                resposta.responseUrl = "";
            }
            catch (Exception ex)
            {
                resposta.responseAutenticado = "1";
                resposta.responseErro = "1";
                resposta.responseMsg = ex.Message;
                resposta.responseUrl = "";
            }

            return Json(resposta);
        }

        public JsonResult FinalizarConversa(string telefone_cliente, string telefone_orcozol, string id_situacao)
        {
            RespostaAutenticacao resposta = new RespostaAutenticacao();
            try
            {
                using (Models.WWebEntities ctx = new WWebEntities())
                {
                    var cliente = ctx.Cliente.Where(c => c.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();
                    if (cliente != null)
                    {
                        cliente.Status = Convert.ToInt32(id_situacao);
                        cliente.Pendente = false;

                        ctx.SaveChanges();
                    }
                }

                PublicarMensagen(telefone_cliente, "Conversa Finalizada!", telefone_orcozol);

                resposta.responseAutenticado = "1";
                resposta.responseErro = "0";
                resposta.responseMsg = "Conversa finalizada com sucesso.";
                resposta.responseUrl = "";
            }
            catch (Exception ex)
            {
                resposta.responseAutenticado = "1";
                resposta.responseErro = "1";
                resposta.responseMsg = ex.Message;
                resposta.responseUrl = "";
            }
            return Json(resposta);

        }

        public JsonResult AtualizarStatus(string telefone_cliente, string id_situacao)
        {
            RespostaAutenticacao resposta = new RespostaAutenticacao();
            try
            {
                using (Models.WWebEntities ctx = new WWebEntities())
                {
                    var cliente = ctx.Cliente.Where(c => c.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();
                    if (cliente != null)
                        cliente.Status = Convert.ToInt32(id_situacao);

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                resposta.responseAutenticado = "1";
                resposta.responseErro = "1";
                resposta.responseMsg = ex.Message;
                resposta.responseUrl = "";
            }
            return Json(resposta);
        }
    }
}