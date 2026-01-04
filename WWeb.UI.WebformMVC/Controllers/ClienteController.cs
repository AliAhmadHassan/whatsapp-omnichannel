using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WWeb.UI.WebformMVC.Models.DTO;

namespace WWeb.UI.WebformMVC.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Index(int pagina)
        {
            ViewBag.Pagina = pagina;
            int skip = pagina;
            int count = 30;

            if (pagina > 1)
                skip = ((pagina - 1) * count);
            else
                skip = 0;

            var result = new Models.BLL.ClienteBLL().ListarComStatus().Skip(skip).Take(count).ToList();

            return View(result);
        }

        public ActionResult Buscar(DateTime inicio, DateTime fim)
        {
            ViewBag.Pagina = 1;
            ViewBag.Inicio = Convert.ToDateTime(inicio).ToString("dd/MM/yyyy");
            ViewBag.Fim = Convert.ToDateTime(fim).ToString("dd/MM/yyyy");

            inicio = new DateTime(inicio.Year, inicio.Month, inicio.Day, 0, 0, 0);
            fim = new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59);

            var result = new Models.BLL.ClienteBLL().ListarComStatus().Where(c => c.DataHoraRegistro > inicio && c.DataHoraRegistro < fim).OrderBy(c => c.DataHoraRegistro).ToList();

            return View("Index", result);
        }

        public ActionResult RetornarClientesOnlines(string primeira_consulta)
        {
            try
            {
                ViewBag.PrimeiraConsulta = primeira_consulta;
                bool lista_igual = true;

                Tb_Usuario usuario = (Session["Usuario"] as Tb_Usuario);
                if (WWebSessaoGlobal.UsuariosOnLines.Where(u => u.Usuario.Us_ID.Equals(usuario.Us_ID)).FirstOrDefault() == null)
                    return View("RedirectLogin");

                #region Tamanho da fila de atendimento
                if (Session["ClientesOnlineQtd"] == null)
                {
                    using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
                    {
                        var tamanho_fila_atendimento = ctx.Configuracao.Where(c => c.Id.Equals(6)).FirstOrDefault();
                        Session["ClientesOnlineQtd"] = tamanho_fila_atendimento.Valor;
                    }
                }
                #endregion

                List<ClienteDTO> cli = new List<ClienteDTO>();

                if (Session["ClientesOnline"] == null)
                    Session["ClientesOnline"] = new List<ClienteDTO>() { new ClienteDTO() { TelefoneCliente = "" } };

                List<ClienteDTO> cli_em_atendimento = (Session["ClientesOnline"] as List<ClienteDTO>);
                if (cli_em_atendimento == null)
                    cli_em_atendimento = new List<ClienteDTO>();

                //Retornar todos os clientes disponíveis, quantidade informada
                var clientes_disponiveis = new Models.BLL.ClienteBLL().ListarClientesOnlines(Convert.ToInt32(Session["ClientesOnlineQtd"]));

                #region Verifica em clientes disponiveis quem está conversando e retorna cli

                foreach (var item in clientes_disponiveis)
                    if (WWebSessaoGlobal.ClientesEmNegociacao.Count(c => c.ClienteConversando.TelefoneCliente.Equals(item.TelefoneCliente)) == 0)
                        cli.Add(item);

                #endregion

                List<ClienteDTO> cli_por_usuarios = new List<ClienteDTO>();

                #region Veirifica na lista cli quais são os clientes do usuário

                foreach (var cli_usu in cli)
                    if (new Models.BLL.ClienteBLL().ListarClientesOnlinesPeloUsuario(usuario.Us_ID, cli_usu.TelefoneCliente).ToList().Count > 0)
                        cli_por_usuarios.Add(cli_usu);

                #endregion

                #region Estrutura da fila do operador
                if (cli_por_usuarios.Count > 0)
                {
                    #region Fila do Operador
                    foreach (ClienteDTO c in cli_por_usuarios)
                    {
                        if (WWebSessaoGlobal.ClientesEmNegociacao.Where(d => d.ClienteConversando.TelefoneCliente.Equals(c.TelefoneCliente)).FirstOrDefault() == null)
                        {
                            WWebSessaoGlobal.ClientesEmNegociacao.Add(new ClientesOnLines()
                            {
                                ClienteConversando = new Models.Cliente()
                                {
                                    Cpf = c.Cpf,
                                    TelefoneCliente = c.TelefoneCliente,
                                    NomeCliente = c.NomeCliente,
                                    DataHoraRegistro = c.DataHoraRegistro,
                                    DataVisualizacao = c.DataVisualizacao
                                },
                                UsuarioConversando = usuario
                            });
                        }
                    }
                    #endregion

                    foreach (ClienteDTO c in cli_por_usuarios)
                    {
                        if (cli_em_atendimento.Where(x => x.TelefoneCliente.Equals(c.TelefoneCliente)).FirstOrDefault() == null)
                        {
                            lista_igual = false;
                            break;
                        }
                    }

                    if (lista_igual)
                        ViewBag.ExisteDiferenca = "false";
                    else
                    {
                        Session["ClientesOnline"] = cli_por_usuarios;
                        ViewBag.ExisteDiferenca = "true";
                    }

                    return View(cli_por_usuarios);
                }
                #endregion

                var clientes_do_usuario = WWebSessaoGlobal.ClientesEmNegociacao.Where(c => c.UsuarioConversando.Us_ID.Equals(usuario.Us_ID)).ToList();
                if (clientes_do_usuario.Count > 0)
                {
                    List<ClienteDTO> clis = new List<ClienteDTO>();
                    if (cli_em_atendimento.Count != clientes_do_usuario.Count)
                    {
                        ViewBag.ExisteDiferenca = "true";
                        foreach (var item in clientes_do_usuario)
                            clis.Add(new ClienteDTO()
                            {
                                Cpf = item.ClienteConversando.Cpf,
                                NomeCliente = item.ClienteConversando.NomeCliente,
                                TelefoneCliente = item.ClienteConversando.TelefoneCliente,
                                DataHoraRegistro = Convert.ToDateTime(item.ClienteConversando.DataHoraRegistro),
                                DataVisualizacao = Convert.ToDateTime(item.ClienteConversando.DataVisualizacao)
                            });

                        Session.Add("ClientesOnline", clis);
                        return View(clis);
                    }

                    foreach (var c in cli_em_atendimento)
                    {
                        if (clientes_do_usuario.Where(cl => cl.ClienteConversando.TelefoneCliente.Equals(c.TelefoneCliente)).FirstOrDefault() == null)
                        {
                            ViewBag.ExisteDiferenca = "true";

                            foreach (var item in clientes_do_usuario)
                                clis.Add(new ClienteDTO()
                                {
                                    Cpf = item.ClienteConversando.Cpf,
                                    NomeCliente = item.ClienteConversando.NomeCliente,
                                    TelefoneCliente = item.ClienteConversando.TelefoneCliente,
                                    DataHoraRegistro = Convert.ToDateTime(item.ClienteConversando.DataHoraRegistro),
                                    DataVisualizacao = Convert.ToDateTime(item.ClienteConversando.DataVisualizacao)
                                });

                            Session.Add("ClientesOnline", clis);
                            return View(clis);
                        }
                    }

                    foreach (var item in clientes_do_usuario)
                        clis.Add(new ClienteDTO()
                        {
                            Cpf = item.ClienteConversando.Cpf,
                            NomeCliente = item.ClienteConversando.NomeCliente,
                            TelefoneCliente = item.ClienteConversando.TelefoneCliente,
                            DataHoraRegistro = Convert.ToDateTime(item.ClienteConversando.DataHoraRegistro),
                            DataVisualizacao = Convert.ToDateTime(item.ClienteConversando.DataVisualizacao)
                        });

                    ViewBag.ExisteDiferenca = "false";
                    Session.Add("ClientesOnline", clis);
                    return View(clis);
                }

                #region Se existir diferença entre cli_em_atendimento e cli_disponiveis, atualiza a fila de atendimento

                if (cli_em_atendimento.Count != cli.Count)
                {
                    Session["ClientesOnline"] = cli;
                    ViewBag.ExisteDiferenca = "true";

                    return View(cli);
                }

                #endregion

                foreach (ClienteDTO c in cli_em_atendimento)
                {
                    if (cli.Where(x => x.TelefoneCliente.Equals(c.TelefoneCliente)).FirstOrDefault() == null)
                    {
                        lista_igual = false;
                        break;
                    }
                }

                if (lista_igual)
                    ViewBag.ExisteDiferenca = "false";
                else
                {
                    Session["ClientesOnline"] = cli;
                    ViewBag.ExisteDiferenca = "true";
                }

                return View(cli);
            }
            catch { }

            return RedirectToAction("error");
        }

        public ActionResult RetornarTodosClientesOnlines()
        {
            return View(new Models.BLL.ClienteBLL().ListarClientesOnlines(2000));
        }

        public JsonResult RetornarFila(string telefone_cliente)
        {
            RespostaAutenticacao resposta = new RespostaAutenticacao();
            try
            {
                using (WWeb.UI.WebformMVC.Models.WWebEntities ctx = new Models.WWebEntities())
                {
                    var cli = ctx.Cliente.Where(c => c.TelefoneCliente.Equals(telefone_cliente)).FirstOrDefault();
                    if (cli != null)
                    {
                        cli.Pendente = true;
                        cli.Mensagem.Take(1).FirstOrDefault().Enviada = false;
                        
                        ctx.SaveChanges();

                        resposta.responseAutenticado = "1";
                        resposta.responseErro = "0";
                        resposta.responseMsg = "O cliente foi enviado para a fila de atendimento com sucesso. Favor realizar o atendimento do cliente.";
                        resposta.responseUrl = "";
                    }
                    else
                    {
                        resposta.responseAutenticado = "1";
                        resposta.responseErro = "0";
                        resposta.responseMsg = "O cliente não foi encontrado para atendimento";
                        resposta.responseUrl = "";
                    }
                }
            }
            catch (Exception ex)
            {
                resposta.responseAutenticado = "1";
                resposta.responseErro = "1";
                resposta.responseMsg = ex.Message;
                resposta.responseUrl = "";

                throw;
            }

            return Json(resposta);
        }
    }
}